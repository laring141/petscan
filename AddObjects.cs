// Decompiled with JetBrains decompiler
// Type: AucScanner.AddObjects
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
  public class AddObjects : Form
  {
    public BlizzardAPIExplorer explorer;
    private PetStorage storage;
    private IContainer components;
    private TextBox textBox1;
    private Label labelId;
    private Label label1;
    private Button buttonSearch;
    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn PetName;
    private DataGridViewTextBoxColumn ID;
    private DataGridViewTextBoxColumn Delete;
    private BackgroundWorker backgroundWorker1;
    private Label labelUpdate;
    private Button button1;
    private Button button2;

    public AddObjects(BlizzardAPIExplorer explorer, PetStorage st)
    {
      this.InitializeComponent();
      this.explorer = explorer;
      this.storage = st;
      this.storage.explorer = explorer;
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void AddObjects_Load(object sender, EventArgs e)
    {
      this.fillDataTable();
    }

    private void buttonSearch_Click(object sender, EventArgs e)
    {
      this.addPet();
    }

    private void textBox1_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.addPet();
      e.Handled = true;
    }

    private void fillDataTable()
    {
      this.dataGridView1.Rows.Clear();
      foreach (Pet pet in this.storage.savedPetList.Values)
        this.dataGridView1.Rows.Add((object) pet.Name, (object) pet.Stats.SpeciesId.ToString(), (object) "X");
    }

    private void addPet()
    {
      int petId = this.storage.checkPetForId(this.textBox1.Text);
      if (petId > 0)
      {
        if (MessageBox.Show("Пет найден! Добавить в отслеживаемые?", this.textBox1.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.storage.savePet(petId);
        this.fillDataTable();
      }
      else if (petId == -1)
      {
        int num1 = (int) MessageBox.Show("Пет уже добавлен!", "Ошибка!");
      }
      else
      {
        int num2 = (int) MessageBox.Show("Нет такого пета", "Ошибка!");
      }
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      this.storage.deletePet(int.Parse(this.dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString()));
      this.fillDataTable();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.storage.addDefaultPets();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBox1 = new TextBox();
      this.labelId = new Label();
      this.label1 = new Label();
      this.buttonSearch = new Button();
      this.dataGridView1 = new DataGridView();
      this.PetName = new DataGridViewTextBoxColumn();
      this.ID = new DataGridViewTextBoxColumn();
      this.Delete = new DataGridViewTextBoxColumn();
      this.backgroundWorker1 = new BackgroundWorker();
      this.labelUpdate = new Label();
      this.button1 = new Button();
      this.button2 = new Button();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.SuspendLayout();
      this.textBox1.Location = new Point(188, 33);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(521, 29);
      this.textBox1.TabIndex = 0;
      this.textBox1.KeyUp += new KeyEventHandler(this.textBox1_KeyUp);
      this.labelId.AutoSize = true;
      this.labelId.Location = new Point(1212, 37);
      this.labelId.Name = "labelId";
      this.labelId.Size = new Size(0, 25);
      this.labelId.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(50, 37);
      this.label1.Name = "label1";
      this.label1.Size = new Size(99, 25);
      this.label1.TabIndex = 1;
      this.label1.Text = "Название";
      this.label1.Click += new EventHandler(this.label1_Click);
      this.buttonSearch.Location = new Point(754, 37);
      this.buttonSearch.Name = "buttonSearch";
      this.buttonSearch.Size = new Size(154, 46);
      this.buttonSearch.TabIndex = 3;
      this.buttonSearch.Text = "Искать";
      this.buttonSearch.UseVisualStyleBackColor = true;
      this.buttonSearch.Click += new EventHandler(this.buttonSearch_Click);
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange((DataGridViewColumn) this.PetName, (DataGridViewColumn) this.ID, (DataGridViewColumn) this.Delete);
      this.dataGridView1.Location = new Point(55, 97);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.RowTemplate.Height = 31;
      this.dataGridView1.Size = new Size(922, 474);
      this.dataGridView1.TabIndex = 4;
      this.dataGridView1.CellClick += new DataGridViewCellEventHandler(this.dataGridView1_CellClick);
      this.PetName.HeaderText = "Название";
      this.PetName.Name = "PetName";
      this.PetName.ReadOnly = true;
      this.PetName.Width = 200;
      this.ID.HeaderText = "PetID";
      this.ID.Name = "ID";
      this.ID.ReadOnly = true;
      this.ID.Width = 200;
      this.Delete.HeaderText = "Delete";
      this.Delete.Name = "Delete";
      this.Delete.ReadOnly = true;
      this.labelUpdate.AutoSize = true;
      this.labelUpdate.BackColor = SystemColors.Window;
      this.labelUpdate.Font = new Font("Microsoft Sans Serif", 14.14286f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelUpdate.Location = new Point(300, 308);
      this.labelUpdate.Name = "labelUpdate";
      this.labelUpdate.Size = new Size(442, 39);
      this.labelUpdate.TabIndex = 5;
      this.labelUpdate.Text = "Обновляется список петов";
      this.labelUpdate.Visible = false;
      this.button1.Location = new Point(1065, 37);
      this.button1.Name = "button1";
      this.button1.Size = new Size(321, 50);
      this.button1.TabIndex = 6;
      this.button1.Text = "Добавить стандарт";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.AutoScaleDimensions = new SizeF(11f, 24f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1497, 845);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.labelUpdate);
      this.Controls.Add((Control) this.dataGridView1);
      this.Controls.Add((Control) this.buttonSearch);
      this.Controls.Add((Control) this.labelId);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBox1);
      this.Name = nameof (AddObjects);
      this.Text = nameof (AddObjects);
      this.Load += new EventHandler(this.AddObjects_Load);
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
