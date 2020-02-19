using System;
using ImageEdit.Core.Domain;
using Xunit;

namespace UnitTests
{
    public class ImgTaskResult_Tests
    {
        [Fact]
        public void Ctor_when_img_is_null_and_extensions_is_set_throws()
        {
            Assert.Throws<ArgumentException>(() => (new ImgTaskResult(Guid.NewGuid(), Guid.NewGuid(), null, "png")));
        }
    }
}