using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoFilterDataGrid.Model
{
    public enum NumericFilterSelectionType
    {
        EQUAL,
        NOT_EQUAL,
        GREATER_THAN,
        GREATER_THAN_OR_EQUALTO,
        LESS_THAN,
        LESS_THAN_OR_EQUALTO,
        RANGE,
        CUSTOM,
    }
}
