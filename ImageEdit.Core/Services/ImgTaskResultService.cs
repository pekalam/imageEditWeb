using System;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;

namespace ImageEdit.Core.Services
{
    public class ImgTaskResultService
    {
        private readonly IImgTaskResultRepository _imgTaskResultRepository;

        public ImgTaskResultService(IImgTaskResultRepository imgTaskResultRepository)
        {
            _imgTaskResultRepository = imgTaskResultRepository;
        }

        public ImgTaskResult GetTaskResult(Guid groupId, Guid taskId)
        {
            var result = _imgTaskResultRepository.GeTaskResult(groupId, taskId);
            return result;
        }
    }
}