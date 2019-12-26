using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using K5OrderBeta.Models;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Net.Http;
using Newtonsoft.Json;

namespace K5OrderBeta.Database
{
    public class K5OrderingDatabase
    {
        public readonly SQLiteAsyncConnection database;

        static string currentUserCode;
        static string currentCustomerCode;
        static int currentTransaction;
        static int currentItem;
        
        static string port = ""; //2020
        static string ip = ""; //192.168.43.138
        static string ngrok = "http://192.168.43.138:2020/";
        static string loadStatus = "";
        static string additionalLoadInfo = "";
        public string CurrentUserCode { get { return currentUserCode; } set { currentUserCode = value; } }
        public string CurrentCustomerCode { get { return currentCustomerCode; } set { currentCustomerCode = value; } }
        public int CurrentTransaction { get { return currentTransaction; } set { currentTransaction = value; } }
        public int CurrentItem { get { return currentItem; } set { currentItem = value; } }
        public string Ngrok { get { return ngrok; } set { ngrok = value; } }
        public string IP { get { return ip; } set { ip = value; } }
        public string Port { get { return port; } set { port = value; } }
        public string LoadStatus { get { return loadStatus; } set { loadStatus = value; } }
        public string AdditionalLoadInfo { get { return additionalLoadInfo; } set { additionalLoadInfo = value; } }
        
        public K5OrderingDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<ProductMain>().Wait();
            database.CreateTableAsync<OrderMain>().Wait();
            database.CreateTableAsync<OrderSub>().Wait();
            database.CreateTableAsync<Customer>().Wait();
            database.CreateTableAsync<User>().Wait();
            database.CreateTableAsync<CartService>().Wait();
            database.CreateTableAsync<UserPrefs>().Wait();
            database.CreateTableAsync<IPConfig>().Wait();
        }

        public void AddProduct()
        {
            ProductMain product = new ProductMain();
            product.productCode = "000";
            product.productName = "Mentos Fruit 300s + FREE Mini Chupa Chups Tongue Painter 14bags (code 690170)";
            product.productImage = "ProductResources/OnonokiPP.jpg";
            product.productDesc = "Mentos ";
            product.productStat = 0;
            product.productIB = 1;
            product.productPC = 1;
            product.productCS = 1;
            SaveFirstProductAsync(product);
        }

        public void AddUser()
        {
            User user = new User()
            {
                userCode ="696969",
                userName ="Kenneth Lacaron",
                loginName = "Kenneth",
                password = "123"
            };
            SaveFirstUser(user);
        }
        public void AddCustomer()
        {
            Customer cust = new Customer()
            {
              customerCode = "0123",
              customerName = "Acebedo, Elma / Elma Dressed Chicken and Frozen Foods"
            };
            SaveFirstCustomer(cust);
        }
        public Task<int> AddAccessPref()
        {
            UserPrefs pref = new UserPrefs()
            {
                prefType = 99,
                timeLog = DateTime.Now,
                userCode = "Accessed"

            };
            return SaveFirstPref(pref);
        }

        public Task<int> SaveIPConfiguration(string ip , string port)
        {
            IPConfig localconfig = new IPConfig()
            {
                ConfigIP = ip,
                ConfigPort = port
            };
            return SaveFirstIPConfig(localconfig);
        }

        public Task<IPConfig> GetLatestConfig()
        {
            return database.Table<IPConfig>().OrderByDescending(x => x.ID).FirstOrDefaultAsync();
        }

        public Task<int> SaveFirstPref(UserPrefs pref)
        {
            return database.InsertAsync(pref);
        }
        public Task<int> SaveFirstIPConfig(IPConfig config)
        {
            return database.InsertAsync(config);
        }


        public Task<int> SaveFirstProductAsync(ProductMain prod)
        {
            return database.InsertAsync(prod);
        }

        public Task<int> SaveFirstUser(User user)
        {
            return database.InsertAsync(user);
        }

        public Task<int> SaveFirstCustomer(Customer cust)
        {
            return database.InsertAsync(cust);
        }

