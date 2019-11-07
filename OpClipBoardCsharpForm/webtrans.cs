using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpClipBoardCsharpForm
{
    class webtrans
    {
        System.Windows.Forms.WebBrowser webBrowser1;
        public String GetResult(string input)
        {
            this.webBrowser1.Url = new Uri("http://fanyi.youdao.com");
            HtmlElement transWords = webBrowser1.Document.All["inputOriginal"];
            transWords.SetAttribute("value", input);//给百度搜索的文本框赋值
             HtmlElement searchButton = webBrowser1.Document.All["transMachine"];//获取百度搜索的按钮//ms_id1
            searchButton.InvokeMember("click");
            HtmlElement OutWords = webBrowser1.Document.All["transTarget"];
          
          //  MessageBox.Show(OutWords.InnerText);
            return OutWords.InnerText;
        }
    }
}
