﻿using MediatR;
using System;

namespace NerdStore.Core.Messages
{
    public class Event : Message, INotification 
    {
        public DateTime Timestamp { get; private set; }

        public Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
