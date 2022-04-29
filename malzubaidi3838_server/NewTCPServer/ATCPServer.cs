using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using logg_server;
using System.Threading;

namespace NewTCPServer
{
    public partial class ATCPServer : ServiceBase
    {
        Listener listener = null;
        Thread t = null;
        public ATCPServer()
        {
            InitializeComponent();
            listener = new Listener();
        }

        // ------------------------------------------------------------
        // Function: OnStart
        // Description:it starts the service by passing the server as an argument to the thread and pass the starting messsage to the logger. 
        // Parameters: args that are never used.
        // Returns: nothing.
        // ------------------------------------------------------------
        protected override void OnStart(string[] args)
        {

            t = new Thread(new ThreadStart(listener.Listen));
            t.Start();
        }

        // ------------------------------------------------------------
        // Function: OnStop
        // Description: it stops the service by setting the variable that the loop genrates based on it's value to false, thus it stops the server from running.
        // Parameters: nothing.
        // Returns: nothing.
        // ------------------------------------------------------------
        protected override void OnStop()
        {
            listener.Run = false;
            t.Join();
        }
    }
}
