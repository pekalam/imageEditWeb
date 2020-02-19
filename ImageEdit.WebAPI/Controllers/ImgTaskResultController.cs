using System;
using ImageEdit.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageEdit.WebAPI.Controllers
{
    public class ImgTaskResultController : AppControllerBase
    {
        private readonly ImgTaskResultService _imgTaskResultService;

        public ImgTaskResultController(ImgTaskResultService imgTaskResultService)
        {
            _imgTaskResultService = imgTaskResultService;
        }

        [HttpGet("results")]
        public FileResult GetImgTaskResult([FromQuery(Name = "gid")] Guid groupId, [FromQuery(Name = "id")] Guid taskId)
        {
            var result = _imgTaskResultService.GetTaskResult(groupId, taskId);

            //TODO
            return File(result.ImgStream, $"image/{result.Extension}", $"{result.TaskId}.{result.Extension}");
        }
    }
}