using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebChat.Entities;
using WebChat.ViewModels.AppUser;

namespace WebChat.Controllers
{
    public class AppUserController : Controller
    {
        readonly WebChatDbContext db;

        public AppUserController(WebChatDbContext db)
        {
            this.db = db;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(AddAppUser model)
        {
            AppUser user = new AppUser();
            if (ModelState.IsValid)
            {
                try
                {
                    //Mã hóa mật khẩu
                    HMACSHA512 hmac = new();
                    var pwByte = Encoding.UTF8.GetBytes(model.Password);
                    user.PasswordHash = hmac.ComputeHash(pwByte);
                    user.PasswordSalt = hmac.Key;

                    //Sao chép dữ liệu
                    user.Username = model.Username.Replace(" ", "").ToLower();
                    user.FullName = model.FullName;
                    user.CreateDate = DateTime.Now;

                    //Lưu dữ liệu
                    await db.AddAsync(user);
                    await db.SaveChangesAsync();

                    TempData["Success"] = "Đăng ký thành công!";
                }
                catch
                {
                    TempData["Error"] = "Đăng ký thất bại!";
                }
            }
            else
            {
                TempData["IsNotValid"] = "Dữ liệu không hợp lệ";
            }
            return RedirectToAction(nameof(SignUp));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login loginData)
        {
            loginData.Username = loginData.Username.Replace(" ", "").ToLower();
            var user = db.AppUsers
                        .AsNoTracking()
                        .SingleOrDefault(u => u.Username == loginData.Username);
            if (user != null)
            {
                HMACSHA512 hmac = new HMACSHA512();
                hmac.Key = user.PasswordSalt;
                var pwByte = Encoding.UTF8.GetBytes(loginData.Password);
                var pwHash = hmac.ComputeHash(pwByte);
                if (pwHash.SequenceEqual(user.PasswordHash))
                {
                    //Lưu thông tin cho phiên đăng nhập này
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.GivenName, user.FullName)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var principal = new ClaimsPrincipal(claimsIdentity);
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddHours(6),
                        IsPersistent = loginData.RememberMe
                    };
                    await HttpContext.SignInAsync("Cookies", principal, authProperties);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["msg"] = "Tên đăng nhập hoặc mật khẩu không chính xác";
                }
            }
            else
            {
                TempData["msg"] = "Tên đăng nhập hoặc mật khẩu không chính xác";
            }
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
