using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Filters
{
    public class GetActionRunTimeAttribute : ActionFilterAttribute
    {
        private DateTime StartActionTime { get; set; }

        private DateTime EndActionTime { get; set; }

        private TimeSpan ActionSpan { get; set; }

        private DateTime StartResultTime { get; set; }

        private DateTime EndResultTime { get; set; }

        private TimeSpan ResultSpan { get; set; }

        public string Unit { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            StartActionTime = DateTime.Now;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            EndActionTime = DateTime.Now;
            ActionSpan = EndActionTime - StartActionTime;
            filterContext.Controller.ViewBag.ActionRunTime = GetTimeSpanCount(ActionSpan);
            Console.WriteLine(GetTimeSpanCount(ActionSpan));
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            StartResultTime = DateTime.Now;
            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            EndResultTime = DateTime.Now;
            ResultSpan = EndResultTime - StartResultTime;
            filterContext.Controller.ViewBag.ResultRunTime = GetTimeSpanCount(ResultSpan);
            Console.WriteLine(GetTimeSpanCount(ResultSpan));
            base.OnResultExecuted(filterContext);
        }

        private string GetTimeSpanCount(TimeSpan t)
        {
            long Result = 0;
            switch (Unit)
            {
                case "sec"://傳回間隔的秒數
                    Result = (long)t.TotalSeconds;
                    break;
                case "min": //傳回間隔的分鐘數
                    Result = (long)t.TotalMinutes;
                    break;
                case "hour": //傳回間隔的分鐘數
                    Result = (long)t.TotalHours;
                    break;
                case "day"://傳回間隔的天數
                    Result = (long)t.TotalDays;
                    break;
                default://傳回間隔的毫秒數
                    Result = (long)t.TotalMilliseconds;
                    Unit = "millisec";
                    break;
            }
            return Result + Unit;
        }
    }
}