using System;
using System.Collections.Generic;
using System.Text;

namespace RSA.Entities
{
    public class Message
    {
        public byte[] MessageBytes
        {
            get
            {

                return MessageBytes;
            }
            set
            {
                if (value.Length < 1023)
                {
                    for (int i = 0; i < 1023; i++)
                    {
                    }
                    MessageBytes = value;
                }
                else
                {
                    MessageBytes = value;
                }
            }
        }
    }
}
