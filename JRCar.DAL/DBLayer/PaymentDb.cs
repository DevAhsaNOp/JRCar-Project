using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using System;
using System.Collections;
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

        public bool DeletePayment(int ShowroomID)
        {
            try
            {
                if (ShowroomID > 0)
                {
                    var reas = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).FirstOrDefault();
                    if (reas.RecievedFromDate == null)
                    {
                        _context.tblPayments.Remove(reas);
                        Save();
                        return true;
                    }
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
                var OldPendingPayment = GetShowroomPaymentById(ShowroomID).Select(s => new { s.RecievableFromDate, s.RecievableToDate }).ToList();
                if (OldPendingPayment.LastOrDefault().RecievableFromDate != null && OldPendingPayment.LastOrDefault().RecievableToDate != null)
                {
                    var startDate = OldPendingPayment.LastOrDefault().RecievableFromDate.Value.Date;
                    var endDate = OldPendingPayment.LastOrDefault().RecievableToDate.Value.Date;
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
                    var ErrorMsg2 = "";
                    IEnumerable<string> MonthsBetweenDates = null;
                    IEnumerable<string> MonthsBetweenUDates = null;
                    IEnumerable<string> IsUnPaidMonths = null;
                    IEnumerable<string> UnPaidMonths = null;
                    IEnumerable<string> NoPaidMonths = null;
                    var ValSelected = Convert.ToDateTime(RMonths.Last());
                    var PaidMonths = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).Select(x => new { x.RecievedFromDate, x.RecievedToDate }).ToList();
                    if (PaidMonths.Count > 1)
                    {
                        if (PaidMonths.FirstOrDefault().RecievedFromDate == null)
                        {
                            var objDel = DeletePayment(ShowroomID);
                            PaidMonths = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).Select(x => new { x.RecievedFromDate, x.RecievedToDate }).ToList();
                        }
                    }
                    if (PaidMonths.LastOrDefault().RecievedFromDate == null)
                    {
                        MonthsBetweenUDates = MonthsBetween(Convert.ToDateTime("April" + " " + "2022"), ValSelected).Select(x => x.Month + " " + x.Year);
                        NoPaidMonths = MonthsBetweenUDates.Except(RMonths);
                    }
                    else
                    {
                        MonthsBetweenDates = MonthsBetween(PaidMonths.FirstOrDefault().RecievedFromDate.Value, PaidMonths.LastOrDefault().RecievedToDate.Value).Select(x => x.Month + " " + x.Year);
                        MonthsBetweenUDates = MonthsBetween(PaidMonths.FirstOrDefault().RecievedFromDate.Value, ValSelected).Select(x => x.Month + " " + x.Year);
                        IsUnPaidMonths = MonthsBetweenUDates.Except(MonthsBetweenDates);
                        UnPaidMonths = IsUnPaidMonths.Except(RMonths);
                    }

                    if (PaidMonths != null && PaidMonths.LastOrDefault().RecievedFromDate != null)
                    {
                        foreach (var month in MonthsBetweenDates)
                        {
                            foreach (var item in RMonths)
                            {
                                if (item == month)
                                {
                                    ErrorMsg += month + ", ";
                                }
                                else
                                {
                                    ErrorMsg += " ";
                                }
                            }
                        }
                    }

                    if (UnPaidMonths != null)
                    {
                        foreach (var item in UnPaidMonths)
                        {
                            ErrorMsg2 += item + " ";
                        }
                    }

                    if (NoPaidMonths != null)
                    {
                        foreach (var item in NoPaidMonths)
                        {
                            ErrorMsg2 += item + " ";
                        }
                    }

                    if (ErrorMsg.Trim() != "" && ErrorMsg2.Trim() != "")
                        return ErrorMsg.Trim() + " fees already paid and " + ErrorMsg2.Trim() + " fees is not paid!";
                    else if (ErrorMsg.Trim() != "")
                        return ErrorMsg.Trim() + " fees already paid";
                    else if (ErrorMsg2.Trim() != "")
                        return ErrorMsg2.Trim() + " fees is not paid!";
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
                        Recievable = a.Recievable,
                        Recieved = a.Recieved,
                        Discount = a.Discount,
                        Balance = a.Balance,
                        Isactive = a.Isactive,
                        Isarchive = a.Isarchive,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
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
                var reas = _context.tblPayments.Select(x => x.tblShowroom).ToList().Distinct();
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
                        var OldPayments = GetShowroomPaymentById(ShowroomID);
                        if (OldPayments != null)
                        {
                            var Sfirstmonth = Convert.ToDateTime("April" + " " + "2022");
                            var StartDate = new DateTime(Sfirstmonth.Year, Sfirstmonth.Month, 1);
                            var TodaysDate = DateTime.Now;
                            var MonthsBetweenDates = MonthsBetween(StartDate, TodaysDate);

                            #region **If Showroom Doesn't Pay Any Payment Yet**

                            var OldPaymentFRecieve = "";
                            var OldPaymentTRecieve = "";
                            var IsNotMonth = "01/01/0001";

                            foreach (var payments in OldPayments)
                            {
                                OldPaymentFRecieve = (payments.RecievedFromDate == null) ? "01/01/0001" : payments.RecievedFromDate.Value.ToString("dd/MM/yyyy");
                                OldPaymentTRecieve = (payments.RecievedToDate == null) ? "01/01/0001" : payments.RecievedToDate.Value.ToString("dd/MM/yyyy");
                            }

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

                            #region **Showroom Payments Management**

                            else
                            {
                                DateTime RecievestartDate;
                                DateTime RecieveendDate;

                                if (OldPayments.Count() == 1)
                                {
                                    RecievestartDate = OldPayments.FirstOrDefault().RecievedFromDate.Value.Date;
                                    RecieveendDate = OldPayments.FirstOrDefault().RecievedToDate.Value.Date;
                                }
                                else
                                {
                                    RecievestartDate = OldPayments.FirstOrDefault().RecievedFromDate.Value.Date;
                                    RecieveendDate = OldPayments.LastOrDefault().RecievedToDate.Value.Date;
                                }

                                var Recievedmonths = MonthsBetween(RecievestartDate, RecieveendDate);

                                /*If Showroom Pays Some of the Month Payment But Not Current or Previous Months*/
                                if (OldPayments.LastOrDefault().RecievableFromDate != null && OldPayments.LastOrDefault().RecievableToDate != null)
                                {
                                    var RecievablestartDate = OldPayments.LastOrDefault().RecievableFromDate.Value.Date;
                                    var RecievableendDate = OldPayments.LastOrDefault().RecievableToDate.Value.Date;
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

                                    var PaymentsLst = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).ToList();
                                    var Payments = PaymentsLst.LastOrDefault();
                                    Payments.RecievableFromDate = firstmonthStart;
                                    Payments.RecievableToDate = lastmonthEnd;
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
                    var OldPendingPayment = GetShowroomPaymentById(ShowroomID).Select(s => new { s.RecievableFromDate, s.RecievableToDate, s.Balance });
                    if (OldPendingPayment != null)
                    {
                        List<DatesD> Datelist = new List<DatesD>();
                        decimal? TBalanceAmnt = 0;

                        if (OldPendingPayment.LastOrDefault().RecievableFromDate != null && OldPendingPayment.LastOrDefault().RecievableToDate != null)
                        {
                            var startDate = OldPendingPayment.LastOrDefault().RecievableFromDate.Value.Date;
                            var endDate = OldPendingPayment.LastOrDefault().RecievableToDate.Value.Date;
                            var months = MonthsBetween(startDate, endDate);
                            foreach (var item in months)
                            {
                                Datelist.Add(new DatesD()
                                {
                                    Month = item.Month,
                                    Year = item.Year,
                                });
                            }
                        }

                        TBalanceAmnt += (OldPendingPayment.LastOrDefault().Balance == null) ? 0 : OldPendingPayment.LastOrDefault().Balance.Value;


                        var payment = _context.tblShowrooms.Where(x => x.ID == ShowroomID).Select(a => new ValidationPayment()
                        {
                            ShowroomName = a.FullName,
                            ShowroomNumber = a.Contact,
                            ShowroomAddress = a.ShopNumber + " " + a.tblAddress.CompleteAddress,
                            Balance = TBalanceAmnt
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
                tblShowroom = a.tblShowroom,
            }).ToList().Distinct();

            if (payment != null)
            {
                return payment;
            }
            else
            {
                return null;
            }
        }
        
        public decimal GetAllShowroomPayments()
        {
            var payment = _context.tblPayments.ToList();
            decimal TotalAmnt = 0;
            if (payment != null)
            {
                foreach (var item in payment)
                {
                    if (item.Recieved != null)
                    {
                        TotalAmnt += item.Recieved.Value;
                    }
                    else
                    {
                        TotalAmnt += 0;
                    }
                }
                return TotalAmnt;
            }
            else
            {
                return TotalAmnt;
            }
        }

        public Tuple<decimal, DateTime> GetPaymentID(int ShowroomID)
        {
            var payment = _context.tblPayments.Where(x => x.ShowroomID == ShowroomID).Select(x => new ValidationPayment() { RecievedFromDate = x.RecievedFromDate, Balance = x.Balance }).ToList();
            decimal Balance = 0;
            foreach (var item in payment)
            {
                if (item.Balance != null)
                {
                    Balance += item.Balance.Value;
                }
            }
            if (payment.FirstOrDefault().RecievedFromDate == null)
            {
                var RecievedFromDate = payment.FirstOrDefault().RecievedFromDate.Value;
                if (payment != null)
                {
                    return Tuple.Create(Balance, RecievedFromDate);
                }
                else
                {
                    return null;
                }
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
                    var OldPayment = GetShowroomPaymentById(ShowroomID).Select(s => new { s.RecievableFromDate, s.RecievableToDate, s.RecievedFromDate, s.RecievedToDate, s.Recieved, s.Recievable, s.Discount, s.Balance });
                    if (OldPayment != null)
                    {
                        List<DatesD> Datelist = new List<DatesD>();
                        List<DatesD> Datelist2 = new List<DatesD>();
                        List<DatesD> Datelist3 = new List<DatesD>();
                        decimal TBalanceAmnt = 0;
                        decimal TDiscountAmnt = 0;
                        decimal TRecievedAmnt = 0;
                        decimal TRecievableAmnt = 0;

                        foreach (var payments in OldPayment)
                        {
                            if (payments.RecievableFromDate != null && payments.RecievableToDate != null)
                            {
                                var startDate = payments.RecievableFromDate.Value.Date;
                                var endDate = payments.RecievableToDate.Value.Date;
                                var months = MonthsBetween(startDate, endDate);
                                foreach (var item in months)
                                {
                                    Datelist.Add(new DatesD()
                                    {
                                        Month = item.Month,
                                        Year = item.Year,
                                        IsPaid = false,
                                        Recievable = payments.Recievable,
                                        Recieved = payments.Recieved,
                                        Discount = payments.Discount,
                                        Balance = 0
                                    });
                                    TBalanceAmnt += (payments.Balance == null) ? 0 : payments.Balance.Value;
                                    TDiscountAmnt += (payments.Discount == null) ? 0 : payments.Discount.Value;
                                    TRecievedAmnt += (payments.Recieved == null) ? 0 : payments.Recieved.Value;
                                    TRecievableAmnt += (payments.Recievable == null) ? 0 : payments.Recievable.Value;
                                }
                            }

                            if (payments.RecievedFromDate != null && payments.RecievedToDate != null)
                            {
                                var startDate = payments.RecievedFromDate.Value.Date;
                                var endDate = payments.RecievedToDate.Value.Date;
                                var months = MonthsBetween(startDate, endDate);
                                foreach (var item in months)
                                {
                                    Datelist2.Add(new DatesD()
                                    {
                                        Month = item.Month,
                                        Year = item.Year,
                                        IsPaid = true,
                                        Recievable = payments.Recievable,
                                        Recieved = payments.Recieved,
                                        Discount = payments.Discount,
                                        Balance = payments.Balance
                                    });
                                    TBalanceAmnt += (payments.Balance == null) ? 0 : payments.Balance.Value;
                                    TDiscountAmnt += (payments.Discount == null) ? 0 : payments.Discount.Value;
                                    TRecievedAmnt += (payments.Recieved == null) ? 0 : payments.Recieved.Value;
                                    TRecievableAmnt += (payments.Recievable == null) ? 0 : payments.Recievable.Value;
                                }
                            }
                        }


                        var payment = _context.tblShowrooms.Where(x => x.ID == ShowroomID).Select(a => new ValidationPayment()
                        {
                            ShowroomName = a.FullName,
                            ShowroomNumber = a.Contact,
                            ShowroomAddress = a.ShopNumber + " " + a.tblAddress.CompleteAddress,
                            tblShowroom = a.tblPayments.FirstOrDefault().tblShowroom,
                            Recievable = TRecievableAmnt,
                            Recieved = TRecievedAmnt,
                            Discount = TDiscountAmnt,
                            Balance = TBalanceAmnt,
                        }).FirstOrDefault();

                        payment.RecievableDate = Datelist;
                        payment.RecievedDates = Datelist2;
                        List<DatesD> result = Datelist.Where(p => !Datelist2.Any(x => x.Month == p.Month)).Distinct().ToList();
                        List<DatesD> result2 = new List<DatesD>();
                        foreach (var item in result)
                        {
                            var reas = result.FindLast(x => x.Month == item.Month);
                            reas.Recievable = 0;
                            reas.Recieved = 0;
                            reas.Discount = 0;
                            result2.Add(reas);
                        }
                        result2 = result2.Distinct().ToList();
                        payment.datesDs = Enumerable.Concat(result2, Datelist2).OrderBy(m => DateTime.Parse(m.Month + " " + m.Year)).ToList();

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
