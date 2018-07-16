using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5Course.Models
{
    public class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        public IQueryable<客戶聯絡人> All(bool isContainDeletedData = false)
        {
            if (isContainDeletedData)
            {
                return base.All();
            }
            return base.All().Where(c => c.是否已刪除 == false);
        }

        public 客戶聯絡人 Find(int id)
        {
            return All().FirstOrDefault(c => c.Id == id);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }

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