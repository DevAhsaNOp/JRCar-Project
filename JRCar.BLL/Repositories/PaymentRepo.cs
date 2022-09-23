using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.DBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.BLL.Repositories
{
    public class PaymentRepo
    {
        private PaymentDb DbObj;

        public PaymentRepo()
        {
            DbObj = new PaymentDb();
        }

        public bool InsertPayment(ValidationPayment model)
        {
            try
            {
                if (model != null)
                {
                    tblPayment payment = new tblPayment()
                    {
                        ShowroomID = model.ShowroomID,
                        Recievable = model.Recievable,
                        Recieved = model.Recieved,
                        FromDate = model.FromDate,
                        ToDate = model.ToDate,
                        CreatedBy = model.CreatedBy                        
                    };
                    var obj = DbObj.InsertPayment(payment);
                    if (obj > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdatePayment(tblPayment model)
        {
            try
            {
                if (model != null)
                {
                    tblPayment payment = new tblPayment()
                    {
                        ShowroomID = model.ShowroomID,
                        Recievable = model.Recievable,
                        Recieved = model.Recieved,
                        FromDate = model.FromDate,
                        ToDate = model.ToDate,
                        UpdatedBy = model.UpdatedBy
                    };
                    var obj = DbObj.UpdatePayment(payment);
                    if (obj > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ValidationPayment GetPaymentById(int id)
        {
            try
            {
                if (id > 0)
                {
                    var payment = GetPaymentById(id);
                    if (payment != null)
                        return payment;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ValidationPayment> GetAllPayments()
        {
            var payment = GetAllPayments();
            if (payment != null)
            {
                return payment;
            }
            else
            {
                return null;
            }
        }
    }
}
