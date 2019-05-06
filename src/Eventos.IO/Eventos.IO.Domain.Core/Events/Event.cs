using System;
using System.Collections.Generic;
using System.Text;

namespace Eventos.IO.Domain.Core.Events
{
    public class Event : Message
    {
        public DateTime Timestamp { get; set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
