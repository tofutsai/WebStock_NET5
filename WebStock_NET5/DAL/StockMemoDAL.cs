using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebStock_NET5.DB;

namespace WebStock_NET5.DAL
{
    public interface IStockMemoDAL
    {
        public bool Create(string type, string codes, string memoContent);
    }
    public class StockMemoDAL : IStockMemoDAL
    {
        private readonly WebStockContextDTO _db;
        public static IConfigurationRoot _config;
        public StockMemoDAL(WebStockContextDTO webStockContextDTO)
        {
            _db = webStockContextDTO;
            //引用json檔案的參數，使用.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //這種方法可以保證可以修改正在運行的.net core 程序，不需要重啟程序
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables();
            //通過 IConfigurationBuilder 物件建立 IConfigurationRoot 物件。
            builder.AddEnvironmentVariables();
            _config = builder.Build();
        }
        public bool Create(string type, string codes, string memoContent)
        {
            bool status = false;
            try
            {
                string sql = "select * from stockMemo";
                string[] stringSeparators = new string[] { "\r\n", "\n" };
                string[] codeArray = codes.Split(stringSeparators, StringSplitOptions.None);

                List<stockMemo> stockMemos = _db.stockMemo.FromSqlRaw(sql).ToList();
                //buckCopy Init
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(Int64));
                dt.Columns.Add("code", typeof(string));
                dt.Columns.Add("ext1", typeof(string));
                dt.Columns.Add("ext2", typeof(string));
                dt.Columns.Add("ext3", typeof(string));
                dt.Columns.Add("ext4", typeof(string));
                dt.Columns.Add("ext5", typeof(string));
                dt.Columns.Add("ext6", typeof(string));
                dt.Columns.Add("ext7", typeof(string));
                dt.Columns.Add("ext8", typeof(string));
                dt.Columns.Add("ext9", typeof(string));
                dt.Columns.Add("ext10", typeof(string));
                dt.Columns.Add("ext11", typeof(string));
                dt.Columns.Add("ext12", typeof(string));
                dt.Columns.Add("ext13", typeof(string));
                dt.Columns.Add("ext14", typeof(string));
                dt.Columns.Add("ext15", typeof(string));
                dt.Columns.Add("ext16", typeof(string));

                foreach (var item in _db.stockIndex.ToList())
                {
                    DataRow row = dt.NewRow();
                    row["code"] = item.code;
                    row["ext1"] = "";
                    row["ext2"] = "";
                    row["ext3"] = "";
                    row["ext4"] = "";
                    row["ext5"] = "";
                    row["ext6"] = "";
                    row["ext7"] = "";
                    row["ext8"] = "";
                    row["ext9"] = "";
                    row["ext10"] = "";
                    row["ext11"] = "";
                    row["ext12"] = "";
                    row["ext13"] = "";
                    row["ext14"] = "";
                    row["ext15"] = "";
                    row["ext16"] = "";
                    dt.Rows.Add(row);
                }
                _db.Database.ExecuteSqlRaw(@"truncate table stockMemo");

                foreach(DataRow dr in dt.Rows)
                {
                    foreach(var item in stockMemos)
                    {
                        if(dr["code"].ToString() == item.code)
                        {
                            dr["ext1"] = item.ext1;
                            dr["ext2"] = item.ext2;
                            dr["ext3"] = item.ext3;
                            dr["ext4"] = item.ext4;
                            dr["ext5"] = item.ext5;
                            dr["ext6"] = item.ext6;
                            dr["ext7"] = item.ext7;
                            dr["ext8"] = item.ext8;
                            dr["ext9"] = item.ext9;
                            dr["ext10"] = item.ext10;
                            dr["ext11"] = item.ext11;
                            dr["ext12"] = item.ext12;
                            dr["ext13"] = item.ext13;
                            dr["ext14"] = item.ext14;
                            dr["ext15"] = item.ext15;
                            dr["ext16"] = item.ext16;
                        }
                    }
                }

                foreach(var code in codeArray)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        if (dr["code"].ToString() == code)
                        {
                            switch (type)
                            {
                                case "ext1":
                                    dr["ext1"] = memoContent; break;
                                case "ext2":
                                    dr["ext2"] = memoContent; break;
                                case "ext3":
                                    dr["ext3"] = memoContent; break;
                                case "ext4":
                                    dr["ext4"] = memoContent; break;
                                case "ext5":
                                    dr["ext5"] = memoContent; break;
                                case "ext6":
                                    dr["ext6"] = memoContent; break;
                                case "ext7":
                                    dr["ext7"] = memoContent; break;
                                default: break;
                            }
                        }
                    }
                }
                //sqlBulkCopy 寫入資料Table
                string connectionstring = _config["ConnectionStrings:WebStockDatabase"];
                using (var sqlCopy = new SqlBulkCopy(connectionstring))
                {
                    sqlCopy.DestinationTableName = "[stockNow]";
                    sqlCopy.BatchSize = 2000;
                    sqlCopy.WriteToServer(dt);
                    status = true;
                }
                return status;
            }
            catch (Exception ex)
            {
                return status;
            }
        }
    }
}
