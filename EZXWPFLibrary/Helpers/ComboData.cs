using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZXWPFLibrary.Helpers
{
    public class ComboData
    {
        public object Name { get; set; }
        public object Value { get; set; }
        public ComboData(object name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
