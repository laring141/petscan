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
  internal class PetImageProvider
  {
    private Dictionary<int, BackgroundWorker> workers = new Dictionary<int, BackgroundWorker>();
    private string folderName;
    private BlizzardAPIExplorer explorer;

    public PetImageProvider(BlizzardAPIExplorer explorer)
    {
            this.explorer = explorer;
            this.folderName = AppDomain.CurrentDomain.BaseDirectory + "tempImages";
      if (Directory.Exists(this.folderName))
        return;
      Directory.CreateDirectory(this.folderName);
     
        }

    public void fillDataCellWithImage(DataGridViewImageCell cell, int petId)
    {
      string imagePath = this.folderName + "\\" + petId + ".jpg";
      string externalPath = "https://render-eu.worldofwarcraft.com/icons/36/" + petId + ".jpg";
            if (System.IO.File.Exists(imagePath))
            {
                try
                {
                    cell.Value = (object)Image.FromFile(imagePath);
                }
                catch(Exception)
                {
                    File.Delete(imagePath);
                    return;
                }
            }
            else if (this.workers.ContainsKey(petId))
            {
                this.workers[petId].RunWorkerCompleted += (RunWorkerCompletedEventHandler)((sender, e) => cell.Value = (object)Image.FromFile(imagePath));
            }
            else
            {
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                this.workers[petId] = backgroundWorker;
                backgroundWorker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
                backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler)((sender, e) => cell.Value = (object)Image.FromFile(imagePath));

                List<object> arguments = new List<object>();
                arguments.Add(petId);
                arguments.Add(imagePath);
                backgroundWorker.RunWorkerAsync(arguments);
            }
    }

    void worker_DoWork(object sender, DoWorkEventArgs e)
	{
         List<object> arguments = (List<object>)e.Argument;
		 int petId = (int)arguments[0];
         string path = (string)arguments[1];
         
            AucScanner.Models.PetImage petImage = this.explorer.GetPetImage(petId);
         if (petImage != null)
         {
             var client = new WebClient();
             client.DownloadFile(petImage.Icon, path);
         }
 	}


    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      throw new NotImplementedException();
    }
  }
}
