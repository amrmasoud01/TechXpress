using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.Services.Abstraction
{
    public  interface IFilesService
    {
        void Upload(string Destination, IFormFile file);
    }
}
