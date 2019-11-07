using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpClipBoardCsharpForm
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //MessageBox.Show("11");
            //IDataObject iData = Clipboard.GetDataObject();
            //Clipboard.SetText("test");
            //if (iData.GetDataPresent(DataFormats.Text))
            //{
            //    MessageBox.Show((string)iData.GetData(DataFormats.Text));
            //}
            //else
            //    MessageBox.Show("目前剪贴板中数据不可转换为文本", "错误");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
