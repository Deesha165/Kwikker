using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ExceptionModels
{
    public class RefreshTokenExpiredException:Exception
    {
        public RefreshTokenExpiredException() : base("Refresh token is expired. Please log in again.") { }
    }
}
