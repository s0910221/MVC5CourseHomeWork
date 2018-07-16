using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5Course.Models
{
    public class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        public bool HasSameEmail(int customerID, string email)
        {
            if (All().Where(x => x.客戶Id == customerID && x.Email == email).Count() > 0) return true;
            return false;
        }

    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {
        bool HasSameEmail(int customerID, string email);
    }
}