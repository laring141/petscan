// Decompiled with JetBrains decompiler
// Type: AucScanner.BlizzardAPIExplorer
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using AucScanner.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using RestSharp;
/*using System.Text.Json;
using System.Text.Json.Serialization;
using RestClient;*/

namespace AucScanner
{
    public class oauthToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public DateTime expiresTime { get; set; }
    }


    public class BlizzardAPIExplorer
    {
        private string privateKey = "u8mz5ymmphznxvqw33eup9qr9s75pv5z";
        private string locale = LocalSettings.serverstype != ServersType.RU ? "en_US" : "ru_RU";
        private string world = "eu";
        private string jSonDir = AppDomain.CurrentDomain.BaseDirectory + "Data";
        private int requestPerSecond = 20;
        private object lockThis = new object();
        private System.Windows.Forms.Timer blizzTimer;
        private int freeRequest;
        private oauthToken token;




        private RestClient authClient = new RestClient("https://eu.battle.net");

        public BlizzardAPIExplorer(string key)
        {

            authClient.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("42ec6c2554eb4235aaa2c4ccc249b3e7", "IlJ1A4sMYG3F9fBbJUr0GmhVIZJCOUbq");


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            this.freeRequest = this.requestPerSecond;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            this.jSonDir = LocalSettings.serverstype != ServersType.RU ? this.jSonDir + "_EU\\" : this.jSonDir + "_RU\\";
            if (!Directory.Exists(this.jSonDir))
                Directory.CreateDirectory(this.jSonDir);

            this.privateKey = key;
            this.deleteAllJsons();
            this.blizzTimer = new System.Windows.Forms.Timer();
            this.blizzTimer.Interval = 1000;
            this.blizzTimer.Tick += new EventHandler(this.BlizzTimer_Tick);
            this.blizzTimer.Start();

            this.authenticate();
        }

        private void BlizzTimer_Tick(object sender, EventArgs e)
        {
            lock (this.lockThis)
                this.freeRequest = this.requestPerSecond;
        }

        private void deleteAllJsons()
        {
            string[] files = Directory.GetFiles(this.jSonDir, "*.json");
            DateTime now = DateTime.Now;
            foreach (string str in files)
            {
                DateTime lastWriteTime = System.IO.File.GetLastWriteTime(str);
                if ((now - lastWriteTime).TotalHours > 2.0)
                    System.IO.File.Delete(str);
                else if (new FileInfo(str).Length < 100L)
                    System.IO.File.Delete(str);
            }
        }

        public Auctions GetAuctions(string serverName)
        {
            this.getAPIToken();
      
            //  return (Auctions)null;
            var url = "connected-realm/" + serverName + "/auctions";
            /* var baseUri = new Uri(this.getURL(url, false));
             var client = new Client(baseUri);
             access_token*/
             HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.getURL(url, false));
            //request
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", this.token.access_token));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
             {
                 using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader)streamReader))
                 {
                     Auctions auctions = new JsonSerializer().Deserialize<Auctions>((JsonReader)jsonTextReader);
                     response.Close();
                     return auctions;
                 }
             }

             return (Auctions)null;
        }

        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            lock (e.UserState)
                Monitor.Pulse(e.UserState);
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
        }

        public List<AucScanner.Models.Pet> GetPets()
        {
            this.getAPIToken();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.getURL("pet/index", true));
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", this.token.access_token));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader)streamReader))
                {
                    Pets pets = new JsonSerializer().Deserialize<Pets>((JsonReader)jsonTextReader);
                    response.Close();
                    return pets.pets;
                }
            }
        }

        public PetImage GetPetImage(int petId)
        {
            this.getAPIToken();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.getURL("pet/"+petId, true));
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", this.token.access_token));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader)streamReader))
                {
                    PetImage pet = new JsonSerializer().Deserialize<PetImage>((JsonReader)jsonTextReader);
                    response.Close();
                    return pet;
                }
            }
        }


        private string getURL(string command, bool isStatic)
        {
            string blizzNamespace = isStatic ? "static-eu" : "dynamic-eu";
            return string.Format("https://eu.api.blizzard.com/data/wow/{0}?locale={1}&namespace={2}", (object)command, this.locale, (object)blizzNamespace);
        }

        private void checkAuthentication()
        {
            if (this.token.expiresTime < DateTime.Now)
            {
                authenticate();
            }
        }


        private void authenticate()
        {
            var request = new RestRequest("/oauth/token", Method.POST);
            // request.AddJsonBody("{\"grant_type\":\"client_credentials\"}");
            request.AddQueryParameter("grant_type", "client_credentials");

            var response = authClient.Post<oauthToken>(request);
            if (response.IsSuccessful)
            {
                this.token = response.Data;
                this.token.expiresTime = DateTime.Now.AddSeconds(this.token.expires_in);
            }


        }

        private void getAPIToken()
        {
            for (bool flag = this.isGotAPIToken(); !flag; flag = this.isGotAPIToken())
                Thread.Sleep(1000);
        }

        private bool isGotAPIToken()
        {
            lock (this.lockThis)
            {
                if (this.freeRequest <= 0)
                    return false;
                --this.freeRequest;
                return true;
            }
        }
    }
}
