using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.WebUI.Tests.Mocks
{
   public class MockHttpContext : HttpContextBase
    {
        //Lastly
        private MockRequest request;
        private MockReponse response;
        private HttpCookieCollection cookies;

        public MockHttpContext()
        {
            cookies = new HttpCookieCollection();
            this.request = new MockRequest(cookies);
            this.response = new MockReponse(cookies);
        }

        public override HttpRequestBase Request
        {
            get {
                return request;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return response;
            }
        }
    }

    //Request and response mehtods to look for cookies
    // Create Mock Response
    public class MockReponse : HttpResponseBase
    {
        // Underline cookies collection
        private readonly HttpCookieCollection cookies;

        public MockReponse(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        /// <summary>
        ///Overriding the base class to use our own cookies to test against
        /// </summary>
        public override HttpCookieCollection Cookies
        {
            get
            {
                return cookies;
            }
        }
    }


    // Create Mock request
    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection cookies;

        public MockRequest(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return cookies;
            }
        }
    }
}
