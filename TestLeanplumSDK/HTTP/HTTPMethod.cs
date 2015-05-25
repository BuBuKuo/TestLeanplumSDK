using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Collections.Specialized;

namespace TestLeanplumSDK.HTTP
{
    class HTTPMethod
    {
        private string _APIPath;

        public string APIPath
        {
            set
            {
                if (value.Length == 0) 
                {
                    throw new ArgumentException("fail");
                }
                _APIPath = value;
            }
            get { return _APIPath; }
        }

        public string HttpGet(string command)
        {
            return new WebClient().DownloadString( _APIPath + command);
        }

        public string HttpPost(string address, Dictionary<string, object> MegBody)
        {
            NameValueCollection data = new NameValueCollection();
            string responseString;

            foreach (var k in MegBody)
            {
                data.Add(k.Key.ToString(), k.Value.ToString());
            }

            using (var client = new WebClient())
            {
                responseString = Encoding.Default.GetString(client.UploadValues( _APIPath + address, data));
            }

            return responseString;
        }
    }
}
