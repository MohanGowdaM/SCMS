using System;
using System.Collections.Generic;
using System.Text;

namespace SmartClinic.Application
{
    public class JwtSettings
    {
        public string Key { get; set; }
            = string.Empty;

        public string Issuer { get; set; }
            = string.Empty;

        public string Audience { get; set; }
            = string.Empty;

        public int DurationInMinutes { get; set; }
    }
}
