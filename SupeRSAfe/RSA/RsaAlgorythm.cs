using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using RSA.Interfaces;

namespace RSA
{
    public class RsaAlgorythm: IEncryptionAlgorythm
    {
        public BigInteger _pValue;
        public BigInteger _qValue;

        public RsaAlgorythm(BigInteger nValue, BigInteger qValue)
        {
            if (nValue <= 0 || !IsPrime(nValue))
            {
                _pValue = GenerateRandomPrimeInteger();
            }
            else
            {
                _pValue = nValue;
            }

            if (qValue <= 0 || !IsPrime(qValue))
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

        public byte[] Encrypt(byte[] message)
        {
            var nValue = _pValue * _qValue;
            var eulerValue = CalculateEulerFunction(_pValue, _qValue);
            var secretKey = GenerateCoprimeInteger(eulerValue);

            var dividedMessage = DivideMessage(message);

            foreach(var subMessage in dividedMessage)
            {

            }

            return new byte[0];
        }

        public byte[] Decrypt(byte[] encryptedMessage)
        {
            return new byte[0];
        }

        public bool IsPrime(BigInteger value, int numberOfTests = 512)
        {
            if (value == 2 || value == 3)
            {
                return true;
            }
            if (value <= 1 || value % 2 == 0)
            {
                return false;
            }

            var start = 0;
            var range = value - 1;
            while (range % 2 == 0)
            {
                start++;
                range = range / 2;
            }
            for (var i = 0; i < numberOfTests; i++)
            {
                var randomNumber = GenerateRandomInteger();
                var x = ModularMultiplication(randomNumber, range, value);
                if (x != 1 && x != value - 1)
                {
                    var j = 1;
                    while (j < start && x != value - 1)
                    {
                        x = ModularMultiplication(x, 2, value);
                        if (x == 1)
                        {
                            return false;
                        }
                        j += 1;
                    }
                    if (x != value - 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        

        public BigInteger ModularMultiplication(BigInteger value, BigInteger powerValue, BigInteger modularValue)
        {
            var result = new BigInteger(0);
            var binaryPowerValue = powerValue.ToByteArray().Reverse();
            var firstValue = new BigInteger(1);
            var lastValue = value;


            foreach (var binaryNumber in binaryPowerValue)
            {
                if (binaryNumber == 1)
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

        public BigInteger GenerateRandomInteger(int length = 1024)
        {
            byte[] bytes = new byte[length];
            BigInteger result;
            var random = new Random();

            random.NextBytes(bytes);
            bytes[bytes.Length - 1] &= (byte)0x7F;
            result = new BigInteger(bytes);

            return result;
        }
    
        public BigInteger GenerateRandomPrimeInteger(int length = 1024)
        {
            var result = new BigInteger(4);

            do
            {
                result = GenerateRandomInteger();
            } while (!IsPrime(result));

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
                var randomLength = random.Next(64, 1025);
                randomPrimeInteger = GenerateRandomPrimeInteger(randomLength);
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

        private byte[,] DivideMessage(byte[] message)
        {
            var numberOfDivision = message.Length / 1024;
            var dividedMessage = new byte[numberOfDivision, 1024];

            for(var i = 0; i <= numberOfDivision; i++)
            {
                for(int j = 0; j < 1024; j++)
                {
                    dividedMessage[i, j] = message[j % 1024];
                }
            }

            return dividedMessage;
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
