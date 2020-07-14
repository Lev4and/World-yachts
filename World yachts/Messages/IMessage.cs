using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World_yachts.Messages
{
    public interface IMessage<T> where T : class
    {
        T Message { get; set; }
    }
}
