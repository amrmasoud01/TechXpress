using techXpress.Services.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.ActionRequests
{
    public class CreateCategoryActionRequest
    {
        [MaxLength(50)]
        [Remote("CheckName","Category",ErrorMessage= "There is already a category with the same name.")]
        public string Name { get; set; }
    }

    public static class CreateCategoryActionRequestMapping
    {
        public static CategoryCreateDTO ToDto(this CreateCategoryActionRequest request)
        {
            return new CategoryCreateDTO { Name = request.Name };
        }
    }
}
