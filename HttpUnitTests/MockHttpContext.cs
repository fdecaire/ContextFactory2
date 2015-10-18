using System.Collections.Specialized;
using System.Web;
using Moq;
using System.Web.SessionState;

namespace HttpUnitTests
{
	public class MockHttpContext
	{
		public NameValueCollection ServerVariables = new NameValueCollection();
		public HttpCookieCollection Cookies = new HttpCookieCollection();
		public NameValueCollection HeaderVariables = new NameValueCollection();
		public MockHttpSession SessionVars = new MockHttpSession();

		public HttpContextBase Context
		{
			get
			{
				var httpRequest = new Moq.Mock<HttpRequestBase>();

				httpRequest.Setup(x => x.ServerVariables.Get(It.IsAny<string>()))
					.Returns<string>(x =>
					{
						return ServerVariables[x];
					});

				httpRequest.SetupGet(x => x.Cookies).Returns(Cookies);

				httpRequest.Setup(x => x.Headers.Get(It.IsAny<string>()))
					.Returns<string>(x =>
						{
							return HeaderVariables[x];
						}
					);

				var httpContext = (new Moq.Mock<HttpContextBase>());
				httpContext.Setup(x => x.Request).Returns(httpRequest.Object);

				httpContext.Setup(ctx => ctx.Session).Returns(SessionVars);

				return httpContext.Object;
			}
		}

		public class MockHttpSession : HttpSessionStateBase
		{
			public SessionStateItemCollection SessionVariables = new SessionStateItemCollection();

			public override object this[string name]
			{
				get
				{
					return SessionVariables[name];
				}
				set
				{
					SessionVariables[name] = value;
				}
			}
		}
	}
}
