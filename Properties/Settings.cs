// Decompiled with JetBrains decompiler
// Type: AucScanner.Properties.Settings
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AucScanner.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
    {
    }

    private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
    {
    }

    public static Settings Default
    {
      get
      {
        return Settings.defaultInstance;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    public StringCollection StoredPets
    {
      get
      {
        return (StringCollection) this[nameof (StoredPets)];
      }
      set
      {
        this[nameof (StoredPets)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("")]
    public string Servers
    {
      get
      {
        return (string) this[nameof (Servers)];
      }
      set
      {
        this[nameof (Servers)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool DefaultPetsAdded
    {
      get
      {
        return (bool) this[nameof (DefaultPetsAdded)];
      }
      set
      {
        this[nameof (DefaultPetsAdded)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public int DefaultPetsVersion
    {
      get
      {
        return (int) this[nameof (DefaultPetsVersion)];
      }
      set
      {
        this[nameof (DefaultPetsVersion)] = (object) value;
      }
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool AddedToFirewall
    {
      get
      {
        return (bool) this[nameof (AddedToFirewall)];
      }
      set
      {
        this[nameof (AddedToFirewall)] = (object) value;
      }
    }
  }
}
