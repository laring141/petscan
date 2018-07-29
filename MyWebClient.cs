// Decompiled with JetBrains decompiler
// Type: AucScanner.MyWebClient
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using System;
using System.Net;

namespace AucScanner
{
  public class MyWebClient : WebClient
  {
    protected override WebRequest GetWebRequest(Uri uri)
    {
      WebRequest webRequest = base.GetWebRequest(uri);
      webRequest.Timeout = 20000;
      return webRequest;
    }
  }
}
