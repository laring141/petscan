// Decompiled with JetBrains decompiler
// Type: AucScanner.LocalSettings
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AucScanner
{
  internal class LocalSettings
  {
    private static string dataDir = AppDomain.CurrentDomain.BaseDirectory + "Data";
    public static ServersType serverstype = ServersType.EU;
    internal static StoredSettings settings;
    private static string fileName;

    internal static void Load()
    {
      string dataDir = LocalSettings.dataDir;
      string str1 = "settings";
      string str2 = LocalSettings.serverstype != ServersType.RU ? dataDir + "_EU\\" : dataDir + "_RU\\";
      string str3;
      LocalSettings.fileName = str3 = str2 + str1;
      if (File.Exists(LocalSettings.fileName))
      {
        LocalSettings.settings = JsonConvert.DeserializeObject<StoredSettings>(File.ReadAllText(LocalSettings.fileName));
      }
      else
      {
        string path = LocalSettings.dataDir + "\\" + str1;
        if (File.Exists(path))
        {
          LocalSettings.settings = JsonConvert.DeserializeObject<StoredSettings>(File.ReadAllText(path));
          LocalSettings.Save();
        }
        else
        {
          LocalSettings.settings = new StoredSettings();
          LocalSettings.settings.StoredPets = new Dictionary<long, long>();
          LocalSettings.settings.TimeToUpdateMin = 5;
          LocalSettings.Save();
        }
      }
    }

    internal static void Save()
    {
      string contents = JsonConvert.SerializeObject((object) LocalSettings.settings, Formatting.Indented);
      File.WriteAllText(LocalSettings.fileName, contents);
    }
  }
}
