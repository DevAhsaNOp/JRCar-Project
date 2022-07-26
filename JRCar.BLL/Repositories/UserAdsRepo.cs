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
    public class UserAdsRepo
    {
        private UserAds dbObj;

        public UserAdsRepo()
        {
            dbObj = new UserAds();
        }

        public IEnumerable<tblUserAdd> GetAllUserAds()
        {
            return dbObj.GetAllUserAds();
        }

        public ValidationUserAds GetUserAdsDetail(int AdsId)
        {
            var user = dbObj.GetUserAdsDetail(AdsId);
            if (user != null)
            {
                return user;
            }
            else
                return null;
        }

        public bool InsertUserAds(ValidationUserAds model)
        {
            try
            {
                if (model != null)
                {
                    tblUserAdd userAds = new tblUserAdd()
                    {
                        UserID = model.UserID,
                        Model = model.Model,
                        Year = model.Year,
                        Condition = model.Condition,
                        Title = model.Title,
                        Description = model.Description,
                        Price = model.Price,
                        Address = model.Address,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude
                    };
                    var user = dbObj.InsertUserAds(userAds);
                    if (user > 0)
                    {
                        ValidateUserAdsImage userAdImagesObj = new ValidateUserAdsImage()
                        {
                            Image = model.CarImage,
                            tblUserAddID = user
                        };
                        var userAdImages = InsertUserAdsImages(userAdImagesObj);
                        if (userAdImages)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateUserAds(tblUserAdd model)
        {
            try
            {
                if (model != null)
                {
                    var user = dbObj.UpdateUserAds(model);
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool InsertUserAdsImages(ValidateUserAdsImage model)
        {
            try
            {
                if (model != null)
                {
                    tblUserAddImage userAddImage = new tblUserAddImage()
                    {
                        Image = model.Image,
                        tblUserAddID = model.tblUserAddID
                    };
                    var user = dbObj.InsertUserAdsImages(userAddImage);
                    return user;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateUserAdsImages(tblUserAddImage model)
        {
            try
            {
                if (model != null)
                {
                    var user = dbObj.UpdateUserAdsImages(model);
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
