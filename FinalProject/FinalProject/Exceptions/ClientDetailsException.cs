using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Exceptions
{
    internal class ClientDetailsException : Exception
    {
        public ClientDetailsException(string? message) : base(message)
        {
        }
    }
}
