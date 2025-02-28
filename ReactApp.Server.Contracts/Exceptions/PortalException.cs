using Microsoft.Extensions.Logging;
using System.Runtime.Serialization;

namespace ReactApp.Server.Contracts.Exceptions
{
    [Serializable]
    public class PortalException : Exception
    {
        public PortalException()
        {
        }

        public PortalException(string message)
            : base(message)
        {
        }

        public PortalException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        /// <inheritdoc/>
        [Obsolete("Need to remove in the feature")]
        protected PortalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        /// <inheritdoc/>
        [Obsolete("Need to remove in the feature")]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
