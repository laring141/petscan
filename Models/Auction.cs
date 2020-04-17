// Decompiled with JetBrains decompiler
// Type: AucScanner.Models.Auction
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe
using System;  
using System.Collections.Generic;

namespace AucScanner.Models
{
  public class Auction
  {
    public int Id { get; set; }

    public Dictionary<string, object> Item { get; set; }

    public long Buyout { get; set; }

    public int Quantity { get; set; }

    public String TimeLeft { get; set; }

    public int petQualityId 
    {
        get
        {
            
            return Convert.ToInt32(this.Item["pet_quality_id"]);
        }
    }
    
    public int petBreedId
    {
        get
        {
            
            return Convert.ToInt32(this.Item["pet_breed_id"]);
        }
    }

   public int petLevel
    {
        get
        {
            
            return Convert.ToInt32(this.Item["pet_level"]);
        }
    }

    public int petSpeciesId
    {
        get
        {
            
            return Convert.ToInt32(this.Item["pet_species_id"]);
        }
    }

    public bool isPet
    {
        get
        {
            long itemId = (Int64)this.Item["id"];
            return itemId == 82800;
        }
    }
  }
}
