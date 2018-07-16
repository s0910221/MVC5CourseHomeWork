using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC5Course.Models.ViewModels
{
    public class CustomerCountViewModel
    {
        public int Id { get; set; }
        public string 客戶名稱 { get; set; }
        public string 統一編號 { get; set; }
        public string 電話 { get; set; }
        public string 傳真 { get; set; }
        public string 地址 { get; set; }
        public string Email { get; set; }
        public string 客戶分類 { get; set; }
        public int 聯絡人數量 { get; set; }
        public int 銀行帳戶數量 { get; set; }
    }
}