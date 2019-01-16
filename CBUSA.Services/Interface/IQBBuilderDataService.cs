using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Services.Interface
{
    public interface IQBBuilderDataService
    {
        //QBVendorDataReceived GetQBVendorDataReceived(Int64 TranId);
        void UpdateQBBuilderVendorData(QBVendorDataReceived ObjQBVendorDataReceived, bool DisposeConn = false);
        bool CheckBuilderAuthentication(Int64 BuilderId, string Passwrd);
        void UpdateQBBuilderBillData(QBBillDataReceived ObjQBBillDataReceived, bool DisposeConn = false);
        void UpdateQBBuilderCategoryData(QBCategoryDataReceived ObjQBCategoryDataReceived, bool DisposeConn = false);
        void UpdateQBBuilderInvoiceData(QBInvoiceDataReceived ObjQBInvoiceDataReceived, bool DisposeConn = false);
    }
}
