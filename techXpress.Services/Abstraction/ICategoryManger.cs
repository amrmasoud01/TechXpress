using techXpress.Services.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace techXpress.Services.Abstraction
{
    public interface ICategoryManager
    {
        IEnumerable<CategoryDTO> GetAll(params string[]? Includes);
        Task<CategoryDTO?> GetByIdAsync(int id);
        Task CreateCategoryAsync(CategoryCreateDTO categoryCreateDTO);
        Task UpdateCategoryAsync(CategoryDTO categoryDTO);
        Task DeleteCategoryAsync(int id);
        CategoryDTO? GetByName(string name);
    }
}
