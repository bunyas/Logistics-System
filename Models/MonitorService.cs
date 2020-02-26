using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using mascis.ScheduledTasks;
using System.Data;
using System.Configuration;
using mascis.Hubs;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using TableDependency.SqlClient;
using Microsoft.AspNet.SignalR.Hubs;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient.Base.Enums;
using mascis.Models;

namespace mascis.Models
{
     
    public class MonitorService  
    {
        //public MonitorService(IHubConnectionContext<dynamic> clients) : base(clients)
        //{
        //}

        // Singleton instance
        private readonly static Lazy<MonitorService> _instance = new Lazy<MonitorService>(
            () => new MonitorService(GlobalHost.ConnectionManager.GetHubContext<MonitorHub>().Clients));

        private static SqlTableDependency<SAP_ExportHeader> _tableDependency;

        private MonitorService(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;

            var mapper = new ModelToTableMapper<SAP_ExportHeader>();
            mapper.AddMapping(s => s.OrderNumber, "OrderNumber");

            _tableDependency = new SqlTableDependency<SAP_ExportHeader>(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                "SAP_ExportHeader"/*, mapper*/);

            _tableDependency.OnChanged += SqlTableDependency_Changed;
            _tableDependency.OnError += SqlTableDependency_OnError;
            _tableDependency.Start();
        }

        public static MonitorService Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        public IEnumerable<SAP_ExportHeader> GetAllStocks()
        {
            var stockModel = new List<SAP_ExportHeader>();

            var connectionString = ConfigurationManager.ConnectionStrings
                    ["DefaultConnection"].ConnectionString;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT * FROM [SAP_ExportHeader]";

                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            var OrderNumber = sqlDataReader.GetString(sqlDataReader.GetOrdinal("OrderNumber"));
                            var RecordReadBySAP = sqlDataReader.GetBoolean(sqlDataReader.GetOrdinal("RecordReadBySAP"));
                            var DocNum = sqlDataReader.GetString(sqlDataReader.GetOrdinal("DocNum"));

                            stockModel.Add(new SAP_ExportHeader { OrderNumber = OrderNumber, RecordReadBySAP = Convert.ToBoolean(RecordReadBySAP), DocNum = DocNum });
                        }
                    }
                }
            }

            return stockModel;
        }

        void SqlTableDependency_OnError(object sender, ErrorEventArgs e)
        {
            throw e.Error;
        }

        /// <summary>
        /// Broadcast New Stock Price
        /// </summary>
        void SqlTableDependency_Changed(object sender, RecordChangedEventArgs<SAP_ExportHeader> e)
        {
            if (e.ChangeType != ChangeType.None)
            {
                BroadcastStockPrice(e.Entity);
            }
        }

        private void BroadcastStockPrice(SAP_ExportHeader stock)
        {
            Clients.All.updateStockPrice(stock);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tableDependency.Stop();
                }

                disposedValue = true;
            }
        }

        ~MonitorService()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}