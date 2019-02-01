using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using WoWCharacterCodex.Application.Blizzard;
using WoWCharacterCodex.Data;
using Newtonsoft.Json.Linq;

namespace WoWCharacterCodex.Application
{
    public class BlizzardService
    {
        private CredentialService _credentials;
        private string _accessToken;
        private Credential _credential;
        private string credentialRequestURI = "https://us.battle.net/oauth/token";
        private static string _communityUrl = "https://us.api.blizzard.com/wow/";
        private static string _dataUrl = "https://us.api.blizzard.com/data/wow/";
        public BlizzardService(CredentialService credentials)
        {
            _credentials = credentials;
        }

        
        public Blizzard.Guild GetGuild(string guildName, string guildRealm, params string[] fields)
        {
            if (fields == null || fields.Count() == 0)
            {
                fields = new string[] { "members" };
            }
            else if (!fields.Contains("members"))
            {
                fields = fields.Union(new string[] { "members" }).ToArray();
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "fields", string.Join(",", fields) }
            };

            parameters = GetDefaultParameters(parameters);
            var responseBody = Get(_communityUrl + "guild/" + guildRealm + "/" + guildName,parameters);
            Blizzard.Guild guild = JsonConvert.DeserializeObject<Blizzard.Guild>(responseBody);
            return guild;
        }

        public List<Blizzard.WoWClass> GetBlizzardClasses()
        {
            var parameters = GetDefaultParameters();
            parameters["namespace"] = "static-us";
            var responseBody = Get(_dataUrl + "playable-class/index", parameters);
            var obj = JsonConvert.DeserializeObject<JObject>(responseBody)["classes"];
            List<Blizzard.WoWClass> classes = obj.ToObject<Blizzard.WoWClass[]>().ToList();
            foreach(Blizzard.WoWClass wowClass in classes)
            {
                
                responseBody = Get(_dataUrl + "/playable-class/" + wowClass.Id, parameters);
                List<int> specIds = JsonConvert.DeserializeObject<JObject>(responseBody)["specializations"].Select(j => (int)j["id"]).ToList();
                wowClass.Specializations = new Blizzard.Specialization[specIds.Count];
                foreach (int specId in specIds)
                {
                    responseBody = Get(_dataUrl + "/playable-specialization/" + specId, parameters);
                    Blizzard.Specialization spec = JsonConvert.DeserializeObject<Blizzard.Specialization>(responseBody);
                    wowClass.Specializations[specIds.FindIndex(s => s == specId)] = spec;
                }
            }
            return classes;
        }

        public Blizzard.Character GetCharacter(string name, string realm)
        {
            var parameters = GetDefaultParameters();
            parameters["fields"] = "progression";
            var responseBody = Get(_communityUrl + "character/" + realm + "/" + name, parameters);
            return JsonConvert.DeserializeObject<Blizzard.Character>(responseBody);

        }

        private Dictionary<string,string> GetDefaultParameters(Dictionary<string, string> defaultValues = null)
        {
            defaultValues = defaultValues ?? new Dictionary<string, string>();
            if(string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = GetAccessToken();
            }
            defaultValues.Add("locale", "en_US");
            defaultValues.Add("access_token", _accessToken);
            return defaultValues;
        }

        private string GetAccessToken()
        {
            string formData = "grant_type=client_credentials";
            byte[] formBytes = Encoding.UTF8.GetBytes(formData);

            _credential = _credentials.GetCredential();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(credentialRequestURI);
            request.Method = "POST";
            request.Headers.Add("Authorization", _credential.ClientID + " " + _credential.ClientSecret);

            request.ContentType = "multipart/form-data";
            request.ContentLength = formBytes.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(formBytes, 0, formBytes.Length);

            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseStream = new StreamReader(response.GetResponseStream());
                var jResponse = JsonConvert.DeserializeObject<JObject>(responseStream.ReadToEnd());
                if (jResponse.ContainsKey("access_token"))
                {
                    return (string)jResponse["access_token"];
                }
            }
            return GetAccessToken();
        }

        private string Get(string uri, Dictionary<string, string> parameters)
        {
            _credentials.LogRequest(_credential);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if((int)response.StatusCode == 429)
                {
                    _accessToken = GetAccessToken();
                    parameters["access_token"] = _accessToken;
                    return Get(uri, parameters);
                }
                else
                {
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}

