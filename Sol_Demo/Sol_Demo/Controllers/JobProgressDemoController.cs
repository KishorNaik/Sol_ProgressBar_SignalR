using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Coravel.Queuing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Sol_Demo.Hubs;

namespace Sol_Demo.Controllers
{
    public class JobProgressDemoController : Controller
    {
        #region Declaration

        // Step 1
        private readonly IQueue queue = null;

        private readonly IHubContext<JobProgressGroupHub> hubContext = null;

        #endregion Declaration

        #region Constructor

        // Step 2
        public JobProgressDemoController(IQueue queue, IHubContext<JobProgressGroupHub> hubContext)
        {
            this.queue = queue;
            this.hubContext = hubContext;
        }

        #endregion Constructor

        #region Private Method

        // Step 5
        private async Task PerformSomeBackgroundJob(string jobId)
        {
            for (int counter = 0; counter <= 100; counter += 5)
            {
                // Send a Message to Client side using SignalR Hub for real time communication from server to client.
                await hubContext.Clients.Group(jobId).SendAsync("progress", counter);

                await Task.Delay(1000);
            }
        }

        #endregion Private Method

        #region Actions

        // Step 3 : Add a View
        public IActionResult Index()
        {
            return View();
        }

        // Step 6: Start Backgroung progress
        [HttpPost("start-progress", Name = "StartProgress")]
        public IActionResult StartLongRunningBackgroungQueueProgress()
        {
            try
            {
                // Generate a Unique Id for new Request for running background Job
                string jobId = Guid.NewGuid().ToString("N");

                // Queue Background Job in Server
                queue.QueueAsyncTask(() => this.PerformSomeBackgroundJob(jobId));

                // redirect to Display Progress Action
                return base.RedirectToRoute("DisplayProgress", new { jobId });
            }
            catch
            {
                throw;
            }
        }

        // Step 7: Display Progress Bar for Long running task (Add a View)
        [HttpGet("display-progress/{jobId}", Name = "DisplayProgress")]
        public IActionResult DisplayProgressBar(string jobId)
        {
            ViewBag.JobId = jobId;

            return View();
        }

        #endregion Actions
    }
}