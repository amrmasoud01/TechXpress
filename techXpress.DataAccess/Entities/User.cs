using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.DataAccess.Entities
{
    public class User : IdentityUser<Guid>
    {
        public List<Order>  Orders { get; set; }
        public List<Review> Reviews { get; set; }   
    }
}
