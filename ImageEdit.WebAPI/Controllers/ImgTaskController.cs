using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ImageEdit.Core.Services;
using ImageEdit.WebAPI.Dto;
using ImageEdit.WebAPI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageEdit.WebAPI.Controllers
{
    public class ImgTaskController : AppControllerBase
    {
        private readonly ImgTaskService _imgTaskService;


        public ImgTaskController(ImgTaskService imgTaskService)
        {
            _imgTaskService = imgTaskService;
        }

        [HttpPost("convert")]
        public ActionResult<ImgTaskResponseDto> ConvertImage([FromForm][Required] IFormFile img, [ModelBinder(BinderType = typeof(JsonModelBinder))][Required]
            Dictionary<string, string> dict)
        {
            var creationParams = new ImgTaskCreationParams()
            {
                TaskName = "convert",
                TaskParams = dict
            };

            var progressGroupId = _imgTaskService.CreateEditTask(img.OpenReadStream(), img.GetFileExtension(), new []{creationParams});
            return Ok(new ImgTaskResponseDto(){ProgressGroupId = progressGroupId});
        }
    }
}