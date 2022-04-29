// Listener.cs
// This is the source file were the server resides to handle the client's requests.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace logg_server
{
    class Listener
    {
        int port = int.Parse(ConfigurationManager.AppSettings.Get("port"));
        IPAddress ipAddress = IPAddress.Parse(ConfigurationManager.AppSettings.Get("ipAddress"));
        TcpListener server = null;
        int BUFFERSIZE = 1024;
        public volatile bool Run = true;
        string logFile = ConfigurationManager.AppSettings.Get("logFile");
        string date = ConfigurationManager.AppSettings.Get("Date");
        string Time = ConfigurationManager.AppSettings.Get("Time");
        string UserName = ConfigurationManager.AppSettings.Get("UserName");
        string EventID = ConfigurationManager.AppSettings.Get("EventID");
        string Type = ConfigurationManager.AppSettings.Get("Type");
        string Msg = ConfigurationManager.AppSettings.Get("Msg");
        string clientName = null;
        int c = 0;
        int reqC = 0;
        // ds contains the name of the client
        Stopwatch stopWatch = new Stopwatch();
        List<string> list = new List<string>();
        List<string> dangerousClients = new List<string>();
        // ------------------------------------------------------------
        // Function: Listen().
        // Description: it starts the server and recieves the client's requests then it inserts these requests into the log file.
        // Parameters: none
        // Returns: none
        // ------------------------------------------------------------
        public void Listen()
        {
            try
            {
                server = new TcpListener(ipAddress, port);
                server.Start();


                byte[] bytes = new byte[BUFFERSIZE];

                while (Run)
                {
                    if (server.Pending())
                    {

                        TcpClient client = server.AcceptTcpClient();
                        c++;

                        NetworkStream stream = client.GetStream();

                        int i;

                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            reqC++;

                            if (!File.Exists(logFile))
                            {
                                StreamWriter sw = File.CreateText(logFile);
                                string logFormat = date + "\t\t\t" + Time + "\t\t\t\t" + Type + "\t" + UserName + "\t" + EventID + "\t\t\t " + Msg;
                                sw.WriteLine(logFormat);
                                sw.Close();
                            }

                            string container = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                            clientName = container.Split(':')[0];
                            if (dangerusClientChecker(stream, client, clientName) == true)
                            {
                                break;
                            }

                            // insert the name of the client to the ds
                            list.Add(clientName);


                            // check to see how many times the client had made reuqets during the last second
                            int clientOccurances = 0;
                            foreach (string s in list)
                            {
                                if (s == clientName)
                                {
                                    clientOccurances++;
                                }
                            }
                            if (clientOccurances == 1)
                            {
                                stopWatch.Start();
                            }

                            // if the client had made 10 requetsts or more and it has not been a second, then don't fullfill that client's requests anymore.
                            if (clientOccurances >= 10 && stopWatch.ElapsedMilliseconds < 1000)
                            {

                                list.Clear();
                                stopWatch.Restart();
                                stream.Close();
                                client.Close();
                                dangerousClients.Add(clientName);
                                break;
                            }
                            else
                            {
                                DateTime dateTime = DateTime.UtcNow.Date;
                                Random rnd = new Random();
                                int num = rnd.Next();
                                string clientMsg = container.Split(':').Last();
                                string msgLevel = container.Split(':')[1];
                                writeToFile(logFile, dateTime.ToString("d"), DateTime.Now.ToString("h:mm:ss tt"), msgLevel, clientMsg, clientName, num.ToString());
                                stream.Close();
                                client.Close();

                                if (stopWatch.ElapsedMilliseconds >= 1000)
                                {
                                    list.Clear();
                                    stopWatch.Restart();
                                }
                                break;
                            }


                        }
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }

                }
            }

            catch (SocketException ex)
            {
                if (!File.Exists("Exceptions.txt"))
                {
                    StreamWriter sw = File.CreateText("Exceptions.txt");
                    sw.WriteLine(ex.Message);
                    sw.Close();
                }
                else
                {
                    using (StreamWriter sw = File.AppendText("Exceptions.txt"))
                    {
                        sw.WriteLine(ex.Message);
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists("Exceptions.txt"))
                {
                    StreamWriter sw = File.CreateText("Exceptions.txt");
                    sw.WriteLine(ex.Message);
                    sw.Close();
                }
                else
                {
                    using (StreamWriter sw = File.AppendText("Exceptions.txt"))
                    {
                        sw.WriteLine(ex.Message);
                        sw.Close();
                    }
                }
            }
            finally
            {
                if (server != null)
                {
                    server.Stop();
                }
            }
        }


        // ------------------------------------------------------------
        // Function: writeToFile
        // Description: it inserts the incomming logg messages from the client to the log file.
        // Parameters: string logFile, string date, string Time, string Type, string clientMsg, string UserName, string EventID.
        // Returns: nothing.
        // ------------------------------------------------------------
        public void writeToFile(string logFile, string date, string Time, string Type, string clientMsg, string UserName, string EventID)
        {
            using (StreamWriter sw = File.AppendText(logFile))
            {
                string clientMsgWithoutCarriage = clientMsg.Remove(clientMsg.Length - 2, 2);
                sw.WriteLine(date + "\t\t" + Time + "\t\t\t" + Type + "\t\t" + UserName + "\t\t" + EventID + "\t\t" + clientMsgWithoutCarriage);
                sw.Close();
            }
        }


        // ------------------------------------------------------------
        // Function: dangerusClientChecker
        // Description: it searches through the ds that holds the dangerous clients names, if that's the case then returns true.
        // Parameters: NetworkStream stream, TcpClient client, string clientName
        // Returns: bool check.
        // ------------------------------------------------------------
        public bool dangerusClientChecker(NetworkStream stream, TcpClient client, string clientName)
        {
            bool check = false;
            foreach (string dangerousClient in dangerousClients)
            {
                if (clientName == dangerousClient)
                {
                    check = true;

                    stream.Close();
                    client.Close();
                    break;
                }
            }
            return check;
        }
    }
}
