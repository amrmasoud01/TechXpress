using techXpress.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.Services.DTOs.CategoryDTOs
{
    public class CategoryCreateDTO
    {
        public string Name { get; set; }
    }


    public static class CategoryCreateDTOMapping
    {
        public static Category ToCategory(this CategoryCreateDTO categoryCreateDTO)
        {
            return new Category { Name = categoryCreateDTO.Name };
        }

    }
}
