using System;
using System.Runtime.Serialization;

namespace api.laterooms.co.uk.Exceptions
{
    /// <summary>
    /// Custom exceptions for the Insights library
    /// </summary>
    [Serializable]
    public class LateRoomsApiExcepetion : Exception
    {
        public LateRoomsApiExcepetion()
        {
        }

        public LateRoomsApiExcepetion(string message)
            : base(message)
        {
        }

        public LateRoomsApiExcepetion(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public LateRoomsApiExcepetion(Exception innerException, string message)
            : base(message, innerException)
        {            
        }

        public LateRoomsApiExcepetion(Exception innerException, string format, params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected LateRoomsApiExcepetion(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {            
        }
    }
}
