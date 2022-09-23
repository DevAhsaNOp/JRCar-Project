using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.DAL.DBLayer
{
    public class PaymentDb
    {
        private jrcarEntities _context;

        public PaymentDb()
        {
            _context = new jrcarEntities();
        }

        public int InsertPayment(tblPayment model)
        {
            try
            {
                if (model != null)
                {
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.CreatedOn = DateTime.Now;
                    _context.tblPayments.Add(model);
                    Save();
                    return model.ID;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdatePayment(tblPayment model)
        {
            try
            {
                if (model != null)
                {
                    model.CreatedOn = GetPaymentById(model.ID).CreatedOn;
                    model.CreatedBy = GetPaymentById(model.ID).CreatedBy;
                    model.UpdatedOn = DateTime.Now;
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    Save();
                    return model.ID;
                }
                else
                    return 0;
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
                    var payment = _context.tblPayments.Where(x => x.ID == id).Select(a => new ValidationPayment()
                    {
                        ShowroomName = a.tblShowroom.FullName,
                        ShowroomNumber = a.tblShowroom.Contact,
                        ShowroomAddress = a.tblShowroom.ShopNumber + " " + a.tblShowroom.tblAddress.CompleteAddress,
                        FromDate = a.FromDate,
                        ToDate = a.ToDate,
                        Isactive = a.Isactive,
                        Isarchive = a.Isarchive,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        Recieved = a.Recieved,
                        Recievable = a.Recievable,
                        UpdatedOn = a.UpdatedOn.Value,
                        UpdatedBy = a.UpdatedBy
                    }).FirstOrDefault();
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

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<ValidationPayment> GetAllPayments()
        {
            var payment = _context.tblPayments.Select(a => new ValidationPayment()
            {
                ShowroomName = a.tblShowroom.FullName,
                ShowroomAddress = a.tblShowroom.ShopNumber + " " + a.tblShowroom.tblAddress.CompleteAddress,
                FromDate = a.FromDate,
                ToDate = a.ToDate,
                CreatedOn = a.CreatedOn,
                Recieved = a.Recieved,
                Recievable = a.Recievable,
            }).ToList();

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