        public Task<int> SaveLogPref(int type)
        {
            //1 = Log in || 2 - Log Out || 3 - Import || 4 - Export || 5 - Start Transaction
            UserPrefs pref = new UserPrefs()
            {
                timeLog = DateTime.Now,
                prefType = type,
                userCode = currentUserCode
        };
         
            return database.InsertAsync(pref);
        }

        public Task<int> SaveLogPref(int type, string code)
        {
            // 1 = Log in || 2 - Log Out || 3 - Import || 4 - Export || 5 - Start Transaction || 6 - End Transaction
            UserPrefs pref = new UserPrefs()
            {
                timeLog = DateTime.Now,
                prefType = type,
                userCode = currentUserCode,
                customerCode = code
            };

            return database.InsertAsync(pref);
        }

        public async Task<UserPrefs> GetLogPref(int type)
        {
            //1 = Log in || 2 - Log Out || 3 - Import || 4 - Export || 5 - Start Transaction || 6 - End Transaction

            UserPrefs p = await database.Table<UserPrefs>().Where(x => x.prefType == type && x.userCode == currentUserCode).OrderByDescending(x => x.ID).FirstOrDefaultAsync();
            return p;
        }

        public async Task<UserPrefs> GetLogPref()
        {
            //1 = Log in || 2 - Log Out || 3 - Import || 4 - Export || 5 - Start Transaction

            UserPrefs p = await database.Table<UserPrefs>().OrderByDescending(x => x.ID).FirstOrDefaultAsync();
            return p; 
        }


        public Task<List<Customer>> GetAllCustomer()
        {
            //return database.Table<Employee>().ToListAsync();
            return database.QueryAsync<Customer>("SELECT * FROM [Customer]");
            //return database.Table<Employee>().ToArrayAsync();
        }

        public Task<Customer> GetCustomerByCode(string code)
        {
            //return database.Table<Employee>().ToListAsync();
            return database.Table<Customer>().Where(x => x.customerCode == code).FirstOrDefaultAsync();
            //return database.Table<Employee>().ToArrayAsync();
        }



        public Task<List<ProductMain>> GetAllProductsAsync()
        {
            return database.QueryAsync<ProductMain>("SELECT * FROM [ProductMain] WHERE[ProductStat] = 0");
        }

        public Task<List<OrderMain>> GetAllOrderMainQuery()
        {
            return database.QueryAsync<OrderMain>("SELECT * FROM [OrderMain]");
        }

        public Task<List<OrderSub>> GetAllOrderSubQuery()
        {
            return database.QueryAsync<OrderSub>("SELECT * FROM [OrderSub]");
        }

        public Task<List<OrderMain>> GetAllOrderMain()
        {
            return database.Table<OrderMain>().ToListAsync();
        }

        public Task<List<OrderSub>> GetAllOrderSub()
        {
            return database.Table<OrderSub>().ToListAsync();
        }


        public Task<List<GlobalReport>> GetAllMainTransaction(int type)
        {
            //type 0 = Active 1- Cancelled
            return database.QueryAsync<GlobalReport>("SELECT * FROM OrderMain INNER JOIN Customer ON OrderMain.custCode = Customer.customerCode WHERE OrderMain.userCode = ? AND orderStat = ? " ,App.K5OrderingDatabase.CurrentUserCode,type);
        }

        public Task<List<GlobalReport>> GetOrderDetailByCode(string docnum)
        {
            return database.QueryAsync<GlobalReport>("SELECT * FROM OrderSub INNER JOIN OrderMain ON OrderMain.docNum = OrderSub.docNum WHERE OrderSub.docNum = ?",docnum);
        }
        
        public Task<ProductMain> GetProductByCode(string code)
        {
            //return database.Table<Employee>().ToListAsync();
            return database.Table<ProductMain>().Where(i => i.productCode == code && i.productStat == 0).FirstOrDefaultAsync();
            //return database.Table<Employee>().ToArrayAsync();
        }

        public Task<OrderMain> GetOrderByDocnum(string doc)
        {
            //return database.Table<Employee>().ToListAsync();
            return database.Table<OrderMain>().Where(i => i.docNum == doc).FirstOrDefaultAsync();
            //return database.Table<Employee>().ToArrayAsync();
        }

