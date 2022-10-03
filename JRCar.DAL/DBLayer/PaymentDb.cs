﻿using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.CreatedOn = GetPaymentById(model.ID).CreatedOn;
                    model.CreatedBy = GetPaymentById(model.ID).CreatedBy;
                    model.UpdatedOn = DateTime.Now.ToString();
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
                        RecievableFromDate = a.RecievableFromDate.Value,
                        RecievableToDate = a.RecievableToDate.Value,
                        Isactive = a.Isactive,
                        Isarchive = a.Isarchive,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        Recieved = a.Recieved,
                        Recievable = a.Recievable,
                        UpdatedOn = a.UpdatedOn,
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

        public IEnumerable<ValidationPayment> GetShowroomPaymentById(int ShowroomID)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    var payment = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).Select(a => new ValidationPayment()
                    {
                        ID = a.ID,
                        ShowroomID = a.ShowroomID,
                        ShowroomName = a.tblShowroom.FullName,
                        ShowroomNumber = a.tblShowroom.Contact,
                        ShowroomAddress = a.tblShowroom.ShopNumber + " " + a.tblShowroom.tblAddress.CompleteAddress,
                        RecievableFromDate = a.RecievableFromDate.Value,
                        RecievableToDate = a.RecievableToDate.Value,
                        RecievedFromDate = a.RecievedFromDate.Value,
                        RecievedToDate = a.RecievedToDate.Value,
                        Isactive = a.Isactive,
                        Isarchive = a.Isarchive,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        Recieved = a.Recieved,
                        Recievable = a.Recievable,
                        UpdatedOn = a.UpdatedOn,
                        UpdatedBy = a.UpdatedBy
                    }).ToList();
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

        public bool ShowroomPreviousPaymentGenerate(int ShowroomID)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    var payment = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).FirstOrDefault();
                    if (payment != null)
                    {
                        var OldPayments = GetShowroomPaymentById(ShowroomID).FirstOrDefault();
                        if (OldPayments != null)
                        {
                            var StartDate = Convert.ToDateTime("1/4/2022");
                            var TodaysDate = DateTime.Now;
                            var MonthsBetweenDates = MonthsBetween(StartDate, TodaysDate);

                            #region **If Showroom Doesn't Pay Any Payment Yet**

                            var OldPaymentFRecieve = OldPayments.RecievedFromDate.ToString("dd/MM/yyyy");
                            var OldPaymentTRecieve = OldPayments.RecievedToDate.ToString("dd/MM/yyyy");
                            var IsNotMonth = "01/01/0001";

                            if (OldPaymentFRecieve == IsNotMonth && OldPaymentTRecieve == IsNotMonth)
                            {
                                var FirstMonth = MonthsBetweenDates.First();
                                var LastMonth = MonthsBetweenDates.Last();
                                var firstmonth = Convert.ToDateTime(FirstMonth.Month + " " + FirstMonth.Year);
                                var lastmonth = Convert.ToDateTime(LastMonth.Month + " " + LastMonth.Year);
                                var firstmonthStart = new DateTime(firstmonth.Year, firstmonth.Month, 1);
                                var lastmonthStart = new DateTime(lastmonth.Year, lastmonth.Month, 1);
                                var lastmonthEnd = lastmonthStart.AddMonths(1).AddDays(-1);
                                var UnionID = _context.tblUnions.Max(x => x.ID);

                                var Payments = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).First();
                                Payments.RecievableFromDate = firstmonthStart;
                                Payments.RecievableToDate = lastmonthEnd;
                                Payments.Recievable = null;
                                Payments.UpdatedBy = UnionID;
                                Payments.UpdatedOn = DateTime.Now.ToString();

                                if (Payments != null)
                                {
                                    var obj = UpdatePayment(Payments);
                                    if (obj > 0)
                                        return true;
                                    else
                                        return false;
                                }
                                else
                                    return false;
                            }

                            #endregion

                            else
                            {
                                List<DatesD> WantToBeRecievableMonths = new List<DatesD>();

                                var RecievestartDate = OldPayments.RecievedFromDate.Date;
                                var RecieveendDate = OldPayments.RecievedToDate.Date;
                                var RecievablestartDate = OldPayments.RecievableFromDate.Date;
                                var RecievableendDate = OldPayments.RecievableToDate.Date;

                                var Recievedmonths = MonthsBetween(RecievestartDate, RecieveendDate);
                                var Recievablemonths = MonthsBetween(RecievablestartDate, RecievableendDate).Concat(MonthsBetweenDates).Distinct();
                                //var WantToBeRecievableMonths = Recievablemonths.Concat(Recievedmonths);
                                
                                foreach (var months in Recievablemonths)
                                {
                                    foreach (var item in Recievedmonths)
                                    {
                                        if (item.Month == months.Month && item.Year == months.Year )
                                        {

                                        }
                                        else
                                        {
                                            WantToBeRecievableMonths.Add(new DatesD()
                                            {
                                                Month = months.Month,
                                                Year = months.Year,
                                            });
                                        }
                                    }
                                }

                                return true;
                            }

                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        var StartDate = Convert.ToDateTime("1/4/2022");
                        var TodaysDate = DateTime.Now;
                        var MonthsBetweenDates = MonthsBetween(StartDate, TodaysDate);
                        var FirstMonth = MonthsBetweenDates.First();
                        var LastMonth = MonthsBetweenDates.Last();
                        var firstmonth = Convert.ToDateTime(FirstMonth.Month + " " + FirstMonth.Year);
                        var lastmonth = Convert.ToDateTime(LastMonth.Month + " " + LastMonth.Year);
                        var firstmonthStart = new DateTime(firstmonth.Year, firstmonth.Month, 1);
                        var lastmonthStart = new DateTime(lastmonth.Year, lastmonth.Month, 1);
                        var lastmonthEnd = lastmonthStart.AddMonths(1).AddDays(-1);
                        var UnionID = _context.tblUnions.Max(x => x.ID);

                        tblPayment tbpay = new tblPayment()
                        {
                            ShowroomID = ShowroomID,
                            Recievable = null,
                            RecievableFromDate = firstmonthStart,
                            RecievableToDate = lastmonthEnd,
                            Recieved = null,
                            RecievedFromDate = null,
                            RecievedToDate = null,
                            CreatedBy = UnionID,
                        };
                        var obj = InsertPayment(tbpay);
                        if (obj > 0)
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ValidationPayment GetShowroomDetailById(int ShowroomID)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    var OldPendingPayment = GetShowroomPaymentById(ShowroomID).Select(s => new { s.RecievableFromDate, s.RecievableToDate, s.Recievable }).ToList();
                    if (OldPendingPayment.Count > 0 && OldPendingPayment != null)
                    {
                        List<DatesD> Datelist = new List<DatesD>();
                        decimal RecievableAmnt = 0;

                        foreach (var OldPayment in OldPendingPayment)
                        {
                            var startDate = OldPayment.RecievableFromDate.Date;
                            var endDate = OldPayment.RecievableToDate.Date;
                            var months = MonthsBetween(startDate, endDate);
                            foreach (var item in months)
                            {
                                Datelist.Add(new DatesD()
                                {
                                    Month = item.Month,
                                    Year = item.Year,
                                });
                            }
                            RecievableAmnt += OldPayment.Recievable.Value;
                        }


                        var payment = _context.tblShowrooms.Where(x => x.ID == ShowroomID).Select(a => new ValidationPayment()
                        {
                            ShowroomName = a.FullName,
                            ShowroomNumber = a.Contact,
                            ShowroomAddress = a.ShopNumber + " " + a.tblAddress.CompleteAddress,
                            Recievable = RecievableAmnt
                        }).FirstOrDefault();

                        payment.RecievableDate = Datelist;

                        if (payment != null)
                            return payment;
                        else
                            return null;
                    }
                    else
                    {
                        var payment = _context.tblShowrooms.Where(x => x.ID == ShowroomID).Select(a => new ValidationPayment()
                        {
                            ShowroomName = a.FullName,
                            ShowroomNumber = a.Contact,
                            ShowroomAddress = a.ShopNumber + " " + a.tblAddress.CompleteAddress,
                        }).FirstOrDefault();

                        if (payment != null)
                            return payment;
                        else
                            return null;
                    }
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
                RecievableFromDate = a.RecievedFromDate.Value,
                RecievedToDate = a.RecievableToDate.Value,
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

        public static IEnumerable<(string Month, int Year)> MonthsBetween(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return (
                    dateTimeFormat.GetMonthName(iterator.Month),
                    iterator.Year
                );

                iterator = iterator.AddMonths(1);
            }
        }
    }
}
