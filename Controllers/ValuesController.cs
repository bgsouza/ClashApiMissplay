using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.WebUtilities;

namespace Clash.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
        
        private readonly ClashProvider _clashProvider;
        protected string token;
        protected string BaseUrl;
        protected string code;

        public ValuesController(IOptions<ClashProvider> clashProvider)
	    {
	        _clashProvider = clashProvider.Value;
            token = _clashProvider.Token;
            BaseUrl = _clashProvider.Url;
            code = WebUtility.UrlEncode(_clashProvider.HashOfClan);
    	}

        [HttpGet]
        [Route ("getLastWar")]
        public ItemsList GetLastWar () {
            string url = BaseUrl + $"clans/{this.code}/warlog?limit=1";
            WebHeaderCollection headers = new WebHeaderCollection ();
            headers.Add ($"Authorization: Bearer {token}");
            HttpWebRequest getRequest = (HttpWebRequest) WebRequest.Create (url);
            getRequest.Method = "GET";
            getRequest.Headers = headers;
            WebResponse apiResponse = getRequest.GetResponse ();
            StreamReader reader = new StreamReader (apiResponse.GetResponseStream (), Encoding.UTF8);
            string responseString = reader.ReadToEnd ();
            reader.Close ();
            apiResponse.Close ();

            var responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ItemsList> (responseString);

            return responseJson;
        }

        [HttpGet]
        [Route ("getMembers")]
        public List<string> GetMembers () {
            List<string> membersList = new List<string> ();

            string url = BaseUrl + $"clans/{this.code}/members";
            WebHeaderCollection headers = new WebHeaderCollection ();
            headers.Add ($"Authorization: Bearer {token}");
            HttpWebRequest getRequest = (HttpWebRequest) WebRequest.Create (url);
            getRequest.Method = "GET";
            getRequest.Headers = headers;
            WebResponse apiResponse = getRequest.GetResponse ();
            StreamReader reader = new StreamReader (apiResponse.GetResponseStream (), Encoding.UTF8);
            string responseString = reader.ReadToEnd ();
            reader.Close ();
            apiResponse.Close ();

            var responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ItemsList> (responseString);

            foreach (var member in responseJson.Items) {
                membersList.Add (member.Name);
            }

            return membersList;
        }

        [HttpGet]
        [Route ("notParticipantsList")]
        public List<string> NotParticipantsList () {
            List<string> notParticipantsList = new List<string> ();
            var participants = GetLastWar ();
            var clanMembers = GetMembers ();

            /// Verifica quem não participou da última guerra
            foreach (var notParticipant in clanMembers.Where (x => x != (participants.Items[0].Participants.FirstOrDefault (y => y.Name == x) == null ?
                    "" :
                    participants.Items[0].Participants.First (y => y.Name == x).Name))) {
                notParticipantsList.Add(notParticipant);
            }
            return notParticipantsList;
        }

        [HttpGet]
        [Route ("zeroBattlesPlayed")]
        public List<string> ZeroBattlesPlayed () {
            List<string> zeroBattlesPlayedList = new List<string> ();
            var participants = GetLastWar ();
            var clanMembers = GetMembers ();

            /// verifica quem participou da última guerra e não atacou
            foreach (var participant in clanMembers.Where (x => x == (participants.Items[0].Participants.FirstOrDefault (y => y.Name == x && y.WarBattlesPlayed == 0) == null ?
                    "" :
                    participants.Items[0].Participants.First (y => y.Name == x && y.WarBattlesPlayed == 0).Name))) {
                var warParticipant = participant;
                zeroBattlesPlayedList.Add(warParticipant);
            }
            
            return zeroBattlesPlayedList;
        }
    }
}
