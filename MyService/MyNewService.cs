using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Net;
using System.Timers;
using System.Threading;


namespace MyService
{
    public partial class MyNewService : ServiceBase
    {
        public static EventLog eventLog1;
        private bool isLogin;
        private int eventId = 0;

        public MyNewService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }
        void PrinterStart()
        {
            DvPrinter dv = new DvPrinter();
            dv.DvStart();
        }

        protected override void OnStart(string[] args)
        {
            isLogin = false;
            eventLog1.WriteEntry("Typress Start!");

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
            System.Diagnostics.Debugger.Launch();

            ThreadStart th = new ThreadStart(PrinterStart);
            Thread thread1 = new Thread(th);
            thread1.Start();

            // OnPrinterStatusChange() ----> 
            // if(isLogin == false)
            //  loginForm.exe 
            //      -> 성공 isLogin = true;
            //      -> 실패 isLogin = false;
            // else
            //  
        }

        /*
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string outputFile = Path.Combine(dataFolder, "csharp-news.html");

            WebClient cli = new WebClient();
            cli.DownloadFile("http://csharp.news", outputFile);
            Console.WriteLine("g");
        }
        */ // 이벤트

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Typress Exit!");
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("Typress Continue!");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            eventLog1.WriteEntry("Monitoring Typress", EventLogEntryType.Information, eventId++);
        }
    }
}
