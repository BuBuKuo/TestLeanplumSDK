using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestLeanplumSDK.HTTP;
using System.Web.Script.Serialization;
using System.Reflection;


namespace TestLeanplumSDK.Leanplum
{
    class Leanplum_ABTesting
    {
        private HTTPMethod _HTTPMethod;
        private String _KeyKapId, _KeyProduction, _ApiVersion, _UserId;

        public Leanplum_ABTesting(String apiVersion)
        {
            _HTTPMethod = new HTTPMethod();
            _HTTPMethod.APIPath = "https://www.leanplum.com/api?";
            _ApiVersion = "apiVersion=" + apiVersion;
        }

        public void SetAppIdForProductionMode(String appId, String productionKey)
        {
            _KeyKapId = "appId=" + appId;
            _KeyProduction = "clientKey="+ productionKey;
        }

        public void SetUserid(string userId) 
        {
            _UserId = "userId=" + userId;
        }
       
        public object Start(Dictionary<string, object> optionalArguments = null)
        {
            string address = _KeyKapId + "&" + _KeyProduction + "&" + _ApiVersion + "&action=start" + "&" + _UserId;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string responseStr;

            if (optionalArguments == null)
            {
               responseStr = _HTTPMethod.HttpGet(address);
            }
            else 
            {
               responseStr = _HTTPMethod.HttpPost(address, optionalArguments);
            }

            Response response = serializer.Deserialize<Response>(responseStr);

            if (response.response[0].success != true) 
            {
                // Query failed
            }
            return response.response[0].vars;
        }

        public class Response
        {
            public List<ResponseData> response { get; set; }
        }

        public class ResponseData
        {
            public object vars { get; set; }
            public bool success { get; set; }
        }

        public void Track(string trackEvent, Dictionary<string, object> optionalArguments = null) 
        {
            String address = _KeyKapId + "&" + _KeyProduction + "&" + _ApiVersion + "&action=track" + "&" + _UserId + "&event=" + trackEvent;
            if (optionalArguments == null)
            {
                _HTTPMethod.HttpGet(address);
            }
            else
            {
                _HTTPMethod.HttpPost(address, optionalArguments);
            }
        }

        public void Stop()
        {
            string h = _HTTPMethod.HttpGet(_KeyKapId + "&" + _KeyProduction + "&" + _ApiVersion + "&action=stop" + "&" + _UserId);
        }
    }
}
