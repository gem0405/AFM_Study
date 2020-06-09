using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Encode
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Encodebtn_Click(object sender, RoutedEventArgs e)
        {
           
            Encodetxt.Text = TrunEncode(PassWordtxt.Text);
        }   private void Ucodebtn_Click(object sender, RoutedEventArgs e)
        {
           if(Encodetxt.Text != null)
            Encodetxt.Text = TrunUncode(Encodetxt.Text);
        }
        public string TrunEncode(string Fromcode)
        {
            string EncodeView = "";
            int jj = Fromcode.Trim().Length;
            for (int i = 0; i < jj; i++)
            {
                EncodeView = EncodeView + StringEnDeCodecn(Fromcode.Trim().Substring(i, 1));
                
            }
            return EncodeView;
        //    Dim EncodeView As String = ""
        //Dim i As Integer
        //Dim jj As Integer = Len(Trim(FormCode))

        //For i = 1 To jj Step 1
        //    EncodeView = EncodeView & StringEnDeCodecn(Mid(Trim(FormCode), i, 1))
        //Next i
        //Return EncodeView
        }
        public string TrunUncode(string Fromcode)
        {
            string UncodeView = "";
            int jj = Fromcode.Length;
            for (int i = 0; i < jj; i++)
            {
                UncodeView = UncodeView + StringEnDeCodecn(Fromcode.Trim().Substring(i, 1));

            }
            return UncodeView;
        }

       public string StringEnDeCodecn(String strSource )
        {
            long CHARNUM;

            string SINGLECHAR;
            string strTmp="";
            int i, k;
            for (i = 0; i < strSource.Length; i++)
            {
                SINGLECHAR = strSource.Substring(i, 1);
                CHARNUM = (int)Convert.ToChar(SINGLECHAR);
                CHARNUM = CHARNUM ^ 5;
                strTmp = strTmp + Convert.ToChar(CHARNUM);
            }
            if (strTmp == "?")
            {
                strTmp = "";
                for (k=0;k<strSource.Length;k++)
                {
                    SINGLECHAR = strSource.Substring(k, 1);
                    CHARNUM = (int)Convert.ToChar(SINGLECHAR);
                    strTmp = strTmp + Convert.ToChar(CHARNUM);
                }
            }
            return strTmp;
        //For i = 1 To Len(strSource) Step 1
        //    SINGLECHAR = Mid(strSource, i, 1)
        //    CHARNUM = Asc(SINGLECHAR)
        //    CHARNUM = CHARNUM Xor 5
        //    strTmp = strTmp & Chr(CHARNUM)
        //Next i

        //If strTmp = "?" Then
        //    strTmp = ""
        //    For k = 1 To Len(strSource) Step 1
        //        SINGLECHAR = Mid(strSource, k, 1)
        //        CHARNUM = Asc(SINGLECHAR)
        //        strTmp = strTmp & Chr(CHARNUM)
        //    Next k
        //End If
        //Return strTmp
        //Exit Function
   
                }

     
    }
}
