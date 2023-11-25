using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatch.Services.Interfaces
{
    public interface IThresholdServices
    {
        public Task ExecuteAsync(CancellationToken stoppingToken);
    }
}

  
  