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
        source.Add(new Server("booty-bay", "Пиратская бухта"));
        source.Add(new Server("eversong", "Вечная песня"));
        source.Add(new Server("gordunni", "Гордунни"));
        source.Add(new Server("azuregos", "Азурегос"));
        source.Add(new Server("lich-king", "Король лич"));
        source.Add(new Server("soulflayer", "Свежеватель"));
        source.Add(new Server("ashenvale", "Ясеневый лес"));
        source.Add(new Server("grom", "Гром"));
        source.Add(new Server("deathguard", "Страж смерти"));
        source.Add(new Server("blackscar", "Черный шрам"));
        source.Add(new Server("borean-tundra", "Борейская тундра"));
        source.Add(new Server("galakrond", "Галакронд"));
        source.Add(new Server("fordragon", "Дракономор"));
        source.Add(new Server("deephome", "Подземье"));
        source.Add(new Server("howling-fjord", "Ревущий Фьорд"));
        source.Add(new Server("goldrinn", "Голдринн"));
       }
      else
      {
        source.Add(new Server("agamaggan", "Agamaggan"));
        source.Add(new Server("auchindoun", "Auchindoun"));
        source.Add(new Server("bladefist", "Bladefist"));
        source.Add(new Server("bronzebeard", "Bronzebeard"));
        source.Add(new Server("emerald-dream", "Emerald dream"));
        source.Add(new Server("eonar", "Eonar"));
        source.Add(new Server("executus", "Executus"));
        source.Add(new Server("trollbane", "Trollbane"));
        source.Add(new Server("burning-legion", "Burning legion"));
        source.Add(new Server("ragnaros", "Ragnaros"));
        source.Add(new Server("ravencrest", "Ravencrest"));
        source.Add(new Server("stormscale", "Stormscale"));
        source.Add(new Server("sylvanas", "Sylvanas"));
        source.Add(new Server("twisting-nether", "Twisting nether"));
        source.Add(new Server("outland", "Outland"));
        source.Add(new Server("silvermoon", "Silvermoon"));
        source.Add(new Server("anetheron", "Anetheron"));
        source.Add(new Server("das-syndikat", "Das-syndikat"));
        source.Add(new Server("eredar", "Eredar"));
        source.Add(new Server("frostwolf", "Frostwolf"));
        source.Add(new Server("madmortem", "Madmortem"));
        source.Add(new Server("onyxia", "Onyxia"));
        source.Add(new Server("teldrassil", "Teldrassil"));
        source.Add(new Server("aegwynn", "Aegwynn"));
        source.Add(new Server("blackhand", "Blackhand"));
        source.Add(new Server("garrosh", "Garrosh"));
        source.Add(new Server("blackmoore", "Blackmoore"));
                source.Add(new Server("dalaran", "Dalaran"));
                source.Add(new Server("kazzak", "Kazzak"));
                source.Add(new Server("argent-dawn", "Argent Dawn"));
                source.Add(new Server("forscherliga", "Forscherliga"));
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
        try
        {
          Auctions auctions = this.explorer.GetAuctions(server.apiName);
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
        catch (Exception ex)
        {
          Console.WriteLine("failed " + server.apiName);
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
