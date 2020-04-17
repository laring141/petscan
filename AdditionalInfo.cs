// Decompiled with JetBrains decompiler
// Type: AucScanner.AdditionalInfo
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using AucScanner.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AucScanner
{
  public class AdditionalInfo : Form
  {
    private Server server;
    private Pet pet;
    private IContainer components;
    private Label labelPetName;
    private Label labelServerName;
    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn level;
    private DataGridViewTextBoxColumn breed;
    private DataGridViewTextBoxColumn Price;
    private DataGridViewTextBoxColumn Seller;

    public AdditionalInfo(Server server, Pet pet)
    {
      this.server = server;
      this.pet = pet;
      this.InitializeComponent();
    }

    private void fillNames()
    {
      this.labelPetName.Text = this.pet.Name;
      this.labelServerName.Text = this.server.serverName;
    }

    private void fillPrices()
    {
      foreach (Auction auction in this.server.auctions.auctions)
      {
        if (auction.isPet && auction.Buyout > 0L && auction.petSpeciesId == this.pet.BattlePetId)
        {
          int petSpeciesId = auction.petSpeciesId;
          long num = auction.Buyout / 10000L;
          string str = breedString(auction.petBreedId);
          int petLevel = auction.petLevel;
          DataGridViewRow dataGridViewRow = (DataGridViewRow) this.dataGridView1.RowTemplate.Clone();
          dataGridViewRow.CreateCells(this.dataGridView1, (object) petLevel, (object) str, (object) num);
          if (auction.petQualityId == 2)
            dataGridViewRow.Cells[1].Style.ForeColor = Color.Green;
          if (auction.petQualityId == 3)
            dataGridViewRow.Cells[1].Style.ForeColor = Color.Blue;
          this.dataGridView1.Rows.Add(dataGridViewRow);
        }
      }
    }

    private string breedString(int breedId)
    {
      switch (breedId)
      {
        case 3:
        case 13:
          return "B/B";
        case 4:
        case 14:
          return "P/P";
        case 5:
        case 15:
          return "S/S";
        case 6:
        case 16:
          return "H/H";
        case 7:
        case 17:
          return "H/P";
        case 8:
        case 18:
          return "P/S";
        case 9:
        case 19:
          return "H/S";
        case 10:
        case 20:
          return "P/B";
        case 11:
        case 21:
          return "S/B";
        case 12:
        case 22:
          return "H/B";
        default:
          return "";
      }
    }

    private void AdditionalInfo_Shown(object sender, EventArgs e)
    {
      this.fillNames();
      this.fillPrices();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.labelPetName = new Label();
      this.labelServerName = new Label();
      this.dataGridView1 = new DataGridView();
      this.level = new DataGridViewTextBoxColumn();
      this.breed = new DataGridViewTextBoxColumn();
      this.Price = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.SuspendLayout();
      this.labelPetName.AutoSize = true;
      this.labelPetName.Location = new Point(87, 55);
      this.labelPetName.Name = "labelPetName";
      this.labelPetName.Size = new Size(0, 25);
      this.labelPetName.TabIndex = 0;
      this.labelServerName.AutoSize = true;
      this.labelServerName.Location = new Point(347, 55);
      this.labelServerName.Name = "labelServerName";
      this.labelServerName.Size = new Size(0, 25);
      this.labelServerName.TabIndex = 1;
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToDeleteRows = false;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange((DataGridViewColumn) this.level, (DataGridViewColumn) this.breed, (DataGridViewColumn) this.Price);
      this.dataGridView1.Location = new Point(64, 100);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.ReadOnly = true;
      this.dataGridView1.RowTemplate.Height = 31;
      this.dataGridView1.Size = new Size(1226, 570);
      this.dataGridView1.TabIndex = 2;
      this.level.HeaderText = "Уровень";
      this.level.Name = "level";
      this.level.ReadOnly = true;
      this.breed.HeaderText = "Тип";
      this.breed.Name = "breed";
      this.breed.ReadOnly = true;
      this.Price.HeaderText = "Цена";
      this.Price.Name = "Price";
      this.Price.ReadOnly = true;
      this.AutoScaleDimensions = new SizeF(11f, 24f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1601, 847);
      this.Controls.Add((Control) this.dataGridView1);
      this.Controls.Add((Control) this.labelServerName);
      this.Controls.Add((Control) this.labelPetName);
      this.Name = nameof (AdditionalInfo);
      this.Text = nameof (AdditionalInfo);
      this.Shown += new EventHandler(this.AdditionalInfo_Shown);
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
