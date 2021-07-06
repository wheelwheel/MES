using MES.Models;
using MES.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.IO;

namespace MES.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult List(int page = 1, int pageSize = 5)
        {
            using (MESEntities db = new MESEntities())
            {
                var datas = db.users.AsQueryable().Where(m => m.role_no == "U").OrderBy(m => m.m_account);
                return View(datas.ToPagedList(page, pageSize));
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Infor(LoginViewModel model)
        {
            using (MESEntities db = new MESEntities())
            {
                var datas = db.users
                    .Where(m => m.m_account == UserAccount.Account).ToList();
                return View(datas);
            }
        }

        // GET: User
        public ActionResult Login()
        {
            UserAccount.Logout();
            LoginViewModel model = new LoginViewModel()
            {
                Account = "",
                Password = ""
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (MESEntities db = new MESEntities())
            {
                using (Cryptographys cryp = new Cryptographys())
                {
                    string str_password = cryp.SHA256Encode(model.Password);
                    var data = db.users
                        .Where(m => m.m_account == model.Account)
                        .Where(m => m.pass_user == str_password)
                        .FirstOrDefault();
                    if (data == null)
                    {
                        ModelState.AddModelError("Account", "帳號或密碼不合");
                        return View(model);
                    }
                    UserAccount.Account = model.Account;
                    UserAccount.Login();
                    return RedirectToAction("Infor", "User");
                }
            }
        }


        //新增
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create()
        {
            users model = new users()
            {
                m_account = "",
                m_name = "",
                birthday = DateTime.Today
            };
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create(users model)
        {
            using (MESEntities db = new MESEntities())
            {
                if (!ModelState.IsValid) return View(model);
                bool bln_error = false;
                var check = db.users.Where(m => m.m_account == model.m_account).FirstOrDefault();
                if (check != null) { ModelState.AddModelError("", "帳號重複註冊!"); bln_error = true; }
                var check1 = db.users.Where(m => m.email_user == model.email_user).FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("", "信箱重複註冊!"); bln_error = true; }
                var check2 = db.users.Where(m => m.id == model.id).FirstOrDefault();
                if (check2 != null) { ModelState.AddModelError("", "身分證重複註冊!"); bln_error = true; }
                if (bln_error) return View(model);
                using (Cryptographys cryp = new Cryptographys())
                {
                    users data = new users()
                    {
                        m_account = model.m_account,
                        m_name = model.m_name,
                        role_no = "U",
                        pass_user = cryp.SHA256Encode(model.m_account),
                        email_user = model.email_user,
                        birthday = model.birthday,
                        address = model.address,
                        id = model.id,
                        phone = model.phone
                    };
                    db.users.Add(data);
                    db.SaveChanges();

                    return RedirectToAction("List");

                }
            }
        }

        //修改
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(int i)
        {
            using (MESEntities db = new MESEntities())
            {
                var model = db.users.Where(m => m.rowid == i).FirstOrDefault();
                return View(model);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(users model)
        {
            if (!ModelState.IsValid) return View(model);

            using (MESEntities db = new MESEntities())
            {
                var data = db.users.Where(m => m.rowid == model.rowid).FirstOrDefault();
                data.m_account = model.m_account;
                data.m_name = model.m_name;
                data.email_user = model.email_user;
                data.birthday = model.birthday;
                data.address = model.address;
                data.id = model.id;
                data.phone = model.phone;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult UserEdit(int i)
        {
            using (MESEntities db = new MESEntities())
            {
                var model = db.users.Where(m => m.rowid == i).FirstOrDefault();
                return View(model);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult UserEdit(users model)
        {
            if (!ModelState.IsValid) return View(model);

            using (MESEntities db = new MESEntities())
            {
                var data = db.users.Where(m => m.rowid == model.rowid).FirstOrDefault();
                data.m_account = model.m_account;
                data.m_name = model.m_name;
                data.email_user = model.email_user;
                data.birthday = model.birthday;
                data.address = model.address;
                data.id = model.id;
                data.phone = model.phone;

                db.SaveChanges();

                return RedirectToAction("Infor");
            }
        }


        //刪除
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Delete(int i)
        {
            using (MESEntities db = new MESEntities())
            {
                var model = db.users.Where(m => m.rowid == i).FirstOrDefault();
                if (model != null)
                {
                    db.users.Remove(model);
                    db.SaveChanges();
                }
                return RedirectToAction("List", "User");
            }
        }



        //重設密碼
        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult ResetPassword()
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();

            model.UserNo = UserAccount.Account;
            model.CurrentPassword = "";
            model.NewPassword = "";
            model.ConfirmPassword = "";

            return View(model);
        }


        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            string str_password = "";
            using (MESEntities db = new MESEntities())
            {
                using (Cryptographys cryp = new Cryptographys())
                { str_password = cryp.SHA256Encode(model.CurrentPassword); }
                bool bln_error = false;

                var check = db.users
                    .Where(m => m.m_account == model.UserNo)
                    .Where(m => m.pass_user == str_password)
                    .FirstOrDefault();
                if (check == null) { ModelState.AddModelError("", "目前密碼輸入錯誤!!"); bln_error = true; }
                if (bln_error) return View(model);

                str_password = model.NewPassword;
                var user = db.users.Where(m => m.m_account == model.UserNo).FirstOrDefault();
                if (user != null)
                {
                    //密碼加密
                    using (Cryptographys cryp = new Cryptographys())
                    { str_password = cryp.SHA256Encode(str_password); }

                    user.pass_user = str_password;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    db.Configuration.ValidateOnSaveEnabled = true;
                }
                TempData["HeaderText"] = "密碼變更完成";
                TempData["MessageText"] = "密碼已變更，下次登入請使用新密碼";
                return RedirectToAction("MessageText");
            }
        }

        public ActionResult MessageText()
        {
            ViewBag.HeaderText = TempData["HeaderText"].ToString();
            ViewBag.MessageText = TempData["MessageText"].ToString();
            return View();
        }



        public ActionResult UserUploadimage(int id)
        {
            using (MESEntities db = new MESEntities())
            {
                var model = db.users.Where(m => m.rowid == id).FirstOrDefault();
                UserAccount.UploadImageMode = true;
                UserAccount.TempNo = model.m_account;
                return View(model);

            }
        }

        [HttpPost]
        public ActionResult UserUploadimage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = UserAccount.TempNo + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/user"), fileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    file.SaveAs(path);
                }
            }
            UserAccount.UploadImageMode = false;
            return RedirectToAction("List");
        }















        public ActionResult UploadImage()
        {
            UserAccount.UploadImageMode = true;
            return RedirectToAction("Infor");
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = UserAccount.Account + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/user"), fileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    file.SaveAs(path);
                }
            }
            UserAccount.UploadImageMode = false;
            return RedirectToAction("Infor");
        }

        public ActionResult UploadCancel()
        {
            UserAccount.UploadImageMode = false;
            return RedirectToAction("Infor");
        }
    }
}