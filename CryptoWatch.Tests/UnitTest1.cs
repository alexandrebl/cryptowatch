using System.Collections.Generic;

namespace CryptoWatch.Tests
{
    public class UnitTest1
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastPrice">ultimo preço da Binance</param>
        /// <param name="currentPrice">ultimo preço do cache</param>
        /// <param name="result">taxa de variação</param>
        /// <param name="notifiedExpected">indica se notificou ou nao</param>
        /// <param name="isUpOrDownExpected">true -> notifica positivo, false -> notifica negativo, null -> nao faz nada</param>
        [Theory]
        [InlineData(100, 120, 20, true, true)]
        [InlineData(100, 80, -20, true, false)]
        [InlineData(100, 99.008, -0.992, false, false)]
        [InlineData(100, 100.008, 0.008, false, true)]
        public void GetUpDwTests(decimal lastPrice, decimal currentPrice, decimal result, bool notifiedExpected, bool isUpOrDownExpected)        
        {
            var taxaVariacao = (currentPrice / lastPrice - 1) * 100;

            Assert.Equal(result, taxaVariacao);

            var notified = (Math.Abs(taxaVariacao) >= 1);

            Assert.Equal(notifiedExpected, notified);

            var isUpOrDownResult = taxaVariacao >= 0;

            Assert.Equal(isUpOrDownExpected, isUpOrDownResult);
        }
    }
}