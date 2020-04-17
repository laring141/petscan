// Decompiled with JetBrains decompiler
// Type: AucScanner.ServersUpdate
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using AucScanner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AucScanner
{
  internal class ServersUpdate
  {
    private Dictionary<string, BackgroundWorker> workers = new Dictionary<string, BackgroundWorker>();
    private object lockThis = new object();
    public Server[] servers;
    private int[] updateStatus;
    private BlizzardAPIExplorer explorer;
    private ServerUpdateCompleteDelegate refreshDelegate;
    private ServerUpdateStartedDelegate startedDelegate;

    public ServersUpdate(BlizzardAPIExplorer explorer)
    {
      this.explorer = explorer;
      this.fillServers();
    }

    private void fillServers()
    {
      List<Server> source = new List<Server>();
      if (LocalSettings.serverstype == ServersType.RU)
      {
        source.Add(new Server(1924, "Пиратская бухта"));
        source.Add(new Server(1925, "Вечная песня"));
        source.Add(new Server(1602, "Гордунни"));
        source.Add(new Server(1922, "Азурегос"));
        source.Add(new Server(1603, "Король лич"));
        source.Add(new Server(1604, "Свежеватель"));
        source.Add(new Server(1923, "Ясеневый лес"));
        source.Add(new Server(1927, "Гром"));
        source.Add(new Server(1605, "Страж смерти"));
        source.Add(new Server(1929, "Черный шрам"));
        source.Add(new Server(1625, "Борейская тундра"));
        source.Add(new Server(1614, "Галакронд"));
        source.Add(new Server(1623, "Дракономор"));
        source.Add(new Server(1609, "Подземье"));
        source.Add(new Server(1615, "Ревущий Фьорд"));
        source.Add(new Server(1928, "Голдринн"));
       }
      else
      {
        source.Add(new Server(3657, "Bladefist"));
                source.Add(new Server(1080, "Bloodhoof"));
                source.Add(new Server(1081, "Bronzebeard"));
                source.Add(new Server(3713, "Burning legion"));
                source.Add(new Server(1403, "Draenor"));
                source.Add(new Server(2074, "Emerald dream"));
        source.Add(new Server(1416, "Eonar"));
       
        source.Add(new Server(3682, "Ragnaros"));
        source.Add(new Server(1329, "Ravencrest"));
                source.Add(new Server(3391, "Silvermoon"));
                source.Add(new Server(2073, "Stormscale"));
        source.Add(new Server(3687, "Sylvanas"));
                source.Add(new Server(1084, "Tarren Mill"));
                source.Add(new Server(3674, "Twisting nether"));
        source.Add(new Server(1301, "Outland"));
        

        // DE
        source.Add(new Server(1104, "Anetheron"));
                source.Add(new Server(3686, "Antonidas"));
                source.Add(new Server(581, "Blackrock"));
                source.Add(new Server(1121, "Das-syndikat"));
        source.Add(new Server(3692, "Eredar"));
        source.Add(new Server(3703, "Frostwolf"));
        source.Add(new Server(531, "Onyxia"));
        source.Add(new Server(1407, "Teldrassil"));
        source.Add(new Server(3679, "Aegwynn"));
        source.Add(new Server(3691, "Blackhand"));
        source.Add(new Server(580, "Blackmoore"));
                source.Add(new Server(1621, "Dalaran"));
                source.Add(new Server(1305, "Kazzak"));
                source.Add(new Server(3702, "Argent Dawn"));
                source.Add(new Server(516, "Forscherliga"));
                // FR
                source.Add(new Server(1086, "Conceil 'des Ombres"));
                source.Add(new Server(1390, "Hyjal"));
            }
      this.servers = source.ToArray<Server>();
    }

    public void refreshData(ServerUpdateStartedDelegate startedDelegate, ServerUpdateCompleteDelegate refreshDelegate)
    {
      this.refreshDelegate = refreshDelegate;
      this.startedDelegate = startedDelegate;
      for (int index = 0; index < this.servers.Length; ++index)
      {
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.Disposed += new EventHandler(this.Worker_Disposed);
        backgroundWorker.DoWork += new DoWorkEventHandler(this.Worker_DoWork);
        if (!this.workers.ContainsKey(this.servers[index].apiName))
          this.workers.Add(this.servers[index].apiName, backgroundWorker);
        else
          this.workers[this.servers[index].apiName] = backgroundWorker;
        backgroundWorker.RunWorkerAsync((object) index);
      }
    }

    private void Worker_Disposed(object sender, EventArgs e)
    {
      Console.Write("Disposed");
    }

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Server server = this.servers[(int) e.Argument];
      bool flag = false;
      while (!flag)
      {
        Console.WriteLine("Starting " + server.apiName);
        this.startedDelegate(server);
       
          Auctions auctions = this.explorer.GetAuctions(server.apiName);
          auctions.filterData();
          if (auctions != null)
          {
            server.auctions = auctions;
            this.refreshDelegate(server);
            flag = true;
          }
          else
          {
            Console.WriteLine("parsing failed " + server.apiName);
            flag = false;
          }
       

      }
      Console.WriteLine("completed " + server.apiName);
    }

    private Server getNextServerToUpdate()
    {
      lock (this.lockThis)
      {
        int index1 = -1;
        for (int index2 = 0; index2 < this.servers.Length; ++index2)
        {
          if (this.updateStatus[index2] == 0)
          {
            index1 = index2;
            break;
          }
        }
        if (index1 < 0)
          return (Server) null;
        Server server = this.servers[index1];
        this.setServerStatus(server, 1);
        return server;
      }
    }

    private void setServerStatus(Server server, int status)
    {
      int index1 = 0;
      for (int index2 = 0; index2 < this.servers.Length; ++index2)
      {
        if (this.servers[index2] == server)
        {
          index1 = index2;
          break;
        }
      }
      lock (this.lockThis)
        this.updateStatus[index1] = status;
    }
  }
}
