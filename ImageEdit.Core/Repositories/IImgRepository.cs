using System;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
[assembly: InternalsVisibleTo("IntegrationTests")]
[assembly: InternalsVisibleTo("E2ETests")]
namespace ImageEdit.Core.Repositories
{
    public interface IImgRepository
    {
        Guid AddImg(Stream img);
        Stream GetImg(Guid imgId);
    }
}