        public Task<int> RemoveOrderByDocnum(OrderMain doc)
        {
            doc.orderStat = 1;
            return database.UpdateAsync(doc);
        }

        public Task<int> SaveOrderMain(OrderMain order)
        {
            return database.InsertAsync(order);
        }


        public Task<int> SaveOrderSub(OrderSub order)
        {
            return database.InsertAsync(order);
        }

        public Task<int> AddtoCart(CartService cart)
        {
            if(cart.productRowCart != 0)
            {
                return database.UpdateAsync(cart);
            }
            else
            {
                return database.InsertAsync(cart);
            }
        }
        public Task<int> EdittoCart(CartService cart)
        {
            return database.UpdateAsync(cart);
        }
        public Task<List<CartService>> GetCurrentCart()
        {
            return database.Table<CartService>().ToListAsync();
            //return database.QueryAsync<CartService>("SELECT * FROM [CartService]");
            //return database.Table<Employee>().ToArrayAsync();
        }

        public Task<int> NoItemsInCart()
        {
            return database.Table<CartService>().CountAsync();
        }

        public Task<OrderMain> GetSignatureByPO(string PONumber)
        {
            return database.Table<OrderMain>().Where(x=> x.poNumber == PONumber).FirstOrDefaultAsync();
        }


        public async Task<bool> ProccessAllTransacAsync(string PONumber)
        {
            string curDocm = DateTime.Now.Millisecond + App.K5OrderingDatabase.CurrentUserCode + App.K5OrderingDatabase.CurrentCustomerCode + PONumber;
            var q = await App.K5OrderingDatabase.GetOrderByDocnum(curDocm);
            if (q == null)
            {
                OrderMain orderMain = new OrderMain()
                {
                    docNum = curDocm,
                    custCode = App.K5OrderingDatabase.CurrentCustomerCode,
                    userCode = App.K5OrderingDatabase.CurrentUserCode,
                    poNumber = PONumber,
                    tDate = DateTime.Now,
                    pending = true
                    //signature = signature
                   
                };

                int k = 0;
                List<OrderSub> ordersub = new List<OrderSub>();
                foreach (var a in await App.K5OrderingDatabase.GetCurrentCart())
                {
                    OrderSub ord = new OrderSub()
                    {
                        rowNum = k,
                        docNum = curDocm,
                        productImage = a.productImage,
                        productName = a.productName,
                        productCode = a.productCode,
                        IB = a.IB,
                        PC = a.PC,
                        CS = a.CS
                    };

                    ordersub.Add(ord);
                    k++;
                }

                k = 0;
                App.K5OrderingDatabase.CurrentTransaction++;
                await App.K5OrderingDatabase.CheckOutCart(ordersub);
                await App.K5OrderingDatabase.SaveOrderMain(orderMain);
                //await App.K5OrderingDatabase.SaveLogPref(2,0);
                return true;

            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ProccessAllTransacAsync(string PONumber, byte [] signature)
        {
            string curDocm = DateTime.Now.Millisecond + App.K5OrderingDatabase.CurrentUserCode + App.K5OrderingDatabase.CurrentCustomerCode + PONumber;
            var q = await App.K5OrderingDatabase.GetOrderByDocnum(curDocm);
            if (q == null)
            {
                OrderMain orderMain = new OrderMain()
                {
                    docNum = curDocm,
                    custCode = App.K5OrderingDatabase.CurrentCustomerCode,
                    userCode = App.K5OrderingDatabase.CurrentUserCode,
                    poNumber = PONumber,
                    tDate = DateTime.Now,
                    pending = true,
                    signature = signature

                };

                int k = 0;
                List<OrderSub> ordersub = new List<OrderSub>();
                foreach (var a in await App.K5OrderingDatabase.GetCurrentCart())
                {
                    OrderSub ord = new OrderSub()
                    {
                        rowNum = k,
                        docNum = curDocm,
                        productImage = a.productImage,
                        productName = a.productName,
                        productCode = a.productCode,
                        IB = a.IB,
                        PC = a.PC,
                        CS = a.CS
                    };

                    ordersub.Add(ord);
                    k++;
                }

                k = 0;
                App.K5OrderingDatabase.CurrentTransaction++;
                await App.K5OrderingDatabase.CheckOutCart(ordersub);
                await App.K5OrderingDatabase.SaveOrderMain(orderMain);
                //await App.K5OrderingDatabase.SaveLogPref(2,0);
                return true;

            }
            else
            {
                return false;
            }
        }

        public async Task<string> IPMaker()
        {
            string url;

            IPConfig ipconfig = await App.K5OrderingDatabase.GetLatestConfig();

            if(ipconfig.ConfigPort == null || ipconfig.ConfigPort == string.Empty)
            {
                url = "http://" + ipconfig.ConfigIP;
            }
            else
            {
                url = "http://" + ipconfig.ConfigIP + ":" + ipconfig.ConfigPort;
            }

            return url;
        }

        public async Task<List<ProductMain>> GetAllSQLProducts()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync( await IPMaker() +"/API/Order/K5/Import/ProductMain");
            var data = JsonConvert.DeserializeObject<List<ProductMain>>(response);
            LoadStatus = response;
            await database.InsertAllAsync(data);
            return data;
        }
        //localhost:50255
        //65b87b25.ngrok.io
        public async Task<List<User>> GetAllSQLUser()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync( await IPMaker() + "/API/Order/K5/Import/User");
            LoadStatus = response;
            var data = JsonConvert.DeserializeObject<List<User>>(response);
            return data;
        }

