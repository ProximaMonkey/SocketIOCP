﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Threading;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
           
            WebClient wc = new WebClient();

            WriteMsg("Send.");

            byte[] bytes = wc.DownloadData("http://127.0.0.1:9527?aa=123");

            WriteMsg("Receive: " + Encoding.ASCII.GetString(bytes));

        }

        private void WriteMsg(string msg)
        {
            lock(txtMsg)
            {
                txtMsg.AppendText(DateTime.Now.ToString("HH:mm:ss.fff") + " " + msg + "\r\n");
            }
            
        }

        private DateTime beginTime;

        private void btnConTest_Click(object sender, EventArgs e)
        {

            int threadCount = int.Parse(txtThreadCount.Text);

            this.seq = 0;
            this.isStop = false;
            this.beginTime = DateTime.Now;

            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(new ThreadStart(Send));
                thread.Start();
            }

        }

        int seq = 0;

        private void Send()
        {

            WebClient wc = new WebClient();

            int i;

            byte[] bytes; 

            while(true)
            {
                i = ++seq;

                WriteMsg("第 " + i + " 个 请求 Send.");

                try
                {
                    bytes = wc.DownloadData("http://127.0.0.1:9527?aa=123");

                    WriteMsg("第 " + i + " 个 请求 Receive: " + Encoding.ASCII.GetString(bytes));
                }
                catch(Exception ex)
                {
                    WriteMsg("第 " + i + " 个 请求 Error: " + ex.Message);
                }


                if ( isStop )
                {
                    DateTime endTime = DateTime.Now;

                    TimeSpan timeSpan = endTime - beginTime;

                    lblMsg.Text = "经过时间：" + timeSpan.ToString("mm\\:ss\\.fff")
                        + "  完成请求数：" + this.seq + "  平均每秒处理请求数：" + this.seq / timeSpan.TotalSeconds;

                    break;
                }
            }
            
        }

        private bool isStop;

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.isStop = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.seq = 0;
            txtMsg.Clear();
        }
       
    }

}

