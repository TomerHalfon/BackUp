using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesPojectShared.Entities
{
    public class LogData
    {
        public string Message { get; }
        public Box Box { get; } = null;
        public bool? Status { get; } = null;

        public LogData() { }
        public LogData(string message)
        {
            Message = message;
        }
        public LogData(string message, bool status) : this(message)
        {
            Status = status;
        }
        public LogData(string message, Box box):this(message)
        {
            Box = box;
        }
        public LogData(string message, Box box, bool status) : this(message, box)
        {
            Status = status;
            Box = box;
        }
    }
}
