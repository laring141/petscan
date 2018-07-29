// Decompiled with JetBrains decompiler
// Type: AucScanner.Properties.Resources
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace AucScanner.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (AucScanner.Properties.Resources.resourceMan == null)
          AucScanner.Properties.Resources.resourceMan = new ResourceManager("AucScanner.Properties.Resources", typeof (AucScanner.Properties.Resources).Assembly);
        return AucScanner.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return AucScanner.Properties.Resources.resourceCulture;
      }
      set
      {
        AucScanner.Properties.Resources.resourceCulture = value;
      }
    }
  }
}
