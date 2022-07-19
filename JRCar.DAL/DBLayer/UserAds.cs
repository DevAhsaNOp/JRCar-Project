using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRCar.DAL.DBLayer
{
    public class UserAds
    {
        private jrcarEntities _context;

        public UserAds()
        {
            _context = new jrcarEntities();
        }

        public IEnumerable<tblUserAdd> GetAllUserAds()
        {
            return _context.tblUserAdds.ToList();
        }

        public ValidationUserAds GetUserAdsDetail(int AdsId)
        {
            var user = _context.tblUserAdds.Where(x => x.ID == AdsId).Select(s => new ValidationUserAds()
            {
                Model = s.Model,
                Year = s.Year,
                Condition = s.Condition,
                Title = s.Title,
                Description = ((s.Description == null) ? "" : s.Description),
                Price = s.Price,
                Address = ((s.Address == null) ? "" : s.Address),
                Latitude = ((s.Latitude == null) ? "" : s.Latitude),
                Longitude = ((s.Longitude == null) ? "" : s.Longitude),
                CreatedOn = s.CreatedOn,
                ExpiryDate = s.ExpiryDate,
                Image = s.tblUser.Image,
                UserName = s.tblUser.Name,
                Email = s.tblUser.Email,
                UserRole = s.tblUser.tblRole.Role,
                Number = s.tblUser.Number
            }).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
                return null;
        }

        public void InsertUserAds(tblUserAdd model)
        {
            try
            {
                model.CreatedOn = DateTime.Now;
                model.ExpiryDate = DateTime.Now.AddMonths(2);
                _context.tblUserAdds.Add(model);
                Save();
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

        public void UpdateUserAds(tblUserAdd model)
        {
            try
            {
                model.CreatedOn = GetUserAdsDetail(model.ID).CreatedOn;
                model.ExpiryDate = GetUserAdsDetail(model.ID).ExpiryDate;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
