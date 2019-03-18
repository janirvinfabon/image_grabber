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
        public static void DataGridViewStyler(DataGridView dtgv, string colName, int colWidth)
        {
            if (dtgv.Columns.Contains(colName))
            {
                dtgv.Columns[colName].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dtgv.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtgv.Columns[colName].Width = colWidth;
            }

            dtgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
