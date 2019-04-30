using System;
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

            Assert.True(rsa.IsPrimal(rsa._nValue));
            Assert.True(rsa.IsPrimal(rsa._qValue));
        }
    }
}
