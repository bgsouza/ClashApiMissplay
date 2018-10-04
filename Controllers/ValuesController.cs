using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Clash.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {

        private readonly string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6IjJhMjU0NjEzLTg5NjAtNDM0ZS04N2I1LTE2NWQ3NmVhNjgzMCIsImlhdCI6MTUzODQxODk5OSwic3ViIjoiZGV2ZWxvcGVyL2M1OTVjY2Q0LTA1NDktMmY0MC1lMTI0LTA4M2I1Mjc4NTU0YiIsInNjb3BlcyI6WyJyb3lhbGUiXSwibGltaXRzIjpbeyJ0aWVyIjoiZGV2ZWxvcGVyL3NpbHZlciIsInR5cGUiOiJ0aHJvdHRsaW5nIn0seyJjaWRycyI6WyIxNzcuMTI2LjE4MC44MyJdLCJ0eXBlIjoiY2xpZW50In1dfQ.sVW3w94DEDwNTbi6KYTHG0M24llWyEYlxJF5vGb8dLk4H0OflUGoFSRH_GGkifki1CPkzEv27SFEUz4bq3Uurw";
        private readonly string BaseUrl = "https://api.clashroyale.com/v1/";

        [HttpGet]
        [Route ("getLastWar")]
        public ItemsList GetLastWar () {
            string url = BaseUrl + "clans/%232G9L0VCC/warlog?limit=1";
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

            string url = BaseUrl + "clans/%232G9L0VCC/members";
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