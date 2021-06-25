using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Threading;
using System.Diagnostics;

namespace QuizHint
{
    class QuizWithHint
    {
        private talk _talk;
        private string[][] _hint;
        private QuizForm _quizform;
        private PictureBox pb;
        private Size monitorSize = Screen.PrimaryScreen.Bounds.Size;
        private Timer _sleeptimer;
        private int _quizcount;
        private int _hintcount;
        private int QuizNum;
        private int HintNum;
        Dispatcher dispUI;

        public QuizWithHint(talk ai, string[][] Hint)
        {
            _talk = ai;
            _hint = Hint;
            _quizform = new QuizForm();
            pb = new PictureBox();
            pb.Image = new Bitmap("../../data/tmp.jpg");
            pb.Size = new Size(monitorSize.Width, monitorSize.Height);
            pb.Parent = _quizform;
            pb.Click += new EventHandler(pb_click);
            //            pb.Hide();
            QuizNum = _hint.Length;
            dispUI = Dispatcher.CurrentDispatcher;

            _sleeptimer = new Timer { Interval = 5000, Enabled = true };
            _sleeptimer.Tick += new EventHandler(_sleeptimer_Tick);
        }

        public void run()
        {
            HintNum = _hint[0].Length;
            _sleeptimer.Start();
            dispUI.BeginInvoke(new Action(() =>_talk.talkfromkana(0, 0)));
            _quizform.ShowDialog();
            _quizcount = 0;
            _hintcount = 0;
        }
        private void talkfromtxt(string str)
        {
            _talk.talkfromstring(str);
        }
        private void NextorFinish()
        {
            if(_quizcount == QuizNum)
            {
                Close();
            }
            else
            {
                if(_hintcount == HintNum - 1)
                {
                    if (_quizcount == QuizNum - 1)
                    {
                        _talk.talkfromstring("終わりだよー");
                        _quizcount++;
                    }
                    else
                    {
                        _quizcount++;
                        _hintcount = 0;
                        HintNum = _hint[_quizcount].Length;
                        dispUI.BeginInvoke(new Action(() => _talk.talkfromkana(_quizcount, _hintcount)));
                        pb.Image = new Bitmap("../../data/tmp" + _quizcount.ToString() + ".jpg");
                    }
                }
                else
                {
                    _hintcount++;
                    dispUI.BeginInvoke(new Action(() => _talk.talkfromkana(_quizcount, _hintcount)));
                }
                _sleeptimer.Start();
            }
        }
/*
        private void NextorFinish()
        {
            if (_quizcount < QuizNum)
            {
                if (_hintcount < HintNum)
                {
                    dispUI.BeginInvoke(new Action(() => talkfromtxt(_hint[_quizcount][_hintcount])));
                    _hintcount++;
                }
                else
                {
                    if (_quizcount == QuizNum - 1)
                    {
                        _talk.talkfromstring("終わりだよー");
                        _quizcount++;
                    }
                    else
                    {
                        _quizcount++;
                        _hintcount = 1;
                        HintNum = _hint[_quizcount].Length;
                        dispUI.BeginInvoke(new Action(() => talkfromtxt(_hint[_quizcount][0])));
                        pb.Image = new Bitmap("../../data/tmp" + _quizcount.ToString() + ".jpg");
                    }
                }
                _sleeptimer.Start();
            }
            else
            {
                Close();
            }
        }
*/
        /*
       void NextorFinish()
       {
           if (_quizcount < QuizNum)
           {
               if(_hintcount < HintNum)
               {
                   _talk.talkfromstring(_hint[_quizcount][_hintcount]);
                   _hintcount++;
                   _sleeptimer.Start();
               }
               else
               {
                   _quizcount++;
                   _hintcount = 0;
                   pb.Image = new Bitmap("../../data/tmp"+_quizcount.ToString() + ".jpg");
                   _talk.talkfromstring(_hint[_quizcount][0]);
                   _hintcount = 1;
                   HintNum = _hint[_quizcount].Length;
                   _sleeptimer.Start();
               }
           }
           else
           {
               Close();
           }
       }*/
        void _sleeptimer_Tick(object sender, EventArgs e)
        {
            _sleeptimer.Stop();
            NextorFinish();
        }

        void pb_click(object sender, EventArgs e)
        {
            Close();
        }

        private void Close()
        {
            _talk.End();
            _quizform.Close();
        }
    }
}
