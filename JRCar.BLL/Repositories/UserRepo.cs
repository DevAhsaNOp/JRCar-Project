using JRCar.BOL;
using JRCar.BOL.Validation_Classes;
using JRCar.DAL.DBLayer;
using JRCar.DAL.UserDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JRCar.BLL.Repositories
{
    public class UserRepo
    {
        private UserDb dbObj;

        public UserRepo()
        {
            dbObj = new UserDb();
        }

        public bool InActiveModel(int UserID, string Role)
        {
            if (UserID > 0 && Role != null)
            {
                var AdminData = dbObj.GetAllAdmin().Where(x => x.ID == UserID && x.tblRole.Role.ToLower().Contains(Role.ToLower())).FirstOrDefault();
                var ShowroomData = dbObj.GetAllShowRoom().Where(x => x.ID == UserID && x.tblRole.Role.ToLower().Contains(Role.ToLower())).FirstOrDefault();
                var UnionData = dbObj.GetAllUnion().Where(x => x.ID == UserID && x.tblRole.Role.ToLower().Contains(Role.ToLower())).FirstOrDefault();
                var UserData = dbObj.GetAllUsers().Where(x => x.ID == UserID && x.tblRole.Role.ToLower().Contains(Role.ToLower())).FirstOrDefault();
                if (AdminData != null)
                {
                    dbObj.InActiveAdmin(AdminData);
                    return true;
                }
                else if (ShowroomData != null)
                {
                    dbObj.InActiveShowroom(ShowroomData);
                    return true;
                }
                else if (UnionData != null)
                {
                    dbObj.InActiveUnion(UnionData);
                    return true;
                }
                else if (UserData != null)
                {
                    dbObj.InActiveUser(UserData);
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

        public bool ActiveModel(int UserID, string Role)
        {
            if (UserID > 0 && Role != null)
            {
                var UnionData = dbObj.GetAllUnion().Where(x => x.ID == UserID && x.tblRole.Role.ToLower().Contains(Role.ToLower())).FirstOrDefault();
                if (UnionData != null)
                {
                    dbObj.ActiveUnion(UnionData);
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

        public bool IsEmailExist(string Email)
        {
            if (Email != null)
            {
                var reas = dbObj.IsEmailExist(Email);
                if (reas)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public bool IsPhoneNumberExist(string PhoneNumber)
        {
            if (PhoneNumber != null)
            {
                var reas = dbObj.IsPhoneNumberExist(PhoneNumber);
                if (reas)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public bool IsUpdateEmailExist(string Email, string CurrentEmail)
        {
            if (Email != null && Email != CurrentEmail)
            {
                var reas = dbObj.IsEmailExist(Email);
                if (reas)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public bool IsUpdatePhoneNumberExist(string PhoneNumber, string CurrentNumber)
        {
            if (PhoneNumber != null && PhoneNumber != CurrentNumber)
            {
                var reas = dbObj.IsPhoneNumberExist(PhoneNumber);
                if (reas)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<tblShowroom> GetAllShowRoom()
        {
            return dbObj.GetAllShowRoom();
        }

        public bool IsShowroom(int UserID)
        {
            if (UserID > 0)
            {
                var reas = GetShowRoomByID(UserID);
                if (reas != null)
                {
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        public IEnumerable<tblUser> GetAllUsers()
        {
            return dbObj.GetAllUsers();
        }

        public IEnumerable<tblUnion> GetAllUnion()
        {
            return dbObj.GetAllUnion();
        }

        public bool ForgotPassword(string emailtext)
        {
            try
            {
                var isTrue = dbObj.ForgotPassword(emailtext);
                if (isTrue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserViewDetail GetUserRole(string email)
        {
            if (email != null)
            {
                var reas = dbObj.GetUserDetail(email);
                return reas;
            }
            else return null;
        }

        public bool CheckOTP(string emailtext, string OTP)
        {
            var IsTrue = dbObj.CheckOTP(emailtext, OTP);
            if (IsTrue)
                return true;
            else
                return false;
        }

        public tblUser GetModelByID(int modelId)
        {
            try
            {
                var reas = dbObj.GetUserByID(modelId);
                if (reas != null)
                {
                    reas.Password = EncDec.Decrypt(reas.Password);
                    return reas;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tblUser GetUserByID(int modelId)
        {
            try
            {
                var reas = dbObj.GetUserByID(modelId);
                if (reas != null)
                {
                    return reas;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tblShowroom GetShowroomByID(int modelId)
        {
            try
            {
                var reas = dbObj.GetShowRoomByID(modelId);
                if (reas != null)
                {
                    return reas;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DAL.UserDefine.UserViewDetail CheckLoginDetails(string emailtext, string password)
        {
            try
            {
                var reas = dbObj.GetUserDetail(emailtext);
                if (reas != null)
                {
                    if (EncDec.Decrypt(reas.Password).Equals(password))
                    {
                        var entity = dbObj.GetUserDetail(emailtext);
                        return entity;
                    }
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertUser(ValidateUser model)
        {
            if (model != null)
            {
                tblUser obj = new tblUser()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Number = model.Number,
                    Address = model.Address,
                    Password = EncDec.Encrypt(model.Password),
                    Image = model.Image
                };
                dbObj.InsertUser(obj);
            }
        }

        public void InsertAdmin(ValidateUser model)
        {
            if (model != null)
            {
                tblAdmin obj = new tblAdmin()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Number = model.Number,
                    Address = model.Address,
                    Password = EncDec.Encrypt(model.Password),
                    Image = model.Image
                };
                dbObj.InsertAdmin(obj);
            }
        }

        public void InsertUnion(ValidateUser model)
        {
            if (model != null)
            {
                tblUnion obj = new tblUnion()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Number = model.Number,
                    Address = model.Address,
                    Password = EncDec.Encrypt(model.Password),
                    Image = model.Image,
                    CreatedBy = model.CreatedBy,
                    tblCRoleID = model.tblRoleID,
                };
                dbObj.InsertUnion(obj);
            }
        }

        public List<SelectListItem> GetAllUnionRole()
        {
            var AllRole = GetRolePermission();
            var roles = new List<SelectListItem>
            {
                new SelectListItem() { Text = "---Select Union Role---", Value = "0", Disabled = true, Selected = true }
            };
            foreach (var item in AllRole)
            {
                roles.Add(new SelectListItem() { Text = item.tblRole.Role, Value = item.tblRole.ID.ToString() });
            }
            return roles;
        }
        
        public List<SelectListItem> GetAllUnionRoleE()
        {
            var AllRole = GetRolePermission();
            var roles = new List<SelectListItem>
            {
                new SelectListItem() { Text = "---Select Union Role---", Value = "0", Disabled = true, Selected = true }
            };
            foreach (var item in AllRole)
            {
                roles.Add(new SelectListItem() { Text = item.tblRole.Role, Value = item.tblRole.ID.ToString() });
            }
            return roles;
        }


        public bool InsertRoleWithPermission(ValidateRolePermission model)
        {
            if (model != null)
            {
                tblRole role = new tblRole()
                {
                    Role = model.Role,
                    CreatedBy = model.CreatedBy
                };
                var IsRoleInserted = dbObj.InsertRole(role);
                if (IsRoleInserted > 0)
                {
                    tblRolePermission obj = new tblRolePermission()
                    {
                        AddShowroom = model.AddShowroom,
                        AddUnionMember = model.AddUnionMember,
                        AddUser = model.AddUser,
                        DeleteShowroom = model.DeleteShowroom,
                        DeleteUnionMember = model.DeleteUnionMember,
                        DeleteUser = model.DeleteUser,
                        EditProfile = model.EditProfile,
                        EditShowroom = model.EditShowroom,
                        EditUnionMember = model.EditUnionMember,
                        EditUser = model.EditUser,
                        MakeAnnoucment = model.MakeAnnoucment,
                        MakePayments = model.MakePayments,
                        ManagShowroomAds = model.ManagShowroomAds,
                        ManagUserAds = model.ManagUserAds,
                        RoleID = IsRoleInserted,
                        ShowAnnoucment = model.ShowAnnoucment,
                        ShowPayments = model.ShowPayments,
                        ShowShowroom = model.ShowShowroom,
                        ShowUnionMember = model.ShowUnionMember,
                        ShowUsers = model.ShowUsers,
                        AddUnionRole = model.AddUnionRole,
                        DeleteUnionRole = model.DeleteUnionRole,
                        EditUnionRole = model.EditUnionRole,
                        ShowUnionRole = model.ShowUnionRole,
                    };
                    dbObj.InsertRolePermission(obj);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public bool EditRoleWithPermission(ValidateRolePermission model)
        {
            if (model != null)
            {
                tblRole role = new tblRole()
                {
                    ID = model.RoleID,
                    Role = model.Role,
                    UpdatedBy = model.UpdatedBy,
                    CreatedBy = dbObj.GetRoleByID(model.RoleID).CreatedBy,
                    CreatedOn = dbObj.GetRoleByID(model.RoleID).CreatedOn,
                };
                var IsRoleInserted = dbObj.EditRole(role);
                if (IsRoleInserted > 0)
                {
                    tblRolePermission obj = new tblRolePermission()
                    {
                        ID = model.ID,
                        AddShowroom = model.AddShowroom,
                        AddUnionMember = model.AddUnionMember,
                        AddUser = model.AddUser,
                        DeleteShowroom = model.DeleteShowroom,
                        DeleteUnionMember = model.DeleteUnionMember,
                        DeleteUser = model.DeleteUser,
                        EditProfile = model.EditProfile,
                        EditShowroom = model.EditShowroom,
                        EditUnionMember = model.EditUnionMember,
                        EditUser = model.EditUser,
                        MakeAnnoucment = model.MakeAnnoucment,
                        MakePayments = model.MakePayments,
                        ManagShowroomAds = model.ManagShowroomAds,
                        ManagUserAds = model.ManagUserAds,
                        RoleID = IsRoleInserted,
                        ShowAnnoucment = model.ShowAnnoucment,
                        ShowPayments = model.ShowPayments,
                        ShowShowroom = model.ShowShowroom,
                        ShowUnionMember = model.ShowUnionMember,
                        ShowUsers = model.ShowUsers,
                        DeleteUnionRole = model.DeleteUnionRole,
                        EditUnionRole = model.EditUnionRole,
                        ShowUnionRole = model.ShowUnionRole,
                        AddUnionRole = model.AddUnionRole,
                    };
                    dbObj.EditRolePermission(obj);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public ValidateRolePermission GetRolePermissionByID(int ID)
        {
            if (ID > 0)
            {
                var model = dbObj.GetRolePermissionByID(ID);
                if (model != null)
                {
                    ValidateRolePermission obj = new ValidateRolePermission()
                    {
                        AddShowroom = model.AddShowroom.Value,
                        AddUnionMember = model.AddUnionMember.Value,
                        AddUser = model.AddUser.Value,
                        DeleteShowroom = model.DeleteShowroom.Value,
                        DeleteUnionMember = model.DeleteUnionMember.Value,
                        DeleteUser = model.DeleteUser.Value,
                        EditProfile = model.EditProfile.Value,
                        EditShowroom = model.EditShowroom.Value,
                        EditUnionMember = model.EditUnionMember.Value,
                        EditUser = model.EditUser.Value,
                        MakeAnnoucment = model.MakeAnnoucment.Value,
                        MakePayments = model.MakePayments.Value,
                        ManagShowroomAds = model.ManagShowroomAds.Value,
                        ManagUserAds = model.ManagUserAds.Value,
                        Role = model.tblRole.Role,
                        ID = model.ID,
                        RoleID = model.RoleID,
                        ShowAnnoucment = model.ShowAnnoucment.Value,
                        ShowPayments = model.ShowPayments.Value,
                        ShowShowroom = model.ShowShowroom.Value,
                        ShowUnionMember = model.ShowUnionMember.Value,
                        ShowUsers = model.ShowUsers.Value,
                        AddUnionRole = model.AddUnionRole.Value,
                        ShowUnionRole = model.ShowUnionRole.Value,
                        EditUnionRole = model.EditUnionRole.Value,
                        DeleteUnionRole = model.DeleteUnionRole.Value,                       
                    };
                    return obj;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public bool InactiveRole(int RoleID)
        {
            if (RoleID > 0)
            {
                var reas = dbObj.InactiveRole(RoleID);
                if (reas > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        
        public bool ActiveRole(int RoleID)
        {
            if (RoleID > 0)
            {
                var reas = dbObj.ActiveRole(RoleID);
                if (reas > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public IEnumerable<tblRolePermission> GetRolePermission()
        {
            var model = dbObj.GetAllRolePermission();
            if (model != null)
            {
                return model;
            }
            else
                return null;
        }

        public void InsertShowroom(ValidateShowroom model)
        {
            if (model != null)
            {
                tblShowroom obj = new tblShowroom()
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = EncDec.Encrypt(model.Password),
                    CNIC = model.CNIC,
                    Contact = model.Number,
                    ShopNumber = model.ShopNumber,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Image = model.Image,
                    RoleId = 3,
                    AddressId = model.AddressId,
                    UnionId = model.UnionId,
                    CreatedBy = model.CreatedBy,
                };
                dbObj.InsertShowroom(obj);
            }
        }

        public bool UpdateUser(ValidateUser model, string Role)
        {
            if (model != null)
            {
                var admin = dbObj.GetAdminByID(model.ID);
                var ShowroomData = dbObj.GetShowRoomByID(model.ID);
                var UnionData = dbObj.GetUnionByID(model.ID);
                var UserData = dbObj.GetUserByID(model.ID);
                var reas1 = dbObj.GetUserDetail(model.Email);
                if (admin != null && admin.tblRole.Role.ToLower().Contains(Role.ToLower()))
                {
                    admin.Name = model.Name;
                    admin.Email = model.Email;
                    admin.Number = model.Number;
                    admin.Address = model.Address;
                    admin.Password = EncDec.Encrypt(model.Password);
                    admin.Image = model.Image;
                    admin.UpdatedBy = model.UpdatedBy;
                    dbObj.UpdateAdmin(admin);
                    return true;
                }
                else if (UserData != null && UserData.tblRole.Role.ToLower().Contains(Role.ToLower()))
                {
                    UserData.Name = model.Name;
                    UserData.Email = model.Email;
                    UserData.Number = model.Number;
                    UserData.Address = model.Address;
                    UserData.Password = EncDec.Encrypt(model.Password);
                    UserData.Image = model.Image;
                    UserData.UpdatedBy = model.UpdatedBy;
                    dbObj.UpdateUser(UserData);
                    return true;
                }
                else if (UnionData != null && UnionData.tblRole.Role.ToLower().Contains(Role.ToLower()))
                {
                    UnionData.Name = model.Name;
                    UnionData.Email = model.Email;
                    UnionData.Number = model.Number;
                    UnionData.Address = model.Address;
                    UnionData.Password = EncDec.Encrypt(model.Password);
                    UnionData.Image = model.Image;
                    UnionData.UpdatedBy = model.UpdatedBy;
                    dbObj.UpdateUnion(UnionData);
                    return true;
                }
                else if (ShowroomData != null && ShowroomData.tblRole.Role.ToLower().Contains(Role.ToLower()))
                {
                    ShowroomData.FullName = model.Name;
                    ShowroomData.Email = model.Email;
                    ShowroomData.Contact = model.Number;
                    ShowroomData.ShopNumber = model.Address;
                    ShowroomData.Password = EncDec.Encrypt(model.Password);
                    ShowroomData.Image = model.Image;
                    ShowroomData.CNIC = model.CNIC;
                    ShowroomData.UpdatedBy = model.UpdatedBy;
                    ShowroomData.Description = model.ShowroomDescription;
                    dbObj.UpdateShowroom(ShowroomData);
                    return true;
                }
                else if (reas1 != null)
                {
                    if (reas1.Role == "Admin")
                    {
                        var adminData = dbObj.GetAdminByID(reas1.ID);
                        adminData.Password = EncDec.Encrypt(model.Password);
                        dbObj.UpdateAdmin(adminData);
                    }
                    else if (reas1.Role == "User")
                    {
                        var userData = dbObj.GetUserByID(reas1.ID);
                        userData.Password = EncDec.Encrypt(model.Password);
                        dbObj.UpdateUser(userData);
                    }
                    else if (reas1.Role == "Showroom")
                    {
                        var showroomData = dbObj.GetShowRoomByID(reas1.ID);
                        showroomData.Password = EncDec.Encrypt(model.Password);
                        dbObj.UpdateShowroom(showroomData);
                    }
                    else
                    {
                        var unionData = dbObj.GetUnionByID(reas1.ID);
                        unionData.Password = EncDec.Encrypt(model.Password);
                        dbObj.UpdateUnion(unionData);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public tblShowroom GetShowRoomByID(int modelId)
        {
            return dbObj.GetShowRoomByID(modelId);
        }

        public ValidateUser GetUserDetailById(int Id, string Role)
        {
            try
            {
                var reas = dbObj.GetUserDetailById(Id, Role);
                if (reas != null)
                {
                    reas.Password = EncDec.Decrypt(reas.Password);
                    return reas;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ValidationUserAds> UsersInfoList() 
        {
            var reas = dbObj.UsersInfoList();
            return reas;
        }
        
        public IEnumerable<ValidateShowroomAds> ShowroomInfoList() 
        {
            var reas = dbObj.ShowroomInfoList();
            return reas;
        }
    }
}