        public async Task<List<Customer>> GetAllSQLCustomer()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync( await IPMaker() + "/API/Order/K5/Import/Customer");
            LoadStatus = response;
            var data = JsonConvert.DeserializeObject<List<Customer>>(response);
            return data;
        }

        public async Task<int> SaveAllImportedDataToSQLite(/*List<User> user,List<Customer> cust,List<ProductMain> prod*/)
        {

            var user = await GetAllSQLUser();
            var cust = await GetAllSQLCustomer();
            var prod = await GetAllSQLProducts();

            try {

                await ResetAfterImport();

                await database.InsertAllAsync(user);
                await database.InsertAllAsync(cust);
                await database.InsertAllAsync(prod);

                return 0;

            }
            catch (Exception)
            {
                return 1;
            }
         
        }




        public Task<int> CheckOutCart(List<OrderSub> order)
        {
            return database.InsertAllAsync(order);
        }

        public Task<int> RemoveItemInCart(int row)
        {
            return database.Table<CartService>().DeleteAsync(x => x.productRowCart == row);
        }

        public Task<User> CheckUserLogin(string user , string pass)
        {
            return database.Table<User>().Where(x=> x.loginName == user && x.password == pass).FirstOrDefaultAsync();
        }

        public Task<User> GetUserByCode(string code)
        {
            return database.Table<User>().Where(x => x.userCode == code).FirstOrDefaultAsync();
        }

        public Task<int> ResetTransaction()
        {
            App.K5OrderingDatabase.CurrentCustomerCode = null;
            //App.K5OrderingDatabase.CurrentUserCode = null;
            return database.DeleteAllAsync<CartService>();
        }

        public Task<int> Logout()
        {
            App.K5OrderingDatabase.CurrentCustomerCode = null;
            App.K5OrderingDatabase.CurrentUserCode = null;
            return database.DeleteAllAsync<CartService>();
        }


        public Task<int> ResetAfterExport()
        {
             database.DeleteAllAsync<CartService>();
             database.DeleteAllAsync<OrderMain>();
            return database.DeleteAllAsync<OrderSub>();
        }

        

        public async Task<bool> ResetAfterImport()
        {
            try
            {
                await database.DeleteAllAsync<CartService>();
                await database.DeleteAllAsync<OrderMain>();
                await database.DeleteAllAsync<OrderSub>();
                await database.DeleteAllAsync<ProductMain>();
                await database.DeleteAllAsync<Customer>();
                await database.DeleteAllAsync<User>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }


    }
}
