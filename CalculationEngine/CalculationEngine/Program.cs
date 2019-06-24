using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

//TODO le calculation engine va chercher les datas dans la base metric puis les calcul/transforme puis les stockes dans la bases 

namespace CalculationEngine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Calculation_Engine()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}



//https://www.codeproject.com/Questions/776623/How-to-call-java-api-in-net-application
//https://jormes.developpez.com/articles/services-windows-dotnet/
