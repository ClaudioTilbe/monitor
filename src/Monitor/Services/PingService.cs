using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public class PingService : IPingService
    {

        public async Task<bool> HacerPingAsync(string ip)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(ip, 1000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }



    }


}
