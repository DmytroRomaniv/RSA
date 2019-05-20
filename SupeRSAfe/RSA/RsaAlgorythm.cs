using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using RSA.Interfaces;
using System.Security.Cryptography;
using RSA.Extension;
using System.Threading.Tasks;

namespace RSA
{
    public class RsaAlgorithm : IEncryptionAlgorithm
    { 
        public BigInteger PValue;
        public BigInteger QValue;
        public BigInteger SecretKey { get; set; }
        public BigInteger PublicKey { get; set; }
        private Random _random = new Random((int)DateTime.Now.Ticks);

        public RsaAlgorithm()
        {
            while (PValue < 100 || QValue < 100)
            {
                PValue = GenerateRandomPrimeInteger();
                QValue = PValue;
                while (PValue == QValue)
                {
                    QValue = GenerateRandomPrimeInteger();
                }
            }
        }

        public RsaAlgorithm(BigInteger nValue, BigInteger qValue)
        {
            if (nValue <= 0 || !FermatsIsPrime(nValue))
            {
                PValue = GenerateRandomPrimeInteger();
            }
            else
            {
                PValue = nValue;
            }

            if (qValue <= 0 || !FermatsIsPrime(qValue))
            {
                QValue = PValue;

                while (PValue == QValue)
                {
                    QValue = GenerateRandomPrimeInteger();
                }
            }
            else
            {
                QValue = qValue;
            }
        }

        public async Task<string> Encrypt(string message)
        {
            string result = string.Empty;
            await Task.Run(() =>
            {
                PublicKey = PValue * QValue;
                var eulerValue = CalculateEulerFunction(PValue, QValue);
                SecretKey = GenerateCoprimeInteger(eulerValue);
                var secondSecretKey = UseExtendedEuclid(SecretKey, eulerValue);
                var messageLength = PublicKey.ToString().Length;
                var dividedMessage = DivideMessage(message, messageLength);
                var encodedMessages = new List<string>();

                foreach (var subMessage in dividedMessage)
                {
                    string partMessage = subMessage;
                    if (partMessage.Length < messageLength)
                    {
                        partMessage = partMessage.PadRight(PublicKey.ToString().Length - 1, '0');
                    }

                    if (BigInteger.TryParse(partMessage, out var big))
                    {
                        var encodedMessage = ModularMultiplication(big, secondSecretKey, PublicKey);
                        encodedMessages.Add(encodedMessage.ToString());
                    }
                }

                result = ConvertToString(encodedMessages);
            });

            return result;
        }


        public async Task<string> Decrypt(string encryptedMessage, BigInteger publicKey, BigInteger secretKey)
        {
            this.PublicKey = publicKey;
            this.SecretKey = secretKey;

            return await Decrypt(encryptedMessage);

        }
        public async Task<string> Decrypt(string encryptedMessage)
        {
            string result = string.Empty;

            if (PublicKey != null && PublicKey > 1 && SecretKey != null && SecretKey > 1)
            {
                await Task.Run(() =>
                {
                    var messageLength = PublicKey.ToString().Length;
                    var dividedMessage = DivideMessage(encryptedMessage, messageLength);

                    var decryptedMessages = new List<string>();

                    foreach (var subString in dividedMessage)
                    {
                        if (!BigInteger.TryParse(subString, out var big))
                            continue;
                        var decryptedMessage = ModularMultiplication(big, SecretKey, PublicKey);
                        decryptedMessages.Add(decryptedMessage.ToString());
                    }

                    result = ConvertToString(decryptedMessages);

                });
            }

            return result;
        }

        private IEnumerable<string> DivideMessage(string message, int lenght)
        {
            var dividedMessage = new List<string>();

            var index = message.Length / lenght;
            for (var i = 0; i <= index; i++)
            {
                var start = i * lenght;
                string subMessage = start + lenght < message.Length ? message.Substring(start, lenght) : message.Substring(start);
                dividedMessage.Add(subMessage);
            }

            return dividedMessage;
        }

        private string ConvertToString(IEnumerable<string> dividedMessage)
        {
            var returnMessage = new StringBuilder();

            foreach (var subMessage in dividedMessage)
            {
                returnMessage.Append(subMessage);
            }

            return returnMessage.ToString();
        }

        public bool FermatsIsPrime(BigInteger value, int numberOfTests = 128)
        {
            if (value == 2 || value == 3)
            {
                return true;
            }
            if (value <= 1 || value % 2 == 0 || value % 3 == 0 || value % 5 == 0)
            {
                return false;
            }

            var range = (long)value - 1;
            var strongPrimes = new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 183, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 461, 1215, 34862, 379215 };
            var strongPrimesArray = new[] { 2, 3, 5, 7, 11, 13, 17 };

