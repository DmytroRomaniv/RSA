using System;
using System.Numerics;
using RSA;
using Xunit;

namespace RSA.Tests
{
    public class AlgorythmTest
    {
        [Fact]
        public async void SmallNumbersTest()
        {
            var rsa = new RsaAlgorithm(3557, 2579);
            var message = "104101108108111104101108108111104101108108111104101108108111104101108108111104101108108111110097109101110097109101110097110097110097110097110110097097";
            var encrMessage = await rsa.Encrypt(message);
            var decMessage = await rsa.Decrypt(encrMessage);
            Assert.Contains(message, decMessage);
        }


        [Fact]
        public void generatePrime()
        {
            var rsa = new RsaAlgorithm(2, 3);
            var prime = rsa.GenerateRandomPrimeInteger(long.MaxValue);

            Assert.True(RsaAlgorithm.FermatsIsPrime(prime));
        }
        [Fact]
        public void CoprimeSmallNumbersTest()
        {
            var rsa = new RsaAlgorithm(0,0);

            Assert.True(rsa.AreItegersCoprime(7,10));
        }

        [Fact]
        public void CoprimeNumbersTest()
        {
            var rsa = new RsaAlgorithm(0,0);
            var euler = rsa.CalculateEulerFunction(rsa.PValue, rsa.QValue);
            
            Assert.True(rsa.AreItegersCoprime(rsa.GenerateCoprimeInteger(euler), euler));
        }

        [Fact]
        public void EucledSmallNumbersTest()
        {
            var rsa = new RsaAlgorithm(0, 0);
            var a = new BigInteger(3);
            var b = new BigInteger(9167368);
            var eulerValue = rsa.CalculateEulerFunction(rsa.PValue, rsa.QValue);
            var eValue = rsa.GenerateCoprimeInteger(eulerValue);
            
            var res = rsa.UseExtendedEuclid(eValue, eulerValue);

            Assert.Equal( 1, eValue*res % eulerValue);
        }

        [Fact]
        public async void EncryptTest()
        {
            var rsa = new RsaAlgorithm(3557, 2579);
            var byteMessage = "104101108108111104101108108111104101108108111104101108108111104101108108111104101108108111110097109101110097109101110097110097110097110097110110097097";
            var message = await rsa.Encrypt(byteMessage);
            var decMessage = await rsa.Decrypt(message);

            Assert.Contains(byteMessage, decMessage);
        }

        [Fact]
        public void ModuloMultiplication()
        {
            var rsa = new RsaAlgorithm(2, 3);
            var resul = RsaAlgorithm.ModularMultiplication(175, 235, 257);
            Assert.Equal(3, resul);
        }
    }
}
