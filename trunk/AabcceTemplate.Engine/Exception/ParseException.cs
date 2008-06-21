using System;
using System.Collections.Generic;
using System.Text;

namespace AabcceTemplate.Engine.Exception
{
    /// <summary>
    /// ParseException
    /// </summary>
    [Serializable]
    public class ParseException : System.Exception
    {
        public ParseException():base() 
        { 
             
        } 
 
        public ParseException(string message):base(message) 
        { 
             
        } 
 
        public ParseException(string message,System.Exception innerException):base(message,innerException) 
        { 
 
        }

        protected ParseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) 
        { 
             
        } 
    }
}
