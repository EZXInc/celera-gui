using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoFilterDataGrid.Model
{
    public enum OperatorType
    {
        NA,
        EXISTS,
        NOT_EXISTS,
        EQUAL,
        NOT_EQUAL,
        LESS_THAN_OR_EQUAL,
        LESS_THAN,
        GREATER_THAN_OR_EQUAL,
        GREATER_THAN,
    }
}
