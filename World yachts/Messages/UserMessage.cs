using System;
using World_yachts.Model.Database.Models;

namespace World_yachts.Messages
{
    public class UserMessage : IMessage<v_user>
    {
        public v_user Message { get; set; }

        public UserMessage(v_user message)
        {
            Message = message;
        }
    }
}
