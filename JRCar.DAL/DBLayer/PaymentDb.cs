using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

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

        public List<string> GetRecievableMonths(int ShowroomID)
        {
            if (ShowroomID > 0)
            {
                var OldPendingPayment = GetShowroomPaymentById(ShowroomID).Select(s => new { s.RecievableFromDate, s.RecievableToDate }).FirstOrDefault();
                if (OldPendingPayment.RecievableFromDate != null && OldPendingPayment.RecievableToDate != null)
                {
                    var startDate = OldPendingPayment.RecievableFromDate.Value.Date;
                    var endDate = OldPendingPayment.RecievableToDate.Value.Date;
                    var months = MonthsBetween(startDate, endDate);
                    List<string> list = new List<string>();
                    foreach (var item in months)
                    {
                        list.Add(item.Month + " " + item.Year);
                    }
                    return list;
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        public bool ShowroomPaymentClear(tblPayment model)
        {
            try
            {
                if (model != null)
                {
                    if (model.RecievedFromDate != null && model.RecievedToDate != null)
                    {
                        var Sfirstmonth = Convert.ToDateTime("April" + " " + "2022");
                        var StartDate = new DateTime(Sfirstmonth.Year, Sfirstmonth.Month, 1);
                        var TodaysDate = DateTime.Now.AddMonths(1);
                        var MonthsBetweenDates = MonthsBetween(StartDate, TodaysDate);

                        var RecievestartDate = model.RecievedFromDate.Value.Date;
                        var RecieveendDate = model.RecievedToDate.Value.Date;
                        var Recievedmonths = MonthsBetween(RecievestartDate, RecieveendDate);

                        var IsPaidNextMonth = MonthsBetweenDates.All(Recievedmonths.Contains);

                        if (IsPaidNextMonth)
                        {
                            model.RecievableFromDate = null;
                            model.RecievableToDate = null;
                        }
                        else
                        {
                            model.RecievableToDate = DateTime.Now.AddMonths(1).AddDays(-1);
                        }
                    }
                    model.Isactive = true;
                    model.Isarchive = false;
                    model.CreatedOn = GetPaymentById(model.ID).CreatedOn;
                    model.CreatedBy = GetPaymentById(model.ID).CreatedBy;
                    model.UpdatedOn = DateTime.Now.ToString();
                    _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    Save();
                    return true;
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

        public string IsMonthPaymentRecieved(int ShowroomID, List<string> RMonths)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    var ErrorMsg = "";
                    var PaidMonths = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).Select(x => new { x.RecievedFromDate, x.RecievedToDate }).FirstOrDefault();
                    var MonthsBetweenDates = MonthsBetween(PaidMonths.RecievedFromDate.Value, PaidMonths.RecievedToDate.Value).Select(x => x.Month + " " + x.Year);

                    if (PaidMonths != null)
                    {
                        foreach (var month in MonthsBetweenDates)
                        {
                            foreach (var item in RMonths)
                            {
                                if (item == month)
                                {
                                    ErrorMsg += month +", ";
                                }
                                else
                                {
                                    ErrorMsg += " ";
                                }
                            }
                        }
                    }

                    if (ErrorMsg.Trim() != "")
                        return ErrorMsg.Trim() + " fees already paid";
                    else
                        return "";
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
        
        public IEnumerable<tblShowroom> GetAllShowRoom()
        {
            try
            {
               var reas = _context.tblPayments.Select(x => x.tblShowroom).ToList();
               return reas;
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
                            var Sfirstmonth = Convert.ToDateTime("April" + " " + "2022");
                            var StartDate = new DateTime(Sfirstmonth.Year, Sfirstmonth.Month, 1);
                            var TodaysDate = DateTime.Now;
                            var MonthsBetweenDates = MonthsBetween(StartDate, TodaysDate);

                            #region **If Showroom Doesn't Pay Any Payment Yet**

                            var OldPaymentFRecieve = (OldPayments.RecievedFromDate == null) ? "01/01/0001" : OldPayments.RecievedFromDate.Value.ToString("dd/MM/yyyy");
                            var OldPaymentTRecieve = (OldPayments.RecievedToDate == null) ? "01/01/0001" : OldPayments.RecievedToDate.Value.ToString("dd/MM/yyyy");
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
                                        return false;
                                    else
                                        return false;
                                }
                                else
                                    return false;
                            }

                            #endregion

                            #region **Showroom Payments Management**

                            else
                            {
                                var RecievestartDate = OldPayments.RecievedFromDate.Value.Date;
                                var RecieveendDate = OldPayments.RecievedToDate.Value.Date;
                                var Recievedmonths = MonthsBetween(RecievestartDate, RecieveendDate);

                                /*If Showroom Pays Some of the Month Payment But Not Current or Previous Months*/
                                if (OldPayments.RecievableFromDate != null && OldPayments.RecievableToDate != null)
                                {
                                    var RecievablestartDate = OldPayments.RecievableFromDate.Value.Date;
                                    var RecievableendDate = OldPayments.RecievableToDate.Value.Date;
                                    var Recievablemonths = MonthsBetween(RecievablestartDate, RecievableendDate).Concat(MonthsBetweenDates).Distinct();
                                    var WantToBeRecievableMonths = Recievablemonths.Except(Recievedmonths).OrderBy(m => DateTime.Parse(m.Month + " " + m.Year)).ToList();

                                    var FirstMonth = WantToBeRecievableMonths.First();
                                    var LastMonth = WantToBeRecievableMonths.Last();
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
                                            return false;
                                        else
                                            return false;
                                    }
                                    else
                                        return false;
                                }
                                /*If Showroom Pays All months payment including Current Months*/
                                else
                                {
                                    var RecievedInCurrentMonthDates = Recievedmonths.Except(MonthsBetweenDates);
                                    var RecievedLastMonth = DateTime.ParseExact(RecievedInCurrentMonthDates.Last().Month, "MMMM", CultureInfo.CurrentCulture).Month;
                                    if (RecievedLastMonth > DateTime.Now.Month)
                                        return true;
                                    else
                                        return false;
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        var StartDate = Convert.ToDateTime("April" + " " + "2022");
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
                    var OldPendingPayment = GetShowroomPaymentById(ShowroomID).Select(s => new { s.RecievableFromDate, s.RecievableToDate, s.Recievable }).FirstOrDefault();
                    if (OldPendingPayment != null)
                    {
                        List<DatesD> Datelist = new List<DatesD>();
                        decimal RecievableAmnt = 0;

                        if (OldPendingPayment.RecievableFromDate != null && OldPendingPayment.RecievableToDate != null)
                        {
                            var startDate = OldPendingPayment.RecievableFromDate.Value.Date;
                            var endDate = OldPendingPayment.RecievableToDate.Value.Date;
                            var months = MonthsBetween(startDate, endDate);
                            foreach (var item in months)
                            {
                                Datelist.Add(new DatesD()
                                {
                                    Month = item.Month,
                                    Year = item.Year,
                                });
                            }
                            RecievableAmnt = 0;
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
                tblShowroom = a.tblShowroom,
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

        public ValidationPayment GetPaymentID(int ShowroomID)
        {
            var payment = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).Select(x => new ValidationPayment() { ID = x.ID, RecievedFromDate = x.RecievedFromDate, Recieved = x.Recieved, Recievable = x.Recievable }).FirstOrDefault();

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

        public ValidationPayment GetShowroomDetailsById(int ShowroomID)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    var OldPayment = GetShowroomPaymentById(ShowroomID).Select(s => new { s.RecievableFromDate, s.RecievableToDate, s.RecievedFromDate, s.RecievedToDate,s.Recieved, s.Recievable }).FirstOrDefault();
                    if (OldPayment != null)
                    {
                        List<DatesD> Datelist = new List<DatesD>();
                        List<DatesD> Datelist2 = new List<DatesD>();
                        List<DatesD> Datelist3 = new List<DatesD>();
                        decimal RecievableAmnt = 0;
                        decimal RecievedAmnt = 0;

                        if (OldPayment.RecievableFromDate != null && OldPayment.RecievableToDate != null)
                        {
                            var startDate = OldPayment.RecievableFromDate.Value.Date;
                            var endDate = OldPayment.RecievableToDate.Value.Date;
                            var months = MonthsBetween(startDate, endDate);
                            foreach (var item in months)
                            {
                                Datelist.Add(new DatesD()
                                {
                                    Month = item.Month,
                                    Year = item.Year,
                                    IsPaid = false,
                                });
                            }
                            RecievableAmnt += OldPayment.Recievable.Value;
                        }
                        
                        if (OldPayment.RecievedFromDate != null && OldPayment.RecievedToDate != null)
                        {
                            var startDate = OldPayment.RecievedFromDate.Value.Date;
                            var endDate = OldPayment.RecievedToDate.Value.Date;
                            var months = MonthsBetween(startDate, endDate);
                            foreach (var item in months)
                            {
                                Datelist2.Add(new DatesD()
                                {
                                    Month = item.Month,
                                    Year = item.Year,
                                    IsPaid = true,
                                });
                            }
                            RecievedAmnt += OldPayment.Recieved.Value;
                        }

                        var payment = _context.tblShowrooms.Where(x => x.ID == ShowroomID).Select(a => new ValidationPayment()
                        {
                            ShowroomName = a.FullName,
                            ShowroomNumber = a.Contact,
                            ShowroomAddress = a.ShopNumber + " " + a.tblAddress.CompleteAddress,
                            tblShowroom = a.tblPayments.FirstOrDefault().tblShowroom,
                            Recievable = RecievableAmnt,
                            Recieved = RecievedAmnt
                        }).FirstOrDefault();

                        payment.RecievableDate = Datelist;
                        payment.RecievedDates = Datelist2;
                        payment.datesDs = Enumerable.Concat(Datelist, Datelist2).OrderBy(m => DateTime.Parse(m.Month + " " + m.Year)).ToList();

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
    }
}
