using AutoMapper;
using MVC5Course.Models;
using MVC5Course.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC5Course.App_Start
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<客戶資料, CustomerCountViewModel>()
                .ForMember(x => x.聯絡人數量, y => y.MapFrom(s => s.客戶聯絡人.Where(z => !z.是否已刪除).Count()))
                .ForMember(x => x.銀行帳戶數量, y => y.MapFrom(s => s.客戶銀行資訊.Where(z => !z.是否已刪除).Count()));
            });
        }
    }
}