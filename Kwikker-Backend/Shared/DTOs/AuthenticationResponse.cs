using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record AuthenticationResponse
    {
        public int userId { get; set; }
        public TokenDto? token { get; set; }
    }
}
