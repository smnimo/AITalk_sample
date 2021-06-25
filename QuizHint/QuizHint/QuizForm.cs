using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuizHint
{
    public partial class QuizForm : Form
    {
        public QuizForm()
        {
            InitializeComponent();
        }
        public void QuizForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
