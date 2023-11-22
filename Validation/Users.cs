using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation
{
    internal class Users
    {
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Введите минимум 5 символов!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [EmailAddress(ErrorMessage = "Введите почту корректно!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Введите минимум 5 символов!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string RepPassword { get; set; }
    }
}
