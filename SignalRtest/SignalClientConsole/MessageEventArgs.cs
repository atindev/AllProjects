﻿namespace SignalClientConsole
{
    class MessageEventArgs
    {
        public MessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
