using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace RSA
{
    public class RsaAlgorythm
    {
        public double _nValue;
        public double _qValue;

        public RsaAlgorythm(double nValue, double qValue)
        {
            if (nValue <= 0 || !IsPrimal(nValue))
            {
                _nValue = GenerateRandomPrimalDouble();
            }
            else
            {
                _nValue = nValue;
            }

            if (qValue <= 0 || !IsPrimal(qValue))
            {
                _qValue = _nValue;

                while (_nValue == _qValue)
                {
                    _qValue = GenerateRandomPrimalDouble();
                }
            }
            else
            {
                _qValue = qValue;
            }

            double n = _qValue * _nValue;
        }

        public bool IsPrimal(double value, int numberOfTests = 512)
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
                range = Math.Floor(range / 2);
            }
            for (var i = 0; i < numberOfTests; i++)
            {
                var randomNumber = GenerateRandomDouble(value - 1);
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

        

        public double ModularMultiplication(double value, double powerValue, double modularValue)
        {
            var result = 0.0;
            var binaryPowerValue = ConvertToBinary(powerValue).Reverse();
            var firstValue = 1.0;
            var lastValue = value;


            foreach (var binaryNumber in binaryPowerValue)
            {
                if (binaryNumber == 1)
                {
                    firstValue = (firstValue * lastValue) % modularValue;
                }
                lastValue = Math.Pow(firstValue, 2) % modularValue;

            }

            result = firstValue;

            return result;
        }

        public IEnumerable<byte> ConvertToBinary(double value)
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

        private double GenerateRandomPrimalDouble()
        {
            var result = 4.0;
            while (!IsPrimal(result))
            {
                result = GenerateRandomDouble((double)int.MaxValue + int.MaxValue);
            }

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
