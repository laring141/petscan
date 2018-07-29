// Decompiled with JetBrains decompiler
// Type: AucScanner.Models.Auction
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

namespace AucScanner.Models
{
  public class Auction
  {
    public int auc { get; set; }

    public long item { get; set; }

    public string Owner { get; set; }

    public long Bid { get; set; }

    public long Buyout { get; set; }

    public int Quantity { get; set; }

    public int petSpeciesId { get; set; }

    public string TimeLeft { get; set; }

    public int petBreedId { get; set; }

    public int petLevel { get; set; }

    public int petQualityId { get; set; }
  }
}
