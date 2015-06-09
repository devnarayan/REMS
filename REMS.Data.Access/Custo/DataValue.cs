using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access
{
  
        public class DataValue
        {
            public static int AssuredReturnTDSLimit()
            {
                return 20000;
            }
            public static decimal AssuredReturnTDS()
            {
                return 10;
            }
            public static decimal AssuredReturnInterest()
            {
                decimal d = 12.5M;
                return d;
            }
        }
    
}
