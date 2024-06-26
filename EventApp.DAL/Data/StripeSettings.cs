using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.DAL.Data
{
    public class StripeSettings
    {
        public string? SecretKey { get; set; }
        public string? PublicKey { get; set; }
    }
}
