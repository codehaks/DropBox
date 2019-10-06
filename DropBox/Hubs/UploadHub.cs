using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DropBox.Hubs
{
    public class UploadHub : Hub<IUploadHub>
    {
        //public async Task SendProgress(int progress)
        //{
            
        //    await Clients.All.SendProgress(progress);
        //    //Clients.All.SendProgress("UpdateProgress", progress);
        //}
    }
}
