using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.DBLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
                        RecievableFromDate = model.RecievableFromDate,
                        RecievableToDate = model.RecievableToDate,
                        Recieved = model.Recieved,
                        RecievedFromDate = model.RecievedFromDate,
                        RecievedToDate = model.RecievedToDate,
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
                        RecievableFromDate = model.RecievableFromDate,
                        RecievableToDate = model.RecievableToDate,
                        Recieved = model.Recieved,
                        RecievedFromDate = model.RecievedFromDate,
                        RecievedToDate = model.RecievedToDate,
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

        public List<string> GetRecievableMonths(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var list = DbObj.GetRecievableMonths(ShowroomID);
                return list;
            }
            else
            {
                return null;
            }
        }

        public ValidationPayment GetPaymentID(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var payment = DbObj.GetPaymentID(ShowroomID);
                return payment;
            }
            else
            {
                return null;
            }
        }

        public bool ShowroomPaymentClear(ValidationPayment model)
        {
            try
            {
                if (model != null)
                {
                    tblPayment payment = new tblPayment()
                    {
                        ID = model.ID,
                        ShowroomID = model.ShowroomID,
                        Recievable = model.Recievable,
                        RecievableFromDate = model.RecievableFromDate,
                        RecievableToDate = model.RecievableToDate,
                        Recieved = model.Recieved,
                        RecievedFromDate = model.RecievedFromDate,
                        RecievedToDate = model.RecievedToDate,
                        UpdatedBy = model.UpdatedBy
                    };
                    var obj = DbObj.ShowroomPaymentClear(payment);
                    if (obj)
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
        
        public bool ShowroomPreviousPaymentGenerate(int ShowroomID)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    var obj = DbObj.ShowroomPreviousPaymentGenerate(ShowroomID);
                    if (obj)
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

        public string IsMonthPaymentRecieved(int ShowroomID, List<string> RMonths)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    return DbObj.IsMonthPaymentRecieved(ShowroomID, RMonths);
                }
                else
                    return null;
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
                    var payment = DbObj.GetPaymentById(id);
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
            var payment = DbObj.GetAllPayments();
            if (payment != null)
            {
                return payment;
            }
            else
            {
                return null;
            }
        }
        
        public ValidationPayment GetShowroomDetailById(int ShowroomID)
        {
            var payment = DbObj.GetShowroomDetailById(ShowroomID);
            if (payment != null)
            {
                return payment;
            }
            else
            {
                return null;
            }
        }
        
        public ValidationPayment GetShowroomDetailsById(int ShowroomID)
        {
            var payment = DbObj.GetShowroomDetailsById(ShowroomID);
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
