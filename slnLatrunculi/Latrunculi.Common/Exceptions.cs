using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    [Serializable()]
    public abstract class LatrunculiException : Exception
    {
        protected LatrunculiException()
            : base()
        {
        }
        
        public LatrunculiException(string message)
            : base(message)
        {
        }

        public LatrunculiException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        protected LatrunculiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable()]
    public class MoveInvalidException : LatrunculiException
    {
        public MoveInvalidException(string str, Exception innerException)
            : base(string.Format("Z řetězce {0} nelze sestavit platný tah, protože {1}", 
                    str, (innerException == null) ? "došlo k chybě." : innerException.Message))
        {
        }

        protected MoveInvalidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
