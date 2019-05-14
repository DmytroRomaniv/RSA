using System;
using System.Numerics;
using RSA;
using Xunit;

namespace RSA.Tests
{
    public class AlgorythmTest
    {
        //[Fact]
        //public void SmallNumbersTest()
        //{
        //    var rsa = new RsaAlgorythm(0, 0);
        //    var primeNumber = 127;
        //}
       

        [Fact]
        public void generatePrime()
        {
            var rsa = new RsaAlgorythm(2, 3);
            var prime = rsa.GenerateRandomPrimeInteger();

            Assert.True(rsa.FermatsIsPrime(40168526266853));
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
            
            var res = rsa.UseExtendedEuclid(eValue, eulerValue);

            Assert.Equal( eValue*res % eulerValue, 1);
        }

        [Fact]
        public void EncryptTest()
        {
            var rsa = new RsaAlgorythm(3557, 2579);
        }
    }
}
