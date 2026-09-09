using techXpress.Services.DTOs.CategoryDTOs;

namespace techXpress.UI.VMs.Category
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }

    public static class CategoryVMMapping
    {
        public static CategoryVM ToVM(this CategoryDTO categoryDTO)
        {
            return new CategoryVM
            {
                Name = categoryDTO.Name,
                CategoryId = categoryDTO.CategoryId
            };
        }
    }
   

}
