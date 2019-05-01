using System;
using System.Collections.Generic;
using System.Text;

namespace RSA.Interfaces
{
    public interface IEncryptionAlgorythm
    {
        byte[] Encrypt(byte[] message);

        byte[] Decrypt(byte[] encryptedMessage);
    }
}
