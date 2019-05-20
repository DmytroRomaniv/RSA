using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RSA.Interfaces
{
    public interface IEncryptionAlgorithm
    {
        BigInteger SecretKey { get; set; }
        BigInteger PublicKey { get; set; }

        Task<string> Encrypt(string message);

        Task<string> Decrypt(string encryptedMessage);
    }
}
