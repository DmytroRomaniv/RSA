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

        [Fact]
        public void EucledSmallNumbersTest()
        {
            var rsa = new RsaAlgorythm(0, 0);
            var a = new BigInteger(3);
            var b = new BigInteger(9167368);
            var eulerValue = rsa.CalculateEulerFunction(rsa._pValue, rsa._qValue);
            var eValue = rsa.GenerateCoprimeInteger(eulerValue);

            BigInteger y;
            var res = rsa.UseExtendedEuclid(eValue, eulerValue);

            Assert.Equal( eValue*res % eulerValue, 1);
        }

        [Fact]
        public void EncryptTest()
        {
            var rsa = new RsaAlgorythm(3557, 2579);
            var message = new byte[]
        }
    }
}
