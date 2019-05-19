using System;
using System.Collections.Generic;
using System.Text;

namespace RSA.Interfaces
{
    public interface IEncryptionAlgorythm
    {
        string Encrypt(string message);

        string Decrypt(string encryptedMessage);
    }
}