            if (!FindSanD(range, out var s, out var d))
                return false;

            foreach (var strongPrime in strongPrimesArray)
            {
                var checkResult = ModularMultiplication(strongPrime, new BigInteger(d), value);
                if (BigInteger.Abs(checkResult) == 1)
                    continue;

                foreach (var probableStrongPrime in strongPrimes)
                {
                    var firstCheck = ModularMultiplication(strongPrime, range, value);
                    var secondCheck = ModularMultiplication(strongPrime, range / probableStrongPrime, value);
                    if (firstCheck != 1 || secondCheck == 1)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < numberOfTests; i++)
                {
                    var r = LongRandom(2, value, _random);
                    if (BigInteger.Abs(ModularMultiplication((long)Math.Pow(strongPrime, d), (long)Math.Pow(2, (double)r), value)) == 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool FindSanD(long range, out long s, out double d)
        {
            s = 1;
            d = range / Math.Pow(2, s); ;
            while (!((Math.Pow(2, s) * d == range) && Math.Round(d) == d && d % 2 == 1) && d >= 3)
            {
                s++;
                d = range / Math.Pow(2, s);
            }

            return d >= 3;
        }

        public BigInteger ModularMultiplication(BigInteger value, BigInteger powerValue, BigInteger modularValue)
        {
            BigInteger result;
            var binaryPowerValue = powerValue.ToBinaryString().Reverse();
            var firstValue = new BigInteger(1);
            var lastValue = value;


            foreach (var binaryNumber in binaryPowerValue)
            {
                if (binaryNumber == '1')
                {
                    firstValue = (firstValue * lastValue) % modularValue;
                }
                lastValue = BigInteger.Pow(lastValue, 2) % modularValue;

            }

            result = firstValue;

            return result;
        }

        public BigInteger GenerateRandomIngInteger()
        {
            return GenerateRandomInteger(long.MaxValue);
        }

        public BigInteger GenerateRandomInteger(BigInteger max)
        {
            var randomInteger = LongRandom(2, max, _random);

            return randomInteger;
        }

        private BigInteger LongRandom(BigInteger min, BigInteger max, Random rand)
        {
            var rNgCryptoServiceProvider = new RNGCryptoServiceProvider();
            var buf = new byte[64];
            rNgCryptoServiceProvider.GetBytes(buf);

            var longRand = BitConverter.ToInt64(buf);
            return BigInteger.Abs(ModularMultiplication(longRand, 1, max - min) + min);
        }

        public BigInteger GenerateRandomPrimeInteger()
        {
            return GenerateRandomPrimeInteger(long.MaxValue);
        }

        public BigInteger GenerateRandomPrimeInteger(BigInteger max)
        {
            BigInteger result;

            do
            {
                result = GenerateRandomInteger(max);
            } while (!FermatsIsPrime(result));

            return result;
        }

        public BigInteger CalculateEulerFunction(BigInteger pValue, BigInteger qValue)
        {
            var eulerFunctionResult = (pValue - 1) * (qValue - 1);
            return eulerFunctionResult;
        }

        public BigInteger GenerateCoprimeInteger(BigInteger primeValue)
        {
            BigInteger randomPrimeInteger = primeValue + 1;

            while (!(randomPrimeInteger < primeValue && AreItegersCoprime(primeValue, randomPrimeInteger)))
            {
                randomPrimeInteger = GenerateRandomPrimeInteger(primeValue);
            }

            return randomPrimeInteger;
        }

        public bool AreItegersCoprime(BigInteger firstValue, BigInteger secondValue)
        {
            if (firstValue == secondValue)
            {
                return false;
            }

            while (firstValue > 1 && secondValue > 1)
            {
                if (firstValue < secondValue)
                {
                    SwapIntegers(ref firstValue, ref secondValue);
                }

                firstValue = firstValue % secondValue;
            }

            return (firstValue == 1 || secondValue == 1);
        }

        private void SwapIntegers(ref BigInteger firstValue, ref BigInteger secondValue)
        {
            var saveValue = firstValue;
            firstValue = secondValue;
            secondValue = saveValue;
        }

        public BigInteger UseExtendedEuclid(BigInteger firstValue, BigInteger secondValue)
        {
            var saveValue = secondValue;
            BigInteger x0 = 1;
            BigInteger xn = 1;
            BigInteger x1 = 0;
            BigInteger resultValue = firstValue % secondValue;

            while (resultValue > 0)
            {
                var f = firstValue / secondValue;
                xn = x0 - f * x1;

                x0 = x1;
                x1 = xn;
                firstValue = secondValue;
                secondValue = resultValue;
                resultValue = firstValue % secondValue;
            }

            if (xn < 0)
            {
                xn = saveValue + xn;
            }

            return xn;
        }
    }
}
