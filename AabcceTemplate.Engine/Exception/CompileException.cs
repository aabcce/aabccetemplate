using System;
using System.Collections.Generic;
using System.Text;

namespace AabcceTemplate.Engine.Exception
{
    /// <summary>
    /// CompileException
    /// </summary>
    [Serializable]
    public class CompileException : System.Exception
    {
        public CompileException():base() 
        { 
             
        } 
 
        public CompileException(string message):base(message) 
        { 
             
        } 
 
        public CompileException(string message,System.Exception innerException):base(message,innerException) 
        { 
 
        }

        protected CompileException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) 
        { 
             
        } 
    }
}
