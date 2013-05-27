using System;

namespace AsbaBank.Core.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAuthorizeAttribute : Attribute
    {
        public string Role { get; private set; }

        public CommandAuthorizeAttribute(string role)
        {
            Role = role;
        }
    }
}
