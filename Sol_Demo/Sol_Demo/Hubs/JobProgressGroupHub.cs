using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sol_Demo.Hubs
{
    public class JobProgressGroupHub : Hub
    {
        // Send a Message on Particular Job Id.
        public async Task AssociateJob(string jobId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, jobId);
        }
    }
}