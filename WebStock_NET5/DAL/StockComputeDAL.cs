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
using WebStock_NET5.DTO;

namespace WebStock_NET5.DAL
{
    public interface IStockComputeDAL
    {
        public bool stockAvgStatistics();
        public bool stockNowsStatistics();
    }
    public class StockComputeDAL:IStockComputeDAL
    {
        private readonly WebStockContextDTO _db;
        public static IConfigurationRoot _config;
        public StockComputeDAL(WebStockContextDTO webStockContextDTO)
        {
            _db = webStockContextDTO;
            //引用json檔案的參數
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables();

            builder.AddEnvironmentVariables();
            _config = builder.Build();
        }
        public bool stockAvgStatistics()
        {
            bool status = false;
            try
            {
                List<StockAvgDTO> stockAvgDTOs = new List<StockAvgDTO>();
                //buckCopy Init
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(Int64));
                dt.Columns.Add("code", typeof(string));
                dt.Columns.Add("avgPrice", typeof(double));
                dt.Columns.Add("highestPrice", typeof(double));
                dt.Columns.Add("lowestPrice", typeof(double));
                dt.Columns.Add("avgShares", typeof(Int64));
                dt.Columns.Add("avgTurnover", typeof(double));

                var sys = _db.sysConfig.FirstOrDefault();
                string sql = $"select s.code,year(dataDate) as dataYear, round(avg(closeprice),2) as avgPrice, MAX(closeprice) as highestPrice, MIN(closeprice) as lowestPrice, round(avg(shares), 2) as avgShares, round(avg(turnover), 2) as avgTurnover " +
                                     $"from stockData s where dataDate between '{sys.avgStartDate.ToShortDateString()}' and '{sys.avgEndDate.ToShortDateString()}' group by year(dataDate), s.code order by s.code, dataYear";
                stockAvgDTOs = _db.StockAvgDTO.FromSqlRaw(sql).ToList();

                foreach (var item in _db.stockIndex.ToList())
                {
                    double fiveYearsAvgPrice = 0;
                    double fiveYearsAvghighestPrice = 0;
                    double fiveYearsAvglowestPrice = 0;
                    int fiveYearsAvgShares = 0;
                    double fiveYearsAvgTurnOver = 0;
                    var singleCodeStock = stockAvgDTOs.Where(x => x.code == item.code).AsEnumerable();
                    if (singleCodeStock.Count() == 0)
                        continue;
                    foreach (var data in singleCodeStock)
                    {
                        fiveYearsAvgPrice += data.avgPrice;
                        fiveYearsAvghighestPrice += data.highestPrice;
                        fiveYearsAvglowestPrice += data.lowestPrice;
                        fiveYearsAvgShares += data.avgShares;
                        fiveYearsAvgTurnOver += data.avgTurnover;
                    }
                    fiveYearsAvgPrice = Math.Round(fiveYearsAvgPrice / singleCodeStock.Count(), 2);
                    fiveYearsAvghighestPrice = Math.Round(fiveYearsAvghighestPrice / singleCodeStock.Count(), 2);
                    fiveYearsAvglowestPrice = Math.Round(fiveYearsAvglowestPrice / singleCodeStock.Count(), 2);
                    fiveYearsAvgShares = fiveYearsAvgShares / singleCodeStock.Count();
                    fiveYearsAvgTurnOver = Math.Round(fiveYearsAvgTurnOver / singleCodeStock.Count(), 2);
                    //BuckCopy寫入結果
                    DataRow row = dt.NewRow();
                    row["code"] = item.code;
                    row["avgPrice"] = fiveYearsAvgPrice;
                    row["highestPrice"] = fiveYearsAvghighestPrice;
                    row["lowestPrice"] = fiveYearsAvglowestPrice;
                    row["avgShares"] = fiveYearsAvgShares;
                    row["avgTurnover"] = fiveYearsAvgTurnOver;
                    dt.Rows.Add(row);
                }
                //清空資料Table
                _db.Database.ExecuteSqlRaw(@"truncate table stockAvg");
                //sqlBulkCopy 寫入資料Table
                string connectionstring = _config["ConnectionStrings:WebStockDatabase"];
                using (var sqlCopy = new SqlBulkCopy(connectionstring))
                {
                    sqlCopy.DestinationTableName = "[stockAvg]";
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

        public bool stockNowsStatistics()
        {
            bool status = false;
            try
            {
                List<StockNowDTO> stockNowDTOs = new List<StockNowDTO>();
                string sql = @"SELECT
                              	 i.code AS code
                                 ,dt.dataDate AS dataDate
                                 ,dt.closePrice AS closePrice
                                 ,a.highestPrice AS highestPrice
                                 ,a.lowestPrice AS lowestPrice
                              FROM stockIndex i
                              JOIN stockDataTmp dt
                              	ON i.code = dt.code
                              JOIN stockAvg a
                              	ON i.code = a.code
                              UNION
                              SELECT
                              	 i.code AS code
                                 ,dto.dataDate AS dataDate
                                 ,dto.closePrice AS closePrice
                                 ,a.highestPrice AS highestPrice
                                 ,a.lowestPrice AS lowestPrice
                              FROM stockIndex i
                              JOIN stockDataTmpOtc dto
                              	ON i.code = dto.code
                              JOIN stockAvg a
                              	ON i.code = a.code
                              ORDER BY i.code";
                stockNowDTOs = _db.StockNowDTO.FromSqlRaw(sql).ToList();

                //buckCopy Init
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(Int64));
                dt.Columns.Add("code", typeof(string));
                dt.Columns.Add("closePrice", typeof(double));
                dt.Columns.Add("position", typeof(double));
                dt.Columns.Add("dataDate", typeof(DateTime));

                foreach (var item in _db.stockIndex.ToList())
                {
                    var stockNowDTO = stockNowDTOs.Where(x => x.code == item.code).AsEnumerable();
                    if (stockNowDTO == null)
                        continue;
                    foreach (var data in stockNowDTO)
                    {
                        //BuckCopy寫入結果
                        DataRow row = dt.NewRow();
                        row["code"] = data.code;
                        row["closePrice"] = data.closePrice;
                        row["position"] = Math.Round((data.closePrice - data.lowestPrice) / (data.highestPrice - data.lowestPrice), 2);
                        row["dataDate"] = data.dataDate;
                        dt.Rows.Add(row);
                    }
                }
                //再foreach尚未更新前的stockNow list，找出code不存在本次list，補充寫入上去dt
                foreach (var item in _db.stockNow.ToList())
                {
                    var stockNowDTO = stockNowDTOs.Where(x => x.code == item.code).AsEnumerable();
                    if (stockNowDTO.Count() == 0)
                    {
                        DataRow row = dt.NewRow();
                        row["code"] = item.code;
                        row["closePrice"] = item.closePrice;
                        row["position"] = item.position;
                        row["dataDate"] = item.dataDate;
                        dt.Rows.Add(row);
                    }
                }
                _db.Database.ExecuteSqlRaw(@"truncate table stockNow");
                //sqlBulkCopy 寫入資料Table
                string connectionstring = _config["ConnectionStrings:WebStockDatabase"];
                using (var sqlCopy = new SqlBulkCopy(connectionstring))
                {
                    sqlCopy.DestinationTableName = "[stockNow]";
                    sqlCopy.BatchSize = 2000;
                    sqlCopy.WriteToServer(dt);
                    status = true;
                }
                var sysConfig = _db.sysConfig.FirstOrDefault();
                var stockNow = _db.stockNow.Where(x => x.id == 1).FirstOrDefault();
                sysConfig.nowDate = stockNow.dataDate;
                _db.SaveChanges();
                return status;
            }
            catch (Exception ex)
            {
                return status;
            }
        }
    }
}
