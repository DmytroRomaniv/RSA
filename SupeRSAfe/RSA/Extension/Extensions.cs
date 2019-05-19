using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RSA.Extension
{
    public static class Extensions
    {
        public static string ToBinaryString(this BigInteger bigint)
        {
            var bytes = bigint.ToByteArray();
            var idx = bytes.Length - 1;
            
            var base2 = new StringBuilder(bytes.Length * 8);
            
            var binary = Convert.ToString(bytes[idx], 2);
            
            if (binary[0] != '0' && bigint.Sign == 1)
            {
                base2.Append('0');
            }
            
            base2.Append(binary);
            
            for (idx--; idx >= 0; idx--)
            {
                base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
            }

            return base2.ToString();
        }
    }
}
