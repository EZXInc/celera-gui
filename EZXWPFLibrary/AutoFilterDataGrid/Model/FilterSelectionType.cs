using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoFilterDataGrid.Model
{
    public enum FilterSelectionType
    {
        NA,
        NUMERIC_EQ,
        NUMERIC_NE,
        NUMERIC_GT,
        NUMERIC_LT,
        NUMERIC_GE,
        NUMERIC_LE,
        NUMERIC_RNG,
        NUMERIC_CUST,
    }
}
