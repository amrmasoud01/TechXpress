using techXpress.Services.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;
using techXpress.UI.VMs.Category;

namespace techXpress.UI.ActionRequests
{
    public class UpdateCategoryActionRequest
    {
        public int Id { get; set; }

        [Remote(action: "CheckName", controller: "Category", AdditionalFields = nameof(Id), ErrorMessage = "There is already a category with the same name.")]
        public string Name { get; set; }
    }


    public static class UpdateCategoryActionRequestMapping
    {
        public static CategoryDTO ToDto(this UpdateCategoryActionRequest request)
        {
            return new CategoryDTO 
            {
                Name = request.Name ,
                CategoryId = request.Id
            };
        }
        public static CategoryVM ToVM(this UpdateCategoryActionRequest request) 
        {
            return new CategoryVM { Name = request.Name };
        }

        public static UpdateCategoryActionRequest ToActionRequest(this CategoryDTO categoryDTO)
        {
            return new UpdateCategoryActionRequest 
            {
                Name = categoryDTO.Name,
                Id = categoryDTO.CategoryId
            };

        }
    }

}
