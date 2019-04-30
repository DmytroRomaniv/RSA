using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace RSA
{
    public class RsaAlgorythm
    {
        public BigInteger _nValue;
        public BigInteger _qValue;

        public RsaAlgorythm(BigInteger nValue, BigInteger qValue)
        {
            if (nValue <= 0 || !IsPrime(nValue))
            {
                _nValue = GenerateRandomPrimeInteger();
            }
            else
            {
                _nValue = nValue;
            }

            if (qValue <= 0 || !IsPrime(qValue))
            {
                _qValue = _nValue;

                while (_nValue == _qValue)
                {
                    _qValue = GenerateRandomPrimeInteger();
                }
            }
            else
            {
                _qValue = qValue;
            }
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

        private double GenerateRandomDouble(double value)
        {
            var random = new Random();
            var randomNumber = 0.0;

            if(value < int.MaxValue)
            {
                randomNumber = random.Next(2, (int)value);
                return randomNumber;
            }
            else
            {
                for(int i = 0; i< value / int.MaxValue; i++)
                {
                    randomNumber += random.Next(2, int.MaxValue);
                }
                var remainder = value % int.MaxValue;
                if (remainder > 2)
                {
                    randomNumber += random.Next(2, (int)remainder);
                }
                return randomNumber;
            }

        }
        private double CalculateModulo(double value, double moduloValue)
        {
            while(value > moduloValue)
            {
                value -= moduloValue;
            }

            return value;
        }
    }
}
