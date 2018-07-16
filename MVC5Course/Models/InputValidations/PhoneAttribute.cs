using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVC5Course.Models.InputValidations
{
    public class PhoneAttribute : DataTypeAttribute
    {
        public PhoneAttribute() : base(DataType.Text)
        {
            ErrorMessage = "格式應為 xxxx-xxxxxx, ex:0911-111111";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string str = (string)value;

            if (str == String.Empty)
            {
                return true;
            }
            Regex regex = new Regex("09[0-9]{2}-[0-9]{6}");
            return regex.IsMatch(str);
        }


    }
}