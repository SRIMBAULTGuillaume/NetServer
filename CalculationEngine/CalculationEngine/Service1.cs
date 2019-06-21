using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration.Install;

namespace CalculationEngine
{
    public partial class Calculation_Engine : ServiceBase
    {
        //Variable
        private Timer t = null;

        public Calculation_Engine()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            t = new Timer(10000); // Timer de 10 secondes.
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
            t.Start();
        }

        protected override void OnStop()
        {
            t.Stop();
        }

        protected void t_Elapsed(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\temp\test.txt"))
            {
                StreamWriter sw = new StreamWriter(@"C:\temp\test.txt");
                sw.WriteLine(DateTime.Now.ToString());
                sw.Close();
            }
            else
            {
                TextWriter file = File.CreateText(@"C:\temp\test.txt");
                file.WriteLine(DateTime.Now.ToString());
                file.Close();
            }
        }


        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
        }
    
    }   //fin CalculationEngine

    [RunInstaller(true)]
    public class ServiceInstall : Installer
    {
        public ServiceInstall() : base()
        {
            // On définit le compte sous lequel le service sera lancÃ© (compte Système)
            ServiceProcessInstaller process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;

            // On définit le mode de lancement (Manuel), le nom du service et sa description
            ServiceInstaller service = new ServiceInstaller();
            service.StartType = ServiceStartMode.Manual;
            service.ServiceName = "Developpez";
            service.DisplayName = "Developpez";
            service.Description = "Service de test pour DVP";

            // On ajoute les installeurs à la collection (l'ordre n'a pas d'importance) 
            Installers.Add(service);
            Installers.Add(process);
        }
    }



}   //fin Namespace
