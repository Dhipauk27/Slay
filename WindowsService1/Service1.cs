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
using System.Threading;
using System.IO;

//TO-DO auto start service post installation
namespace WindowsService1
{
    [RunInstaller(true)]
    public partial class Service1 : ServiceBase
    {
        public Thread Worker = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ThreadStart start = new ThreadStart(Working);
                Worker = new Thread(start);
                Worker.Start();
            } catch(Exception)
            {
                throw;
            }
        }

        public void Working ()
        {
            while(true)
            {
                string path = "C:\\processKiller.txt";
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine("\n Checking through the following list");
                    writer.Close();
                }
                PrintProcessesList(path);
                CheckIfProcessIsRunning("EXCEL", path);
            }
        }

        static void PrintProcessesList(string path)
        {
            Process[] processList = Process.GetProcesses();
            foreach (Process p in processList)
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(string.Format(p.ProcessName));
                    writer.Close();
                }
            }
        }

        static void CheckIfProcessIsRunning(string processName, string path)
        {
            PrintProcessesList(path);

            Process[] processList = Process.GetProcesses();
            foreach(Process p in processList)
            {
                if (p.ProcessName.Contains(processName))
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine("Process found" + processName + p.Id);
                        writer.Close();
                    }
                    
                    //kill the process
                    p.Kill();
                }
            }
            CheckAgainIfProcessIsRunning(processName, path);
        }

        static void CheckAgainIfProcessIsRunning(string processName, string path)
        {
            Thread.Sleep(1 * 60 * 1000);
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("Checking again in 1 minute \n");
                writer.Close();
            }
            CheckIfProcessIsRunning(processName, path);
        }

        protected override void OnStop()
        {
            try
            {
                if((Worker != null) & Worker.IsAlive)
                {
                    Worker.Abort();
                }
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
