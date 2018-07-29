// Decompiled with JetBrains decompiler
// Type: AucScanner.Server
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using AucScanner.Models;

namespace AucScanner
{
  public class Server
  {
    public string apiName;
    public string russianName;
    public Auctions auctions;

    public Server(string api, string rus)
    {
      this.apiName = api;
      this.russianName = rus;
    }
  }
}
