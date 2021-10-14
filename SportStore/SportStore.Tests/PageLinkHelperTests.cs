using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportStore.Infrastructure;
using Xunit;

namespace SportStore.Tests
{
    public class PageLinkHelperTests
    {
        [Fact]
        public void Can_Generate_Page_Links()
        {
            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory.Setup(f =>
                    f.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(urlHelper.Object);


            var helper = new PageLinkHelper(urlHelperFactory.Object)
            {
                PageModel = new()
                {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPage = 10,
                },
                PageAction = "Test"
            };

            var context = new TagHelperContext(new TagHelperAttributeList(), 
                new Dictionary<object, object>(), "");

            var content = new Mock<TagHelperContent>();
            var output = new TagHelperOutput("div", new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(content.Object));
            
            helper.Process(context, output);
            var expected = @"<a href=""Test/Page1"">1</a>"
                           + @"<a href=""Test/Page2"">2</a>"
                           + @"<a href=""Test/Page3"">3</a>";
            var actual = output.Content.GetContent();
            Assert.Equal(expected, actual);
        }
    }
}