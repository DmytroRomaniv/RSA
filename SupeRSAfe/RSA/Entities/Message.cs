using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RSA.Entities
{
    public class Message
    {
        public List<int[]> MessageBytes;

        public Message(int[] message, int length)
        {
            MessageBytes = new List<int[]>();

            for (int j = 0; j < message.Length / length; j++)
            {
                var bytes = new int[length];
                for (int i = 0; i < message.Length; i++)
                {
                    bytes[i % length] = message[i];
                }
                MessageBytes.Add(bytes);
            }
        }

        public Message(string messageInBytes, int length)
        {
            var bytes = WholeChunks(messageInBytes, 3).ToList();;
            
            MessageBytes = new List<int[]>();

            for (int j = 0; j < bytes.Count; j++)
            {
                var bytesArray = new int[length];
                for (int i = 0; i < bytes.Count; i++)
                {
                    byte byteElement;
                    byte.TryParse(bytes[i], out byteElement);
                    bytesArray[i % length] = byteElement;
                }
                MessageBytes.Add(bytesArray);
            }
        }

        public override string ToString()
        {
            var message = new StringBuilder();

            foreach(var line in MessageBytes)
            {
                foreach(var character in line)
                {
                    message.Append(character.ToString("D3"));
                }
            }

            return message.ToString();
        }

        private IEnumerable<string> WholeChunks(string str, int chunkSize)
        {
            var length = str.Length / chunkSize + 1;
            var subStrings = new List<string>();

            for (int i = 0; i < length; i++)
            {
                var sub = str.AsQueryable<char>().TakeLast(chunkSize);
                subStrings.Add(str);
            }

            return subStrings;
        }
    }
}
