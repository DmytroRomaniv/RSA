using System;
using System.Numerics;
using RSA;
using Xunit;

namespace RSA.Tests
{
    public class AlgorythmTest
    {
        [Fact]
        public void SmallNumbersTest()
        {
            var rsa = new RsaAlgorythm(0, 0);
            var primeNumber = 127;

            Assert.True(rsa.IsPrime(rsa._pValue));
            Assert.True(rsa.IsPrime(rsa._qValue));
        }

        [Fact]
        public void CoprimeSmallNumbersTest()
        {
            var rsa = new RsaAlgorythm(0,0);

            Assert.True(rsa.AreItegersCoprime(7,10));
        }

        [Fact]
        public void CoprimeNumbersTest()
        {
            var rsa = new RsaAlgorythm(0,0);
            var euler = rsa.CalculateEulerFunction(rsa._pValue, rsa._qValue);
            
            Assert.True(rsa.AreItegersCoprime(rsa.GenerateCoprimeInteger(euler), euler));
        }
    }
}
