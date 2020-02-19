using System;
using System.IO;
using System.Runtime.CompilerServices;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;

[assembly: InternalsVisibleTo("UnitTests")]
namespace ImageEdit.Core.Services
{
    public class ImgTaskProgressService
    {
        private readonly IImgTaskProgressRepository _imgTaskProgressRepository;

        internal ImgTaskProgressService(IImgTaskProgressRepository imgTaskProgressRepository)
        {
            _imgTaskProgressRepository = imgTaskProgressRepository;
        }

        public ImgTaskProgressGroup GetTaskProgressGroup(Guid groupId)
        {
            var tasks = _imgTaskProgressRepository.GetTaskProgressesByGroupId(groupId);

            return tasks == null ? null : new ImgTaskProgressGroup(groupId, tasks);
        }
    }
}