// Decompiled with JetBrains decompiler
// Type: AucScanner.PetImage
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace AucScanner
{
  internal class PetImage
  {
    private Dictionary<string, BackgroundWorker> workers = new Dictionary<string, BackgroundWorker>();
    private string folderName;

    public PetImage()
    {
      this.folderName = AppDomain.CurrentDomain.BaseDirectory + "tempImages";
      if (Directory.Exists(this.folderName))
        return;
      Directory.CreateDirectory(this.folderName);
    }

    public void fillDataCellWithImage(DataGridViewImageCell cell, string imageName)
    {
      if (string.IsNullOrEmpty(imageName))
        return;
      string imagePath = this.folderName + "\\" + imageName + ".jpg";
      string externalPath = "https://render-eu.worldofwarcraft.com/icons/36/" + imageName + ".jpg";
            if (System.IO.File.Exists(imagePath))
            {
                try
                {
                    cell.Value = (object)Image.FromFile(imagePath);
                }
                catch(Exception ex)
                {
                    File.Delete(imagePath);
                    return;
                }
            }
            else if (this.workers.ContainsKey(imageName))
            {
                this.workers[imageName].RunWorkerCompleted += (RunWorkerCompletedEventHandler)((sender, e) => cell.Value = (object)Image.FromFile(imagePath));
            }
            else
            {
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                this.workers[imageName] = backgroundWorker;
                backgroundWorker.DoWork += (DoWorkEventHandler)((sender, e) => new WebClient()
                {
                    Proxy = ((IWebProxy)null)
                }.DownloadFile(externalPath, imagePath));
                backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler)((sender, e) => cell.Value = (object)Image.FromFile(imagePath));
                backgroundWorker.RunWorkerAsync();
            }
    }

    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      throw new NotImplementedException();
    }
  }
}
