using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record GeneralUserDTO(int Id, string UserName, string Email,string? ProfilePicture);
}
