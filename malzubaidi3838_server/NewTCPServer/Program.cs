/*
  FILE : Program.cs
* PROJECT :  Assignment 6#
* PROGRAMMER : Mahmood Al-Zubaidi
* FIRST VERSION : 25/Nov/2021
* DESCRIPTION : The purpose of this function is to demonstrate the use of services.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NewTCPServer
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
                new ATCPServer()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
