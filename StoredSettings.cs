﻿// Decompiled with JetBrains decompiler
// Type: AucScanner.StoredSettings
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using System.Collections.Generic;

namespace AucScanner
{
  public class StoredSettings
  {
    public Dictionary<long, long> StoredPets { get; set; }

    public int DefaultPetsVersion { get; set; }

    public int TimeToUpdateMin { get; set; }
  }
}
