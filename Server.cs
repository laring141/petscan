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
    public int realmId;
    public string serverName;
    public Auctions auctions;

    public string apiName
    {
        get
        {
             return this.realmId.ToString();
        }
    }           

    public Server(int realmId, string rus)
    {
      this.realmId = realmId;
      this.serverName = rus;
    }
  }
}
