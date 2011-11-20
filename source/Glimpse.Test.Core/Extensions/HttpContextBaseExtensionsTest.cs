using System.Web.Configuration;
using System.Xml;
using Glimpse.Core;
using Glimpse.Core.Extensions;
using System.Web;
using Moq;
using NUnit.Framework;

namespace Glimpse.Test.Core.Extensions
{
    [TestFixture]
    public class HttpContextBaseExtensionsTest
    {
        [Test]
        public void ResourceRoot_NoHandlers_DefaultsPath()
        {
            var rootPath = HttpContextBaseExtensions.GetResourceRootFromHandler(EmptyHandlers());

            Assert.AreEqual("~/", rootPath);
        }

        [Test]
        public void ResourceRoot_BasePath()
        {
            var handler = CreateHandler(typeof(Handler).FullName, "Glimpse.axd");
            var rootPath= HttpContextBaseExtensions.GetResourceRootFromHandler(handler);

            Assert.AreEqual("~/", rootPath);
        }

        [Test]
        public void ResourceRoot_WithSubDirectory()
        {
            var handler = CreateHandler(typeof(Handler).FullName, "MyDirectory/Glimpse.axd");
            var rootPath = HttpContextBaseExtensions.GetResourceRootFromHandler(handler);

            Assert.AreEqual("~/MyDirectory/", rootPath);
        }

        [Test]
        public void ResourceRoot_WithPrecedingSlash()
        {
            System.Console.WriteLine(typeof(Handler).FullName);
            var handler = CreateHandler(typeof(Handler).FullName, "/Glimpse.axd");
            var rootPath = HttpContextBaseExtensions.GetResourceRootFromHandler(handler);

            Assert.AreEqual("~/", rootPath);
        }

        [Test]
        public void ResourceRoot_WithAssemblyQualifiedName()
        {
            var handler = CreateHandler(typeof(Handler).AssemblyQualifiedName, "/Elsewhere/Glimpse.axd");
            var rootPath = HttpContextBaseExtensions.GetResourceRootFromHandler(handler);

            Assert.AreEqual("~/Elsewhere/", rootPath);
        }

        private static XmlNodeList EmptyHandlers()
        {
            var doc = new XmlDocument();
            doc.LoadXml("<wontfind />");
            return doc.SelectNodes("//add");
        }

        public XmlNodeList CreateHandler(string type, string path)
        {
            var doc = new XmlDocument();
            doc.LoadXml(string.Format("<add type=\"{0}\" path=\"{1}\" />", type, path));
            return doc.SelectNodes("//add");
        }
    }
}
