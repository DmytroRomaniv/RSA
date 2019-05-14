using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using RSA.Interfaces;
using RSA.Entities;
using System.Security.Cryptography;

namespace RSA
{
    public class RsaAlgorythm: IEncryptionAlgorythm
    {
        public BigInteger _pValue;
        public BigInteger _qValue;
        private Random _random = new Random((int)DateTime.Now.Ticks);

        public RsaAlgorythm()
        {
            while (_pValue < 100 || _qValue < 100)
            {
                _pValue = GenerateRandomPrimeInteger();
                _qValue = _pValue;
                while (_pValue == _qValue)
                {
                    _qValue = GenerateRandomPrimeInteger();
                }
            }
        }

        public RsaAlgorythm(BigInteger nValue, BigInteger qValue)
        {
            while (_pValue < 100 || _qValue < 100)
            {
                if (nValue <= 0 || !FermatsIsPrime(nValue))
                {
                    _pValue = GenerateRandomPrimeInteger();
                }
                else
                {
                    _pValue = nValue;
                }

                if (qValue <= 0 || !FermatsIsPrime(qValue))
                {
                    _qValue = _pValue;

                    while (_pValue == _qValue)
                    {
                        _qValue = GenerateRandomPrimeInteger();
                    }
                }
                else
                {
                    _qValue = qValue;
                }
            }
        }

        public int[] Encrypt(int[] message)
        {
            var nValue = _pValue * _qValue;
            var eulerValue = CalculateEulerFunction(_pValue, _qValue);
            var secretKey = GenerateCoprimeInteger(eulerValue);
            var secondSecretKey = UseExtendedEuclid(secretKey, eulerValue);

            var numberOfDivisions = nValue.ToString().Count() / 3;
            var dividedMessage = new Message(message, numberOfDivisions);
            var encodedBytes = new List<int>();

            foreach(var subMessage in dividedMessage.MessageBytes)
            {
                var str = new StringBuilder();
                foreach(var symbol in subMessage)
                {
                    str.Append(symbol.ToString("D3"));
                }

                BigInteger big;
                if (BigInteger.TryParse(str.ToString(), out big))
                {
                    var encodedMessage = ModularMultiplication(big, (long)secretKey, nValue);
                    var a = new Message(encodedMessage.ToString(), numberOfDivisions);
                    encodedBytes.AddRange(a.MessageBytes.FirstOrDefault());
                }
            }

            return encodedBytes.ToArray();
        }

        public byte[] Decrypt(byte[] encryptedMessage)
        {
            return new byte[0];
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
            var strongPrimes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 183, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 461 };
            var strongPrimesArray = new int[] { 2, 3, 5, 7, 11, 13, 17 };
            if (FindSanD(range, out var s, out var d))
            {
                foreach(var strPrime in strongPrimes)
                {
                    if(ModularMultiplication(value, 1, strPrime) == 0)
                    {
                        return false;
                    }
                }

                foreach (var strongPrime in strongPrimesArray)
                {
                    var checkResult = ModularMultiplication(strongPrime, (long)d, value);
                    if (BigInteger.Abs(checkResult) != 1)
                    {
                        for (int i = 0; i < numberOfTests; i++)
                        {
                            var r = LongRandom(2, (long)value, _random);
                            if (ModularMultiplication((long)Math.Pow(strongPrime, d), (long)Math.Pow(2, r), value) == -1)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
        private bool FindSanD(long range, out long s, out double d)
        {
            s = 1;
            d = range / Math.Pow(2, s); ;
            while (!((Math.Pow(2, s)*d == range) && Math.Round(d) == d && d%2==1) && d >= 3)
            {
                s++;
                d = range / Math.Pow(2, s);
            }

            return d >= 3;
        }

        public BigInteger ModularMultiplication(BigInteger value, long powerValue, BigInteger modularValue)
        {
            var result = new BigInteger(0);
            var binaryPowerValue = Convert.ToString(powerValue, 2);
            var firstValue = new BigInteger(1);
            var lastValue = value;


            foreach (var binaryNumber in binaryPowerValue)
            {
                if (binaryNumber == '1')
                {
                    firstValue = (firstValue * lastValue) % modularValue;
                }
                lastValue = BigInteger.Pow(firstValue, 2) % modularValue;

            }

            result = firstValue;

            return result;
        }

        public IEnumerable<byte> ConvertToBinary(BigInteger value)
        {
            var binaryNumber = new List<byte>();

            while(value > 0)
            {
                var symbol = value % 2;
                binaryNumber.Add((byte)symbol);
                value = value / 2;
            }

            if (binaryNumber.Any())
            {
                binaryNumber.Add(0);
            }

            return binaryNumber;
        }

        public BigInteger GenerateRandomInteger(long max = 300000000000000)
        {

            BigInteger result;

            var randomInteger = LongRandom(2, max, _random);
            result = new BigInteger(randomInteger);

            return result;
        }

        private long LongRandom(long min, long max, Random rand)
        {
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] buf = new byte[45];
            rNGCryptoServiceProvider.GetBytes(buf);
            //for(int i=0; i< buf.Length; i++)
            //{
            //    buf[i] = (byte)_random.Next(0, 2);
            //}

            var longRand = BitConverter.ToInt64(buf);
            return (Math.Abs(longRand % (max - min)) + min);
        }

        public BigInteger GenerateRandomPrimeInteger(long max = 300000000000000)
        {
            var result = new BigInteger(4);

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
            var random = new Random();
            BigInteger randomPrimeInteger = primeValue + 1;

            while (!(randomPrimeInteger < primeValue && AreItegersCoprime(primeValue, randomPrimeInteger)))
            {
                randomPrimeInteger = GenerateRandomPrimeInteger((long)primeValue);
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
            BigInteger f;
            BigInteger resultValue = firstValue % secondValue;

            while (resultValue > 0)
            {
                f = firstValue / secondValue;
                xn = x0 - f * x1;

                x0 = x1;
                x1 = xn;
                firstValue = secondValue;
                secondValue = resultValue;
                resultValue = firstValue % secondValue;
            }

            if(xn < 0)
            {
                xn = saveValue + xn;
            }

            return xn;
        }
    }
}
