using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageEdit.WebAPI.Controllers
{
    public class ImageEditController : AppControllerBase
    {
        private readonly ImageEditTaskService _imageEditTaskService;

        public ImageEditController(ImageEditTaskService imageEditTaskService)
        {
            _imageEditTaskService = imageEditTaskService;
        }

        [HttpPost("edit")]
        public void ConvertImage([FromForm][Required] IFormFile img, [ModelBinder(BinderType = typeof(JsonModelBinder))][Required]
            Dictionary<string, string> dict)
        {
            _imageEditTaskService.CreateEditTask(img.OpenReadStream(), new[]
            {
                new EditTaskAction("convert", dict),
            });
        }
    }
}