using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Hangfire;
using HangfireScheudler.Hangfire;

namespace HangfireScheudler.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult HangDash()
        {
            return Redirect("/hangfire");
        }

        public ActionResult FireForget()
        {
            var task = new HangFireTasks();
            var jobId = BackgroundJob.Enqueue(() => task.RandomWebClient());
            BackgroundJob.ContinueJobWith(
                jobId,
                () => Console.WriteLine("Continuation!"));

            return View("Index");
        }

        public ActionResult Delayed()
        {
            var task = new HangFireTasks();
            BackgroundJob.Schedule(() => task.RandomWebClient(), TimeSpan.FromSeconds(10));

            return View("Index");
        }

        public ActionResult Loop()
        {
            var task = new HangFireTasks();
            RecurringJob.AddOrUpdate(() => task.RandomWebClient(), Cron.Minutely());

            return View("Index");
        }


    }
}