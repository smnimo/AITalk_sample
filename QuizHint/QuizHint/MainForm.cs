using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuizHint
{
    public partial class MainForm : Form
    {
        talk _ai;
        string[][] Hint;
        QuizWithHint _qwh;

        public MainForm()
        {
            // prepare AITalk
            Hint = Readfromcsv("../../data/hint.csv");
            _ai = new talk(Hint);
            InitializeComponent();

            // prepare Quiz
            _qwh = new QuizWithHint(_ai, Hint);
        }

        private string[][] Readfromcsv(string datapath)  // tagtsv --> WordList
        {
            // read from file
            StreamReader objReader = new StreamReader(datapath, System.Text.Encoding.GetEncoding("shift-jis"));
            string sLine = "";
            ArrayList arrText = new ArrayList();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                {
                    arrText.Add(sLine);
                }
            }
            objReader.Close();

            // count
            int line_count = arrText.Count;
            string tmp = (string)arrText[0];

            // ArrayList --> arry
            string[][] InData = new string[line_count][];
            int a = 0;
            foreach (string sOutput in arrText)
            {
                InData[a] = sOutput.Split(',');                   
                a++;
            }
            return InData;
        }
        private void Start_Button_Click(object sender, EventArgs e)
        {
            _qwh.run();
        }
    }
}
