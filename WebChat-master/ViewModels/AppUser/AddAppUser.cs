using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.ViewModels.AppUser
{
    public class AddAppUser
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(16, ErrorMessage = "Tên đăng nhập phải có từ {2}-{1} kí tự", MinimumLength = 4)]
        [DisplayName("Tên đăng nhập")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Chưa xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        [DisplayName("Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Tên đầy đủ không được để trống")]
        [StringLength(200)]
        [DisplayName("Tên đầy đủ")]
        public string FullName { get; set; }
    }
}