using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventos.IO.Domain.Core.Notifications
{
    public class DomanNotificationHandler : IDomainNotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public DomanNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public void Handle(DomainNotification message)
        {
            _notifications.Add(message);
        }

        public bool HasNotifications()
        {
            return _notifications.Any();
        }

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }
    }
}
