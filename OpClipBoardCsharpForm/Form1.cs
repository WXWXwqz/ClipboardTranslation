using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpClipBoardCsharpForm
{
    public partial class Form1 : Form
    {
        HtmlElement transWords;
        HtmlElement searchButton;
        HtmlElement OutWords;
        string richtmp;
        bool runingflg = true;
        bool auto = true;
        bool addflag;
        string RichText;
        youdaoYunAPI YoudaoAPI = new youdaoYunAPI();
        webtrans webAPI = new webtrans();
        Thread thread1 = new Thread(myStaticThreadMethod);
        int cnt = 0;
        Inifile inifile;
        public static void myStaticThreadMethod()
        {
            int i = 0;
            while (true)
            {
                // MessageBox.Show("myStaticThreadMethod");
                i++;
                Thread.Sleep(2000);
               // RichText = i.ToString();
            }
       
        }
        public Form1()
        {
            InitializeComponent();
            this.richTextBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseWheel);
            this.webBrowser1.Url = new Uri("http://fanyi.youdao.com");
            this.TopMost = true;
            inifile = new Inifile(Application.StartupPath + @"\OpClipBoardCsharpFormConfig.INI");
            try
            {
               richTextBox1.ZoomFactor = float.Parse(inifile.IniReadValue("Lib", "FontSize"));
            }
            catch
            {
                ;
            }
            if (runingflg)
            {
                timer1.Start();
                label1.Text = "Clip Board Contents：monitor ON";
                button1.Text = "Pause";
                for (int i = 0; i < 5; i++)
                {
                    tmpst[i] = "";
                }
            }
            if (this.TopMost)
            {
                button2.Text = "取消置顶";
            }
            else
            {
                button2.Text = "置顶";
            }
        }
        public void richTextBox1_MouseWheel(object sender, MouseEventArgs e)
        {


            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
             //   richTextBox1.Text = richTextBox1.ZoomFactor.ToString();
                inifile.IniWriteValue("Lib", "FontSize", (richTextBox1.ZoomFactor).ToString());
                //     e.Delta.ToString();
            }
            //if (e.Delta > 0)
            //    addsd -= 0.1f;
            //else
            //    addsd += 0.1f;
            //if (addsd >= 3)
            //    addsd = 3;
            //if (addsd <= 1)
            //    addsd = 1f;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
              //  this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            //this.Visible = true;

            //this.WindowState = FormWindowState.Normal;

           // this.notifyIcon1.Visible = false;
        }

        private void 开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            runingflg = !runingflg;
            if (runingflg)
            {
                MessageBox.Show("ClipBoard Monitoring ON");
                contextMenuStrip1.Items[0].Text = "Pause";
                label1.Text = "Clip Board Contents：monitor ON";
                button1.Text = "Pause";
                timer1.Start();
            }
            else
            {
                MessageBox.Show("ClipBoard Monitoring Pause");
                label1.Text = "Clip Board Contents：monitor OFF";
                button1.Text = "Start";
                contextMenuStrip1.Items[0].Text = "Start";
                //thread1.Suspend();
                timer1.Stop();
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //thread1.Abort();
            Application.Exit();
        }

        private void showMainWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
            this.TopMost = false;
            //  this.TopLevel = true;
            //   this.notifyIcon1.Visible = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            thread1.Abort();
        }
        string tmp_last="";
        string[] tmpst = new string[5];
        private void tmpstrenew(string strnew)
        {
            if (strnew[0] > 'z' && strnew[1] > 'z')
            {
                for (int i = 0; i < 4; i++)
                {
                    tmpst[4 - i] = tmpst[3 - i];
                }
                tmpst[0] = strnew;
            }
        }
        private string DealErrLine(string instr1)
        {
            string instr = instr1;
         //   instr = instr.Replace(".\r\n", "$");
            string[] tsttmpp = instr.Replace("\r\n", "\n").Split('\n');
            int[] linelength= new int[tsttmpp.Length];
            int maxlength;
            for (int i=0;i<tsttmpp.Length;i++)
            {
              //  linelength
                linelength[i] = tsttmpp[i].Length;            
            }
            Array.Sort(linelength);
            maxlength = linelength[tsttmpp.Length/3*2];
            //if ((linelength[49] - linelength[47]) > 10)
            //{
            //    maxlength = linelength[47];
            //}
            //else {
            //    maxlength = (linelength[47] + linelength[48] + linelength[49]) / 3;
            //}

            int last=0;
            int[] site = new int[10];
            int num = 0;
            for (int i = 0; i < instr.Length-2; i++)
            {
                if (instr[i] == '\r' && instr[i + 1] == '\n')
                    last = i;
                if (instr[i] == '.' && instr[i + 1] == '\r' && instr[i + 2] == '\n')
                {
                    if ((i - last) >= maxlength -3)
                    {
                        site[num++] = i;
                    }
                }
            }
            //string ttt="";
            char[] ttt = instr.ToCharArray();
            for (int i = 0; i < num; i++)
            {
                ttt[(site[i]+2)]=' ';
                ttt[(site[i] + 1)] = ' ';
            }
           return new string(ttt);
        }
        private bool isTextEnglish(string instr)
        {
            for (int i = 0; i < 10; i++)
            {
                if (instr[i] > 128)
                    return false;
            }
            return true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            cnt++;
            try
            {
                HtmlElement OutWords = webBrowser1.Document.All["transTarget"];
                if (richtmp != OutWords.OuterText)
                {
                    richtmp = OutWords.OuterText;
                    if (addflag || richTextBox1.Text.Length <= 1170)  //添加
                    {
                        if (!addflag)
                        {
                            if (richTextBox1.Text.Length != 0)
                                richTextBox1.Text += "\r\n    ";
                            else
                                richTextBox1.Text += "    ";
                        }
                        richTextBox1.Text += (richtmp.Replace("\r\n\r\n", "\r\n")).Replace("\r\n\r\n", "\r\n").Replace("\r\n", "\r\n    ");
                    }
                    else
                    {
                        richTextBox1.Text = (richtmp.Replace("\r\n\r\n", "\r\n")).Replace("\r\n\r\n", "\r\n").Replace("\r\n", "\r\n    ");
                    }
                    
                }
                if (auto)
                {
                    autoToolStripMenuItem.Text = "HM";
                }
                else
                {
                    autoToolStripMenuItem.Text = "auto";
                }
                IDataObject iData = Clipboard.GetDataObject();
                if (iData.GetDataPresent(DataFormats.Text))
                {
                    string tmp = ((string)iData.GetData(DataFormats.Text));
                    if (tmp != tmp_last)
                    {
                        if (tmp.Length >= 50)
                        {
                            if (auto)
                            {
                                tmp_last = tmp;
                                //if (tmp[0] <= 128 && tmp[0] >= 0 ||(tmp[0] == ' '&&tmp[1] <= 'z' && tmp[1] >= 'A'))
                                if(isTextEnglish(tmp))
                                {
                                    if (tmp[0] <= 'z' && tmp[0] >= 'a')
                                    {
                                        addflag = true;
                                    }
                                    else
                                    {
                                        addflag = false;
                                    }

                                    tmp=DealErrLine(tmp);
                                    tmp = tmp.Replace(".\r\n", "%￥#");
                                    tmp = tmp.Replace("\r\n", " ");
                                    tmp = tmp.Replace("%￥#", ".\r\n");
                                    
                                   // Clipboard.SetText(tmp);
                                    //Clipboard.SetText("test");    
                                    GetWebAPIResult(tmp);
                                    //richTextBox1.Text = OutWords.OuterText;//
                                    tmpstrenew(tmp);
                                }
                            }
                            else
                            {
                                tmp = tmp.Replace("\r\n", " ");
                                tmp_last = tmp;
                                Clipboard.SetText(tmp);
                                //Clipboard.SetText("test");

                                // richTextBox1.Text = GetWebAPIResult(tmp); 
                             //   richTextBox1.Text = (tmp);
                                tmpstrenew(tmp);
                            }
                        }
                    }
                }
            }
            catch
            {
                ;
            }
        }
        public String GetWebAPIResult(string input)
        {
           // cnt = 0;
            HtmlElement transWords = webBrowser1.Document.All["inputOriginal"];
            transWords.SetAttribute("value", input);//给百度搜索的文本框赋值
            HtmlElement searchButton = webBrowser1.Document.All["transMachine"];//获取百度搜索的按钮//ms_id1
            searchButton.InvokeMember("click");
           // while (cnt < 3) ;
            HtmlElement OutWords = webBrowser1.Document.All["transTarget"];
        
            //  MessageBox.Show(OutWords.InnerText);
            return OutWords.InnerText;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            if (this.TopMost)
            {
                button2.Text = "取消置顶";
            }
            else {
                button2.Text = "置顶";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = tmpst[1];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = tmpst[2];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = tmpst[3];
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = tmpst[4];
        }

        private void AutoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            auto = !auto;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void RichTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void RichTextBox1_DoubleClick(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
