using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageGrabber.classes
{
    public class function_helper
    {
        public static void DataGridViewStyler(DataGridView dtgvList, string colName, int colWidth)
        {
            if (dtgvList.Columns.Contains(colName))
            {
                dtgvList.Columns[colName].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dtgvList.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgvList.Columns[colName].Width = colWidth;
            }

            dtgvList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
