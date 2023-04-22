using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace MiniCafeBUS.Utils
{
    public static class Utils
    {
        public static bool checkName(String name)
        {
            string namePattern = @"^\p{L}+(\s\p{L}+)*$";
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Không được để trống tên.");
            }
            if (!Regex.IsMatch(name,namePattern , RegexOptions.IgnoreCase))
            {
                throw new ArgumentException("Tên không hợp lệ.");
            }
            return true;
        }
        public static bool checkNumber(String number)
        {
            string pattern = @"^0\d{9}$";
            if (string.IsNullOrEmpty(number))
            {
                throw new ArgumentException("Không được để trống số điện thoại.");
            }
            if (!Regex.IsMatch(number, pattern))
            {
                throw new ArgumentException("Số điện thoại không hợp lệ.");
            }
            return true;
        }

        public static bool checkBirthday(DateTime birthday)
        {
            if (birthday == null)
            {
                throw new ArgumentNullException("Vui lòng chọn ngày sinh");
            }
            if (birthday >= DateTime.Today)
            {
                throw new ArgumentException("Vui lòng chọn ngày sinh trước ngày hiện tại");
            }
            DateTime now = DateTime.Today;
            DateTime eighteenYearsAgo = now.AddYears(-18);
            DateTime employeeBirthday = birthday;
            if (eighteenYearsAgo.Date < employeeBirthday.Date)
            {
                throw new ArgumentException("Chưa đủ 18 tuổi");
            }
            return true;
        }

        public static bool checkUsername(string username)
        {
           
            string pattern = @"^(?=.{5,20}$)(?![_])(?!.*[_]{2})\w+(?<![_])$";
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Tên đăng nhập không được để trống.");
            }
            if (!Regex.IsMatch(username,pattern))
            {
                throw new ArgumentException("username nên bao gồm các ký tự a-z, A-Z, 0-9 và dấu gạch dưới");
            }
            return true;
        }
    }
}
