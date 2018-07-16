using System;
using System.Linq;
using System.Collections.Generic;

namespace MVC5Course.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public IQueryable<客戶資料> All(bool isContainDeletedData = false)
        {
            if (isContainDeletedData)
            {
                return base.All();
            }
            return base.All().Where(c => c.是否已刪除 == false);
        }

        public 客戶資料 Find(int id)
        {
            return All().FirstOrDefault(c => c.Id == id);
        }

        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}