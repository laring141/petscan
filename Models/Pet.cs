// Decompiled with JetBrains decompiler
// Type: AucScanner.Models.Pet
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

namespace AucScanner.Models
{
  public class Pet
  {
    public int BattlePetId { get; set; }

    public bool CanBattle { get; set; }

    public long CreatureId { get; set; }

    public string CreatureName { get; set; }

    public string Icon { get; set; }

    public bool IsFavorite { get; set; }

    public string Name { get; set; }

    public int QualityId { get; set; }

    public long SpellId { get; set; }

    public PetStats Stats { get; set; }
  }
}
