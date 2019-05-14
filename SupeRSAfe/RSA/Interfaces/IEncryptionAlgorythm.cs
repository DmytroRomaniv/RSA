using System;
using System.Collections.Generic;
using System.Text;

namespace RSA.Interfaces
{
    public interface IEncryptionAlgorythm
    {
        int[] Encrypt(int[] message);

        byte[] Decrypt(byte[] encryptedMessage);
    }
}
