﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin;
using Moq;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin
{
    [TestFixture]
    public class SessionTest
    {
        [Test]
        public void Session_Name_IsSession()
        {
            var name = Plugin.Name;

            Assert.AreEqual("Session", name);
        }

        [Test]
        public void Session_HelpUrl_IsRight()
        {
            var helper = Plugin as IProvideGlimpseHelp;

            Assert.AreEqual("http://getGlimpse.com/Help/Plugin/Session", helper.HelpUrl);
        }

        [Test]
        public void Session_SetupInit_DoesNothing()
        {
            Plugin.SetupInit();
        }

        [Test]
        public void Session_GetData_WithNullSession_ReturnsNull()
        {
            HttpSessionStateBase session = null;
            Context.Setup(ctx => ctx.Session).Returns(session);
            var data = Plugin.GetData(Context.Object);

            Assert.IsNull(data);
            Context.VerifyAll();
        }

        [Test]
        public void Session_GetData_WithEmptySession_ReturnsNull()
        {
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s.Keys).Returns(new NameValueCollection().Keys);

            Context.Setup(ctx => ctx.Session).Returns(session.Object);
            var data = Plugin.GetData(Context.Object);

            Assert.IsNull(data);
            Context.VerifyAll();
        }

        [Test]
        public void Session_GetData_ReturnsData()
        {
            Mock<HttpSessionStateBase> session = CreateSession(new NameValueCollection { { "test", "value" } });
            Context.Setup(ctx => ctx.Session).Returns(session.Object);

            var data = Plugin.GetData(Context.Object);

            var expected = new List<object[]>
                { 
                    Session.Header,
                    new object[] { "test", "value", "System.String", 29 } ,
                    new object[] { "Total Session Size", "", "(calculated)", 29 },
                };

            Assert.AreEqual(expected, data);
            Context.VerifyAll();
        }

        [Test]
        public void Session_GetData_ToleratesNullValues()
        {
            Mock<HttpSessionStateBase> session = CreateSession(new NameValueCollection { { "test", null } });
            Context.Setup(ctx => ctx.Session).Returns(session.Object);

            var data = Plugin.GetData(Context.Object);

            var expected = new List<object[]>
                { 
                    Session.Header,
                    new object[] { "test", null, null, 0 } ,
                    new object[] { "Total Session Size", "", "(calculated)", 0 },
                };

            Assert.AreEqual(expected, data);
            Context.VerifyAll();
        }

        private static Mock<HttpSessionStateBase> CreateSession(NameValueCollection values)
        {
            var session = new Mock<HttpSessionStateBase>();
            session.Setup(s => s.Keys).Returns(values.Keys);
            foreach (string key in values.Keys)
            {
                string captureKey = key;
                session.Setup(s => s[captureKey]).Returns(values[captureKey]);
            }
            return session;
        }

        public IGlimpsePlugin Plugin { get; set; }
        public Mock<HttpContextBase> Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Plugin = new Session();
            Context = new Mock<HttpContextBase>();
        }
    }
}
