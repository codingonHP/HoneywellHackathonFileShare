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

        public void ShareFile(HttpPostedFileBase file)
        {
            var room = Context.Request.QueryString["room"];

            DocumentManager documentManager = new DocumentManager();
            byte[] fileBytes = new byte[file.ContentLength];
            int randIndex = DateTime.Now.Millisecond + new Random(777777).Next();

            try
            {
                if (file.InputStream.CanRead)
                {
                    file.InputStream.Position = 0;
                    file.InputStream.Read(fileBytes, 0, file.ContentLength);
                }

                var fname = file.FileName;
                fileBytes = CompressionUtil.CompressFile(fileBytes, file.FileName);
                fileBytes = EncryptionUtil.EncryptFile(fileBytes);

                var fileName = fname + "_" + randIndex + ".zip";
                string targetFolder = HttpContext.Current.Server.MapPath("~/Store/Documents/" + room);

                documentManager.SaveFile(fileBytes, fileName, targetFolder);

                Clients.OthersInGroup(room).NotifyFileShare("~/Store/Documents/" + room + "/" + fileName);
            }
            catch (Exception ex)
            {

            }
        }

        public void CreateRoom(string roomName)
        {
            if (RoomsDictionary.ContainsKey(roomName))
            {
                throw new Exception("room already exists");
            }

            RoomsDictionary.Add(roomName, 1);
            SetupDirectory(roomName, "/CreateDirectory");
        }

        private void SetupDirectory(string room, string requestUrl)
        {

            using (var wb = new WebClient())
            {
                var data = new NameValueCollection { ["room"] = room };
                wb.UploadValues(requestUrl, "POST", data);
            }
        }
    }

    public class User
    {
        public string ConnectionId { get; set; }
    }
}