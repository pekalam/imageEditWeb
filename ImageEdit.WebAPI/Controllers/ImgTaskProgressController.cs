using System;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageEdit.WebAPI.Controllers
{
    public class ImgTaskProgressController : AppControllerBase
    {
        private readonly ImgTaskProgressService _imgTaskProgressService;

        public ImgTaskProgressController(ImgTaskProgressService imgTaskProgressService)
        {
            _imgTaskProgressService = imgTaskProgressService;
        }

        [HttpGet("progress")]
        public ActionResult<ImgTaskProgressGroup> GetImgTaskProgress([FromQuery(Name = "gid")] Guid groupId)
        {
            var progress = _imgTaskProgressService.GetTaskProgressGroup(groupId);

            return Ok(progress);
        }
    }
}
