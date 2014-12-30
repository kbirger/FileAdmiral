using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAdmiral.IoC
{
    public struct ConstructorParameter
    {
        public string Key;
        public object Value;

        public ConstructorParameter(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
