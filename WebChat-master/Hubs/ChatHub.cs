using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebChat.Common;
using WebChat.Entities;

namespace WebChat.Hubs
{
    public class ChatHub : Hub

    {
		readonly WebChatDbContext db;

		public ChatHub(WebChatDbContext _db)
		{
			db = _db;
		}

		public async Task GuiTinNhan(string targetUserId, string message)
        {
            var currentUserId = Context.UserIdentifier;
            var users = new string[] { currentUserId, targetUserId };
            var response = new
            {
                sender = currentUserId,
                reciver = targetUserId,
                mesg = message,
                datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") 
            };
            await Clients.Users(users).SendAsync("PhanHoiTinNhan", response);

			AppMessage mesg = new AppMessage
			{
				Message = AESThenHMAC.SimpleEncryptWithPassword(message, AppConfig.MESG_KEY),
				SendAt = DateTime.Now,
				ReciverId = Convert.ToInt32(targetUserId),
				SenderId = Convert.ToInt32(targetUserId)
			};
			await db.AddAsync(mesg);
			await db.SaveChangesAsync();
        }

		static List<int> onlineUsers = new List<int>();
		public override async Task <Task> OnConnectedAsync()
		{
			var currentUserId = Context.UserIdentifier;
			onlineUsers.Add(Convert.ToInt32(currentUserId));

			var response = new
			{
				onlineUsers
			};
			await Clients.All.SendAsync("GetUsers", response);
			return base.OnConnectedAsync();
		}

		public override async Task <Task> OnDisconnectedAsync(Exception exception)
		{
			var currentUserId = Context.UserIdentifier;
			onlineUsers.Remove(Convert.ToInt32(currentUserId));

			var response = new
			{
				onlineUsers,
				disconnectedId = Convert.ToInt32(currentUserId)
			};
			await Clients.All.SendAsync("GetUsers", response);
			return base.OnDisconnectedAsync(exception);
		}
	}
}
