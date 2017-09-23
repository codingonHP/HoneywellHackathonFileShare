using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using HoneywellHackathonFileShare.Util;

namespace HoneywellHackathonFileShare.Hub
{
    public class DocumentHub : Microsoft.AspNet.SignalR.Hub
    {
        public static ConcurrentDictionary<string, User> ConnectedUsers = new ConcurrentDictionary<string, User>();
        public static Dictionary<string, int> RoomsDictionary = new Dictionary<string, int>();

        public override Task OnConnected()
        {
            var room = Context.Request.QueryString["room"];
            room = "default";

            if (RoomsDictionary.ContainsKey(room))
            {
                RoomsDictionary[room] += 1;
            }
            else
            {
                var requestUrl = "/CreateDirectory";
                RoomsDictionary.Add(room, 1);
                SetupDirectory(room, requestUrl);
            }

            ConnectedUsers.TryAdd(Context.ConnectionId, new User { ConnectionId = Context.ConnectionId });
            Groups.Add(room, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            User garbage;
            ConnectedUsers.TryRemove(Context.ConnectionId, out garbage);

            var room = Context.Request.QueryString["room"];
            room = "default";

            if (RoomsDictionary.ContainsKey(room))
            {
                RoomsDictionary[room] -= 1;

                if (RoomsDictionary[room] == 0)
                {
                    RoomsDictionary.Remove(room);
                    var requestUrl = "/DeleteDirectory";
                    SetupDirectory(room, requestUrl);
                }
            }

            Groups.Remove(Context.ConnectionId, room);

            return base.OnDisconnected(stopCalled);
        }

        public void ShareFile(string room, string fileName)
        {
            Clients.OthersInGroup(room).NotifyFileShare("~/Store/Documents/" + room + "/" + fileName);
            Clients.Others.NotifyFileShare("hello");
        }

        public void CreateRoom(string roomName)
        {
            roomName = "default";
            if (RoomsDictionary.ContainsKey(roomName))
            {
                throw new Exception("room already exists");
            }

            RoomsDictionary.Add(roomName, 1);
            SetupDirectory(roomName, "/CreateDirectory");
        }

        private void SetupDirectory(string room, string requestUrl)
        {

            //using (var wb = new WebClient())
            //{
            //    var data = new NameValueCollection { ["room"] = room };
            //    wb.UploadValues(requestUrl, "POST", data);
            //}
        }

        public void SendMessage(string message)
        {
            Clients.Others.NotifyMessage(message);
        }
    }

    public class User
    {
        public string ConnectionId { get; set; }
    }
}