using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public enum AuthType
    {
        none,
        ApiKey,
        Bearer,
        OAuth1,
        OAuth2
    }
}
