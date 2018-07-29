// Decompiled with JetBrains decompiler
// Type: AucScanner.Models.Auctions
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using System.Collections.Generic;

namespace AucScanner.Models
{
  public class Auctions
  {
    public List<Realm> realms { get; set; }

    public List<Auction> auctions { get; set; }
  }
}
