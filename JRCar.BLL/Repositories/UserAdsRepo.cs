﻿using JRCar.BOL;
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
        private AddressAutofillRepo AddressRepoObj;

        public UserAdsRepo()
        {
            dbObj = new UserAds();
            AddressRepoObj = new AddressAutofillRepo();
        }

        public IEnumerable<tblUserAdd> GetAllUserActiveAds()
        {
            return dbObj.GetAllUserActiveAds();
        }
        
        public IEnumerable<tblUserAdd> GetAllUserInActiveAds()
        {
            return dbObj.GetAllUserInActiveAds();
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
                    tblAddress addrs = new tblAddress()
                    {
                        State = model.State,
                        City = model.City,
                        Area = model.Area,
                        CompleteAddress = (model.CompleteAddress == null) ? "" : model.CompleteAddress,
                    };
                    var addrsID = AddressRepoObj.InsertAddress(addrs);
                    if (addrsID > 0)
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
                            AddressId = addrsID,
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
                        return false;
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
                    if (user == true)
                        return user;
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
                    if (user == true)
                        return user;
                    else
                        return false;
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
                    if (user == true)
                        return user;
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

        public bool InActiveUserAds(int AdID)
        {
            try
            {
                if (AdID > 0)
                {
                    var user = dbObj.InActiveUserAds(AdID);
                    if (user == true)
                        return user;
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

        public bool ReActiveUserAds(int AdID)
        {
            try
            {
                if (AdID > 0)
                {
                    var user = dbObj.ReActiveUserAds(AdID);
                    if (user == true)
                        return user;
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
    }
}