using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Jumia_Api.Infrastructure.Hubs
{
  
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

            if (userId != null)
            {
                // Add user to their personal group
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");

                // Add admins to admin group
                if (userRole == "Admin")
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
                }

                // Get user's active chat and join chat group
                if (userRole != "Admin")
                {
                    var userChats = await _chatService.GetUserChatAsync(userId);
                    var activeUserChat = userChats?.FirstOrDefault(c => c.Status == ChatStatus.Active.ToString());
                    if (activeUserChat != null)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{activeUserChat.Id}");
                    }
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

            if (userId != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");

                if (userRole == "Admin")
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinChatGroup(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        }

        public async Task LeaveChatGroup(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        }

        public async Task MarkMessagesAsRead(string chatId)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null && Guid.TryParse(chatId, out var chatGuid))
            {
                await _chatService.MarkMessagesAsReadAsync(chatGuid, userId);
            }
        }



        [Authorize(Roles = "Admin")]
        public async Task AssignChat(string chatId)
        {
            var adminId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var adminName = Context.User?.FindFirst(ClaimTypes.Name)?.Value; // Assuming Name claim holds the admin's name

            if (adminId == null || !Guid.TryParse(chatId, out var chatGuid))
            {
                // Optionally send an error back to the client or log
                return;
            }

            try
            {
                var assignedChat = await _chatService.AssignChatToAdminAsync(chatGuid, adminId, adminName);
                // Notify only the admin who assigned the chat and the user whose chat was assigned
                await Clients.Group($"User_{assignedChat.UserId}").SendAsync("ChatAssigned", assignedChat);
                await Clients.Caller.SendAsync("ChatAssigned", assignedChat); // Notify the assigning admin
                await Clients.Group("Admins").SendAsync("ChatAssignedToAdmin", assignedChat); // Notify all admins about the assignment
            }
            catch (ArgumentException ex)
            {
                // Handle chat not found or other service-level errors
                await Clients.Caller.SendAsync("ChatAssignmentError", $"Error assigning chat: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task CloseChat(string chatId)
        {
            if (!Guid.TryParse(chatId, out var chatGuid))
            {
                return;
            }

            try
            {
                var closedChat = await _chatService.CloseChatAsync(chatGuid);
                // Notify all participants and admins that the chat is closed
                await Clients.Group($"Chat_{chatId}").SendAsync("ChatClosed", closedChat);
                await Clients.Group("Admins").SendAsync("ChatClosedByAdmin", closedChat);
                // Remove all connections from this chat group
                await Clients.Group($"Chat_{chatId}").SendAsync("ForceLeaveChatGroup", chatId); // Custom event to tell clients to leave
            }
            catch (ArgumentException ex)
            {
                await Clients.Caller.SendAsync("ChatCloseError", $"Error closing chat: {ex.Message}");
            }
        }
    }
}

