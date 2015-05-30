using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TestLeanplumSDK.HTTP;
using System.Web.Script.Serialization;
using System.Reflection;

using System.Collections;
namespace TestLeanplumSDK.Leanplum
{
    class TrackData
    {
        private string _trackEvent;
        private Dictionary<string, object> _optionalArguments = null;

        public TrackData(string trackEvent) 
        {
            _trackEvent = trackEvent;
        }

        public string GetTrackEvent()
        {
            return _trackEvent;
        }

        public Dictionary<string, object> GetOptionalArguments() 
        {
            return _optionalArguments;
        }

        public void SetOptionalArg(string arg, object value )
        {
            _optionalArguments = new Dictionary<string,object>();
            _optionalArguments.Add(arg,value);
        }

        public void ChangeOptionalArg(string arg, object value) 
        {
            if (_optionalArguments == null) 
            {
                // Error message
                return;
            }
            _optionalArguments[arg] = value;
        }
    }

    class Leanplum_ABTesting
    {
        private static readonly Leanplum_ABTesting _instance = new Leanplum_ABTesting();
        private HTTPMethod _HTTPMethod;
        private String _KeyKapId, _KeyProduction, _ApiVersion, _UserId;
        private bool _isLogin = false;

        public static Leanplum_ABTesting Instance
        {
            get
            {
                return _instance;
            }
        }

        public Leanplum_ABTesting() 
        {
            _HTTPMethod = new HTTPMethod();
            _HTTPMethod.APIPath = "https://www.leanplum.com/api?";
        }

        public void SetApiVersion(String apiVersion)
        {
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

        public Dictionary<string, object> Start(Dictionary<string, object> optionalArguments = null)
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
            _isLogin = true;
            return response.response[0].vars;
        }

        public class Response
        {
            public List<ResponseData> response { get; set; }
        }

        public class ResponseData
        {
            public Dictionary<string, object> vars { get; set; }
            public bool success { get; set; }
        }

        public void Track(TrackData trackData)
        {
            if (_isLogin) 
            {
                // Error
                return;
            }
            String address = _KeyKapId + "&" + _KeyProduction + "&" + _ApiVersion + "&action=track" + "&" + _UserId + "&event=" + trackData.GetTrackEvent();
            
            if (trackData.GetOptionalArguments() == null)
            {
                _HTTPMethod.HttpGet(address);
            }
            else
            {
                _HTTPMethod.HttpPost(address, trackData.GetOptionalArguments());
            }
        }

        public void Stop()
        {
            string h = _HTTPMethod.HttpGet(_KeyKapId + "&" + _KeyProduction + "&" + _ApiVersion + "&action=stop" + "&" + _UserId);
            _isLogin = false;
        }
    }
}
