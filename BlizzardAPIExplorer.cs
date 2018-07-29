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
using System.Text;
using System.Threading;

namespace AucScanner
{
  public class BlizzardAPIExplorer
  {
    private string privateKey = "u8mz5ymmphznxvqw33eup9qr9s75pv5z";
    private string locale = LocalSettings.serverstype != ServersType.RU ? "en_EU" : "ru_RU";
    private string world = "eu";
    private string jSonDir = AppDomain.CurrentDomain.BaseDirectory + "Data";
    private int requestPerSecond = 20;
    private object lockThis = new object();
    private System.Windows.Forms.Timer blizzTimer;
    private int freeRequest;

    public BlizzardAPIExplorer(string key)
    {
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
      WebResponse response = WebRequest.Create(this.getURL("auction/data/" + serverName)).GetResponse();
      Files files;
      using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
      {
        using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) streamReader))
          files = new JsonSerializer().Deserialize<Files>((JsonReader) jsonTextReader);
      }
      response.Close();
      Console.WriteLine(serverName + " " + files.files[0].url);
      string url = files.files[0].url;
      string str1 = files.files[0].lastModified.ToString();
      Uri uri = new Uri(url);
      string str2 = this.jSonDir + uri.Segments[uri.Segments.Length - 2].TrimEnd('/') + "_" + str1 + ".json";
      if (!System.IO.File.Exists(str2) || new FileInfo(str2).Length < 100L)
      {
        MyWebClient myWebClient = new MyWebClient();
        myWebClient.Proxy = (IWebProxy) null;
        myWebClient.Encoding = Encoding.Unicode;
        myWebClient.DownloadFile(url, str2);
      }
      Console.WriteLine(serverName + " downloaded " + files.files[0].url);
      bool flag = false;
      using (StreamReader streamReader = new StreamReader(str2))
      {
        using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) streamReader))
        {
          JsonSerializer jsonSerializer = new JsonSerializer();
          try
          {
            return jsonSerializer.Deserialize<Auctions>((JsonReader) jsonTextReader);
          }
          catch (Exception ex)
          {
            flag = true;
          }
        }
      }
      if (!flag)
        return (Auctions) null;
      System.IO.File.Delete(str2);
      return (Auctions) null;
    }

    private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      lock (e.UserState)
        Monitor.Pulse(e.UserState);
    }

    private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
    }

    public List<Pet> GetPets()
    {
      this.getAPIToken();
          
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.getURL("pet/"));
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
      using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
      {
        using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) streamReader))
        {
          Pets pets = new JsonSerializer().Deserialize<Pets>((JsonReader) jsonTextReader);
          response.Close();
          return pets.pets;
        }
      }
    }

    private string getURL(string command)
    {
      return string.Format("https://{0}.api.battle.net/wow/{1}?locale={2}&apikey={3}", (object) this.world, (object) command, (object) this.locale, (object) this.privateKey);
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
