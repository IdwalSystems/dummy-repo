using System;

namespace MSNK.Models.Modules
{
    public class Notification
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public DateTime NotificationDateTime { get; set; }
    }
}
