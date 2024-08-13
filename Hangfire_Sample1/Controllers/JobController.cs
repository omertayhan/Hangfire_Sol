using Hangfire;
using Hangfire_Sample1.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire_Sample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost]
        [Route("RunTestJob")]
        public ActionResult RunTestJob()
        {
            BackgroundJob.Enqueue<TestJob>(x => x.WriteLog("Background job triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("CreateBackgroundJob")]
        public ActionResult CreateBackgroundJob()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Background job triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("CreateScheduledJob")]
        public ActionResult CreateScheduledJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);
            BackgroundJob.Schedule(() => Console.WriteLine("Scheduled job triggered"),dateTimeOffset);
            return Ok();
        }

        [HttpPost]
        [Route("CreateContinutaionJob")]
        public ActionResult CreateContinutaionJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Scheduled Job 2 Triggered"), dateTimeOffset);

            var job2Id = BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Continutaion job 1 triggered"));
            var job3Id = BackgroundJob.ContinueJobWith(job2Id, () => Console.WriteLine("Continutaion job 2 triggered"));
            var job4Id = BackgroundJob.ContinueJobWith(job3Id, () => Console.WriteLine("Continutaion job 3 triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("CreateRecurringJob")]
        public ActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate("RecurringJob1", () => Console.WriteLine("Recurring Job Triggered"), "* * * * *");
            return Ok();
        }
    }
}
