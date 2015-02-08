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
    /********************/
    [Serializable()]
    public abstract class ControlLoopException : LatrunculiException
    {
        public ControlLoopException(string message)
            : base(message)
        {
        }

        protected ControlLoopException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
    [Serializable()]
    public class ControlLoopResetRequestedException : ControlLoopException
    {
        public ControlLoopResetRequestedException()
            : base("Operaci nelze provést - byl vyžádán reset řídicí smyčky.")
        {
        }

        protected ControlLoopResetRequestedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable()]
    public class ControlLoopQuitException : ControlLoopException
    {
        public ControlLoopQuitException()
            : base("Operaci nelze provést - bylo vyžádáno ukončení aplikace.")
        {
        }

        protected ControlLoopQuitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

/********************/
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

    [Serializable()]
    public class UnableToDeterminePlayerByColor : LatrunculiException
    {
        public UnableToDeterminePlayerByColor()
            : base("Nepodařilo se nalézt hráče s požadovanou barvou.")
        {
        }

        protected UnableToDeterminePlayerByColor(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
