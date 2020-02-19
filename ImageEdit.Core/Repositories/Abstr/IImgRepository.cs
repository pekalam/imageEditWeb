using System;
using System.IO;
using System.Runtime.CompilerServices;
using ImageEdit.Core.Domain;

[assembly: InternalsVisibleTo("UnitTests")]
[assembly: InternalsVisibleTo("IntegrationTests")]
[assembly: InternalsVisibleTo("E2ETests")]
namespace ImageEdit.Core.Repositories.Abstr
{
    public interface IImgRepository
    {
        Img AddImg(Img img);
        Img GetImg(Guid imgId);
    }
}