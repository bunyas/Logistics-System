using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using mascis.ScheduledTasks;

namespace mascis.Models
{
    public class MonitorTable
    {
        mascisEntities context = new mascisEntities();

        // private Boolean existing_records = false;

        private string mOrderNumber;
        private DateTime mAddedDate;
        private string mAddedBy;
        private Boolean? mCurrentReccord;
        private Boolean? mRecordReadBySAP;
        private string mDocNum;
        private DateTime? mDocDate;
        private DateTime? mDocDueDate;
        private string mCardCode;
        private int? mDocEntry;
        private int? mProductCategory;

        public string OrderNumber { get { return mOrderNumber; } set { mOrderNumber = value; } }
        public DateTime AddedDate { get { return mAddedDate; } set { mAddedDate = value; } }
        public string AddedBy { get { return mAddedBy; } set { mAddedBy = value; } }
        public Boolean? CurrentReccord { get { return mCurrentReccord; } set { mCurrentReccord = value; } }
        public Boolean? RecordReadBySAP { get { return mRecordReadBySAP; } set { mRecordReadBySAP = value; } }
        public string DocNum { get { return mDocNum; } set { mDocNum = value; } }
        public DateTime? DocDate { get { return mDocDate; } set { mDocDate = value; } }
        public DateTime? DocDueDate { get { return mDocDueDate; } set { mDocDueDate = value; } }
        public string CardCode { get { return mCardCode; } set { mCardCode = value; } }
        public int? DocEntry { get { return mDocEntry; } set { mDocEntry = value; } }
        public int? ProductCategory { get { return mProductCategory; } set { mProductCategory = value; } }

       
    }
}