using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoneywellHackathonFileShare.Hub
{
    public class DocumentHub : Microsoft.AspNet.SignalR.Hub
    {
        private Dictionary<string, string> _userMapping = new Dictionary<string, string>();

        public void Hello()
        {
            Clients.All.hello();
        }

        //public override Task OnConnected()
        //{
          
        //}
    }
}