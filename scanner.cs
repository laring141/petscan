// Decompiled with JetBrains decompiler
// Type: AucScanner.scanner
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using AucScanner.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace AucScanner
{
  public class scanner : Form
  {
    private Dictionary<int, DataGridViewRow> rowsHelper = new Dictionary<int, DataGridViewRow>();
    private PetImage petImageHelper = new PetImage();
    private object lockThis = new object();
    private sortRows sorter = new sortRows();
    private BlizzardAPIExplorer explorer;
    private ServersUpdate updater;
    private PetStorage petStorage;
    private Timer updateTimer;
    private Timer countTimer;
    private int timerCounter;
    private IContainer components;
    private DataGridView dataGridView1;
    private Button button1;
    private Button buttonAdd;
    private Label labelUpdatePets;
    private Button button2;
    private Label updateLabel;
    private DataGridViewImageColumn petImage;
    private DataGridViewTextBoxColumn PetName;
    private DataGridViewTextBoxColumn wantedPrice;

    public scanner()
    {
      this.explorer = new BlizzardAPIExplorer("u8mz5ymmphznxvqw33eup9qr9s75pv5z");
      LocalSettings.Load();
      this.petStorage = new PetStorage(this.explorer);
      this.updater = new ServersUpdate(this.explorer);
      this.InitializeComponent();
      this.updateTimer = new Timer();
      this.updateTimer.Interval = LocalSettings.settings.TimeToUpdateMin * 60 * 1000;
      this.updateTimer.Tick += new EventHandler(this.UpdateTimer_Tick);
      this.countTimer = new Timer();
      this.countTimer.Interval = 1000;
      this.countTimer.Tick += new EventHandler(this.CountTimer_Tick);
      this.dataGridView1.Sort((IComparer) this.sorter);
    }

    private void CountTimer_Tick(object sender, EventArgs e)
    {
      --this.timerCounter;
      if (this.timerCounter < 0)
        this.timerCounter = 0;
      this.updateLabel.Text = string.Format("Следующее обновление через {0}:{1}", (object) (this.timerCounter / 60), (object) (this.timerCounter % 60));
    }

    private void UpdateTimer_Tick(object sender, EventArgs e)
    {
      this.updateData();
    }

    private void updateData()
    {
      this.timerCounter = LocalSettings.settings.TimeToUpdateMin * 60;
      this.clearData();
      this.updater.refreshData(new ServerUpdateStartedDelegate(this.ServerDataStarted), new ServerUpdateCompleteDelegate(this.ServerDataUpdated));
    }

    public void FillServerColumn(Server server)
    {
      lock (this.lockThis)
      {
        foreach (Auction auction in server.auctions.auctions)
        {
          if (auction.item == 82800L && auction.Buyout > 0L)
          {
            int petSpeciesId = auction.petSpeciesId;
            long num = auction.Buyout / 10000L;
            if (this.petStorage.savedPetList.Keys.Contains<int>(petSpeciesId))
            {
              DataGridViewRow dataGridViewRow = this.rowsHelper[petSpeciesId];
              if (dataGridViewRow.Cells[server.apiName].Value == null)
                dataGridViewRow.Cells[server.apiName].Value = (object) num;
              else if ((long) dataGridViewRow.Cells[server.apiName].Value > num)
                dataGridViewRow.Cells[server.apiName].Value = (object) num;
            }
          }
        }
        this.setMinMaxValues();
        this.checkForWanted(server.apiName);
      }
    }

    private void checkForWanted(string apiName)
    {
      string str = "";
      foreach (DataGridViewRow row in (IEnumerable) this.dataGridView1.Rows)
      {
        if (row.Cells["wantedPrice"].Value != null && row.Cells[apiName].Value != null && row.Cells[apiName].Value.ToString() != "")
        {
          int num1 = int.Parse(row.Cells["wantedPrice"].Value.ToString());
          int num2 = int.Parse(row.Cells[apiName].Value.ToString());
          int num3 = num2;
          if (num1 >= num3)
          {
            row.Tag = (object) 1;
            str = str + row.Cells["petName"].Value.ToString() + "  " + num2.ToString() + "\n";
            row.Cells["petName"].Style.BackColor = Color.LawnGreen;
          }
        }
      }

      if (!(str != "") || !this.InvokeRequired)
        return;

      /*this.Invoke((Delegate) (() =>
      {
        this.dataGridView1.Sort((IComparer) this.sorter);
        this.PopUpThisForm();
      }));*/
    }

    public void setMinMaxValues()
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dataGridView1.Rows)
      {
        long num1 = 999999999;
        long num2 = 0;
        int num3 = 5;
        for (int index = num3; index < row.Cells.Count; ++index)
        {
          DataGridViewCell cell = row.Cells[index];
          if (cell.Value != null)
          {
            if (num2 < (long) cell.Value)
              num2 = (long) cell.Value;
            if (num1 > (long) cell.Value)
              num1 = (long) cell.Value;
          }
        }
        for (int index = num3; index < row.Cells.Count; ++index)
        {
          DataGridViewCell cell = row.Cells[index];
          if (this.dataGridView1.Columns[index].HeaderCell.Style.BackColor != Color.Green)
          {
            cell.Style.BackColor = Color.Gray;
          }
          else
          {
            cell.Style.BackColor = Color.White;
            if (cell.Value != null)
            {
              if (num2 == (long) cell.Value)
                cell.Style.BackColor = Color.LightSalmon;
              if (num1 == (long) cell.Value)
              {
                cell.Style.BackColor = Color.LightGreen;
                row.Cells["minimal"].Value = (object) this.dataGridView1.Columns[index].HeaderText;
                row.Cells["minprice"].Value = (object) num1;
              }
            }
          }
        }
      }
    }

    public void PetsDataUpdated()
    {
      this.button1.Enabled = true;
      this.dataGridView1.Enabled = true;
      this.buttonAdd.Enabled = true;
      this.labelUpdatePets.Visible = false;
      this.button2.Enabled = true;
      if (LocalSettings.settings.DefaultPetsVersion < this.petStorage.petVersion)
        this.petStorage.addDefaultPets();
      this.setPetRows();
    }

    private void clearData()
    {
      foreach (DataGridViewRow row in (IEnumerable) this.dataGridView1.Rows)
      {
        row.Tag = (object) 0;
        row.Cells["petName"].Style.BackColor = Color.White;
        for (int index = 3; index < row.Cells.Count; ++index)
        {
          row.Cells[index].Value = (object) null;
          if (index > 4)
            row.Cells[index].Style.BackColor = Color.Gray;
        }
      }
      this.dataGridView1.Sort((IComparer) this.sorter);
    }

    private void setServersColumns()
    {
      this.dataGridView1.Columns["wantedPrice"].Width = 40;
      this.dataGridView1.Columns["wantedPrice"].DefaultCellStyle.Font = new Font("Arial", 9f, GraphicsUnit.Pixel);
      this.dataGridView1.Columns.Add("minimal", "Сервер");
      this.dataGridView1.Columns["minimal"].ValueType = typeof (string);
      this.dataGridView1.Columns["minimal"].Width = 80;
      this.dataGridView1.Columns["minimal"].ReadOnly = true;
      this.dataGridView1.Columns["minimal"].DefaultCellStyle.Font = new Font("Arial", 9f, GraphicsUnit.Pixel);
      this.dataGridView1.Columns.Add("minprice", "минимальная цена");
      this.dataGridView1.Columns["minprice"].ValueType = typeof (long);
      this.dataGridView1.Columns["minprice"].Width = 40;
      this.dataGridView1.Columns["minprice"].ReadOnly = true;
      this.dataGridView1.Columns["minprice"].DefaultCellStyle.Font = new Font("Arial", 9f, GraphicsUnit.Pixel);
      foreach (Server server in this.updater.servers)
      {
        this.dataGridView1.Columns.Add(server.apiName, server.russianName);
        this.dataGridView1.Columns[server.apiName].ValueType = typeof (long);
        this.dataGridView1.Columns[server.apiName].Width = 40;
        this.dataGridView1.Columns[server.apiName].ReadOnly = true;
        this.dataGridView1.Columns[server.apiName].DefaultCellStyle.Font = new Font("Arial", 9f, GraphicsUnit.Pixel);
      }
    }

    private void setPetRows()
    {
      if (this.petStorage.savedPetList.Count<KeyValuePair<int, Pet>>() == 0)
        return;
      this.dataGridView1.Rows.Clear();
      this.rowsHelper.Clear();
      this.dataGridView1.Rows.Add(this.petStorage.savedPetList.Count<KeyValuePair<int, Pet>>());
      int index = 0;
      foreach (Pet pet in new List<Pet>((IEnumerable<Pet>) this.petStorage.savedPetList.Values).OrderBy<Pet, string>((Func<Pet, string>) (o => o.Name)).ToList<Pet>())
      {
        this.dataGridView1.Rows[index].Tag = (object) 0;
        this.dataGridView1.Rows[index].Height = 20;
        this.dataGridView1.Rows[index].Cells["PetName"].Value = (object) pet.Name;
        this.dataGridView1.Rows[index].Cells["PetName"].Tag = (object) pet.Stats.SpeciesId;
        this.petImageHelper.fillDataCellWithImage((DataGridViewImageCell) this.dataGridView1.Rows[index].Cells["petImage"], pet.Icon);
        if (LocalSettings.settings.StoredPets[(long) pet.Stats.SpeciesId] > 0L)
          this.dataGridView1.Rows[index].Cells["wantedPrice"].Value = (object) LocalSettings.settings.StoredPets[(long) pet.Stats.SpeciesId];
        this.rowsHelper.Add(pet.Stats.SpeciesId, this.dataGridView1.Rows[index]);
        ++index;
      }
    }

    public void ServerDataStarted(Server server)
    {
      this.dataGridView1.Columns[server.apiName].HeaderCell.Style.BackColor = Color.Cyan;
    }

    public void ServerDataUpdated(Server server)
    {
      this.dataGridView1.Columns[server.apiName].HeaderCell.Style.BackColor = Color.Green;
      this.FillServerColumn(server);
    }

    private void setYellowStatusToServers()
    {
      for (int index = 1; index < this.dataGridView1.Columns.Count; ++index)
        this.dataGridView1.Columns[index].HeaderCell.Style.BackColor = Color.Yellow;
    }

    private void scanner_Shown(object sender, EventArgs e)
    {
      this.dataGridView1.Width = this.Width - 30;
      this.dataGridView1.Height = this.Height - this.dataGridView1.Top - 50;
      this.setServersColumns();
      this.button1.Enabled = false;
      this.dataGridView1.Enabled = false;
      this.buttonAdd.Enabled = false;
      this.labelUpdatePets.Visible = true;
      this.button2.Enabled = false;
      this.petStorage.refreshData(new RefreshCompleteDelegate(this.PetsDataUpdated));
      this.dataGridView1.EnableHeadersVisualStyles = false;
      this.updateTimer.Enabled = false;
      this.setUpdateButtonText();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      new AddObjects(this.explorer, this.petStorage).Show();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.updateData();
    }

    private void scanner_SizeChanged(object sender, EventArgs e)
    {
      this.dataGridView1.Width = this.Width - 30;
      this.dataGridView1.Height = this.Height - this.dataGridView1.Top - 50;
    }

    private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
    }

    private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (!(this.dataGridView1.Columns[e.ColumnIndex].Name == "wantedPrice"))
        return;
      DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell) this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
      int tag = (int) this.dataGridView1.Rows[e.RowIndex].Cells["PetName"].Tag;
      if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
      {
        LocalSettings.settings.StoredPets[(long) tag] = -1L;
        LocalSettings.Save();
      }
      else
      {
        int result;
        if (int.TryParse(cell.Value.ToString(), out result))
        {
          LocalSettings.settings.StoredPets[(long) tag] = (long) result;
          LocalSettings.Save();
        }
        else
        {
          long storedPet = LocalSettings.settings.StoredPets[(long) tag];
          cell.Value = storedPet > -1L ? (object) storedPet.ToString() : (object) "";
          int num = (int) MessageBox.Show("Неправильное значение", "Ошибка");
        }
      }
    }

    private void showMessage(string message, string server)
    {
      string caption = string.Format("Сервер {0}", (object) server);
      int num = (int) MessageBox.Show(message, caption);
    }

    private void setUpdateButtonText()
    {
      this.button2.Text = this.updateTimer.Enabled ? "Остановить обновления" : "Запустить обновления";
    }

    private void button2_Click_1(object sender, EventArgs e)
    {
      if (this.updateTimer.Enabled)
      {
        this.updateTimer.Enabled = false;
        this.updateTimer.Stop();
        this.countTimer.Stop();
      }
      else
      {
        this.updateData();
        this.updateTimer.Enabled = true;
        this.updateTimer.Start();
        this.countTimer.Start();
      }
      this.setUpdateButtonText();
    }

    private void openAddInfo(string apiName, int petId)
    {
      Server server1 = (Server) null;
      foreach (Server server2 in this.updater.servers)
      {
        if (server2.apiName == apiName)
        {
          server1 = server2;
          break;
        }
      }
      new AdditionalInfo(server1, this.petStorage.savedPetList[petId]).Show();
    }

    private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      DataGridViewCell cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
      if (e.ColumnIndex <= 4 || cell.Value == null)
        return;
      this.openAddInfo(this.dataGridView1.Columns[e.ColumnIndex].Name, (int) this.dataGridView1.Rows[e.RowIndex].Cells["petName"].Tag);
    }

    private void PopUpThisForm()
    {
      new SoundPlayer("c:\\Windows\\Media\\tada.wav").Play();
      if (this.WindowState == FormWindowState.Minimized)
      {
        this.WindowState = FormWindowState.Normal;
      }
      else
      {
        this.TopMost = true;
        this.Focus();
        this.BringToFront();
        this.TopMost = false;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      this.dataGridView1 = new DataGridView();
      this.button1 = new Button();
      this.buttonAdd = new Button();
      this.labelUpdatePets = new Label();
      this.button2 = new Button();
      this.updateLabel = new Label();
      this.petImage = new DataGridViewImageColumn();
      this.PetName = new DataGridViewTextBoxColumn();
      this.wantedPrice = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.SuspendLayout();
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange((DataGridViewColumn) this.petImage, (DataGridViewColumn) this.PetName, (DataGridViewColumn) this.wantedPrice);
      this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
      this.dataGridView1.Location = new Point(38, 97);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.RowTemplate.Height = 31;
      this.dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
      this.dataGridView1.Size = new Size(2332, 950);
      this.dataGridView1.TabIndex = 0;
      this.dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
      this.dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
      this.dataGridView1.DataError += new DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
      this.button1.Location = new Point(412, 22);
      this.button1.Name = "button1";
      this.button1.Size = new Size(267, 44);
      this.button1.TabIndex = 1;
      this.button1.Text = "Обновить данные ";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.buttonAdd.Location = new Point(707, 27);
      this.buttonAdd.Name = "buttonAdd";
      this.buttonAdd.Size = new Size(359, 39);
      this.buttonAdd.TabIndex = 2;
      this.buttonAdd.Text = "Добавить обьекты сканирования";
      this.buttonAdd.UseVisualStyleBackColor = true;
      this.buttonAdd.Click += new EventHandler(this.button2_Click);
      this.labelUpdatePets.Font = new Font("Microsoft Sans Serif", 14.14286f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelUpdatePets.Location = new Point(752, 399);
      this.labelUpdatePets.Name = "labelUpdatePets";
      this.labelUpdatePets.Size = new Size(604, 168);
      this.labelUpdatePets.TabIndex = 3;
      this.labelUpdatePets.Text = "Обновляется список петов";
      this.labelUpdatePets.TextAlign = ContentAlignment.MiddleCenter;
      this.button2.Location = new Point(38, 1);
      this.button2.Name = "button2";
      this.button2.Size = new Size(316, 90);
      this.button2.TabIndex = 4;
      this.button2.Text = "Запустить обновления";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click_1);
      this.updateLabel.AutoSize = true;
      this.updateLabel.Location = new Point(1138, 27);
      this.updateLabel.Name = "updateLabel";
      this.updateLabel.Size = new Size(0, 25);
      this.updateLabel.TabIndex = 5;
      gridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
      gridViewCellStyle.NullValue = (object) "System.Drawing.Bitmap";
      this.petImage.DefaultCellStyle = gridViewCellStyle;
      this.petImage.HeaderText = "";
      this.petImage.Name = "petImage";
      this.petImage.ReadOnly = true;
      this.petImage.Width = 50;
      this.PetName.HeaderText = "Название";
      this.PetName.Name = "PetName";
      this.PetName.ReadOnly = true;
      this.PetName.Width = 150;
      this.wantedPrice.HeaderText = "нужная цена";
      this.wantedPrice.Name = "wantedPrice";
      this.AutoScaleDimensions = new SizeF(11f, 24f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(2382, 1099);
      this.Controls.Add((Control) this.updateLabel);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.labelUpdatePets);
      this.Controls.Add((Control) this.buttonAdd);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.dataGridView1);
      this.Name = nameof (scanner);
      this.Text = "AucScanner";
      this.Shown += new EventHandler(this.scanner_Shown);
      this.SizeChanged += new EventHandler(this.scanner_SizeChanged);
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
