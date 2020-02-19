using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Autofac;
using FluentAssertions;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Persistence.Entities;
using ImageEdit.Core.Repositories.Abstr;
using Microsoft.Extensions.Configuration;
using TestUtils;
using Xunit;

namespace IntegrationTests
{
    public class ImgRepository_Tests
    {
        private readonly IImgRepository _imgRepository;
        private readonly IContainer _container;
        private readonly ImageEditAppContext _dbContext;


        public ImgRepository_Tests()
        {
            _container = CreateDIContainer();
            _dbContext = _container.Resolve<ImageEditAppContext>();
            _imgRepository = _container.Resolve<IImgRepository>();
        }

        private IContainer CreateDIContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register<IConfiguration>(context => TestSettings.Configuration);
            builder.RegisterModule<FakeCoreModule>();
            return builder.Build();
        }

        [Fact]
        public void GetImg_returns_valid_img()
        {
            var id = Guid.NewGuid();

            _dbContext.DbImg.Add(new DbImg()
            {
                ImgId = id,
                Extension = "png",
                Image = File.ReadAllBytes(@"img/0.png")
            });
            _dbContext.SaveChanges();


            var found = _imgRepository.GetImg(id);

            found.Extension.Should().Be("png");
            found.ImgId.Should().Be(id);
            Assert.True(TestHelpers.CompareStreams(found.ImageStream, File.OpenRead(@"img/0.png")));
        }

        [Fact]
        public void AddImg_adds_img_to_db()
        {
            var id = Guid.NewGuid();

            var img = new Img(id, File.OpenRead(@"img/0.png"), "png");


            _imgRepository.AddImg(img);

            _dbContext.DbImg.Local.Clear();
            var found = _dbContext.DbImg.Find(id);

            found.Should().BeEquivalentTo(img, opt => opt.Excluding(img1 => img1.ImageStream));
            Assert.True(TestHelpers.CompareStreams(new MemoryStream(found.Image), File.OpenRead(@"img/0.png")));
        }
    }
}
