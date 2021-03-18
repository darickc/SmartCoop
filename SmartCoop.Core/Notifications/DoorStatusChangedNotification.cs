using System;
using MediatR;
using SmartCoop.Core.Devices;

namespace SmartCoop.Core.Notifications
{
    public class DoorStatusChangedNotification : INotification
    {
        public IDoor Door { get; }
        public DateTime DateTime { get; }

        public DoorStatusChangedNotification(IDoor door, DateTime dateTime)
        {
            Door = door;
            DateTime = dateTime;
        }
    }
}