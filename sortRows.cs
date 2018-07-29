// Decompiled with JetBrains decompiler
// Type: AucScanner.sortRows
// Assembly: AucScanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F26D38D8-5498-496B-8AB7-C8B101AF70B8
// Assembly location: C:\ESD\scanner\AucScanner_RU.exe

using System.Collections;
using System.Windows.Forms;

namespace AucScanner
{
  public class sortRows : IComparer
  {
    int IComparer.Compare(object a, object b)
    {
      DataGridViewRow dataGridViewRow1 = (DataGridViewRow) a;
      DataGridViewRow dataGridViewRow2 = (DataGridViewRow) b;
      string str = dataGridViewRow1.Cells["petName"].Value.ToString();
      string strB = dataGridViewRow2.Cells["petName"].Value.ToString();
      int tag1 = (int) dataGridViewRow1.Tag;
      int tag2 = (int) dataGridViewRow2.Tag;
      if (tag1 == tag2)
        return str.CompareTo(strB);
      return tag1 >= tag2 ? -1 : 1;
    }
  }
}
