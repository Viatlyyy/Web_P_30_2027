using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System;

namespace WebApplication1.Pages.Chat
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private static List<ChatMessage> _messages = new List<ChatMessage>();

        public List<ChatMessage> PreviousMessages => _messages;

        public void OnGet()
        {
        }

        public static void AddMessage(string userName, string message, DateTime timestamp)
        {
            _messages.Add(new ChatMessage { UserName = userName, Message = message, Timestamp = timestamp });
            if (_messages.Count > 100) _messages.RemoveAt(0);
        }
    }

    public class ChatMessage
    {
        public string UserName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}