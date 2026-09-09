using techXpress.Services.Abstraction;
using techXpress.Services.DTOs.CategoryDTOs;
using techXpress.DataAccess.Abstraction;
using techXpress.DataAccess.Entities;
using System.Threading.Tasks;

namespace techXpress.Services.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CategoryDTO> GetAll(params string[]? Includes)
        {
            IEnumerable<CategoryDTO> categories = _unitOfWork.CategoryRepository.GetAll(Includes)
                .Select(c => c.ToDTO());
            
            return categories;
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(c => c.CategoryId == id);
            if (category == null)
                return null;

            return category.ToDTO();
        }

        public async Task CreateCategoryAsync(CategoryCreateDTO categoryCreateDTO)
        {
            if(categoryCreateDTO == null)
                throw new ArgumentNullException(nameof(categoryCreateDTO));

            Category category = categoryCreateDTO.ToCategory();
            await _unitOfWork.CategoryRepository.CreateAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
                throw new ArgumentNullException(nameof(categoryDTO));

            _unitOfWork.CategoryRepository.Update(categoryDTO.ToCategory());
            await _unitOfWork.SaveAsync();
        }

        public CategoryDTO? GetByName(string name)
        {
            CategoryDTO? categoryDTO = _unitOfWork.CategoryRepository
                .GetAllWhere(c => c.Name == name)
                .FirstOrDefault()?.ToDTO();

            return categoryDTO;    
        }

        public async Task DeleteCategoryAsync(int id) 
        {
            Category? category = await _unitOfWork.CategoryRepository.GetByIdAsync(c => c.CategoryId == id);
            if(category != null)
            {
                _unitOfWork.CategoryRepository.Delete(category);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
