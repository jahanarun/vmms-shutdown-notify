// The MIT License (MIT)
//
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using static VmOrderedShutdown.AsyncDemo;

namespace VmOrderedShutdown
{
    public class VmOrderedShutdownService : ServiceBase
    {
        private const int SERVICEACCEPTPRESHUTDOWN = 0x100;
        private const int SERVICECONTROLPRESHUTDOWN = 0xf;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public VmOrderedShutdownService()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                try
                {
                    FieldInfo serviceAcceptedCommands = typeof(ServiceBase).GetField("acceptedCommands", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    int v = (int)serviceAcceptedCommands.GetValue(this);
                    int xi = v;
                    serviceAcceptedCommands.SetValue(this, xi | SERVICEACCEPTPRESHUTDOWN);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("vmorderedshutdown", ex.Message, EventLogEntryType.Error, 12100, short.MaxValue);
                }
            }
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                EventLog.WriteEntry("vmorderedshutdown", "vmorderedshutdown service starting", EventLogEntryType.Information, 12100, short.MaxValue);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("vmorderedshutdown", e.Message, EventLogEntryType.Error, 12100, short.MaxValue);
            }
        }

        protected override void OnStop()
        {
            try
            {
                EventLog.WriteEntry("vmorderedshutdown", "vmorderedshutdown service stopping", EventLogEntryType.Information, 12100, short.MaxValue);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("vmorderedshutdown", e.Message, EventLogEntryType.Error, 12100, short.MaxValue);
            }
        }

        protected override void OnCustomCommand(int command)
        {
            try
            {
                if (command == SERVICECONTROLPRESHUTDOWN)
                {
                    ManagementScope basescope = new(@"root\virtualization\v2", null);
                    string query = string.Format("select * from Msvm_ComputerSystem where caption = 'Virtual Machine'");
                    ManagementObjectSearcher searcher = new(basescope, new ObjectQuery(query));
                    ManagementObjectCollection myvms = searcher.Get();

                    foreach (ManagementObject myvm in myvms)
                    {
                        if (IsVmRunning(myvm))
                        {
                            string vmname = myvm["elementname"].ToString();
                            Console.WriteLine("Shutdown " + vmname);
                            AsyncDemo ad = new();
                            AsyncMethodCaller caller = new(ad.ShutdownViaIC);
                            IAsyncResult resbackult = caller.BeginInvoke(vmname, null, null);
                        }
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        ManagementScope ibasescope = new(@"root\virtualization\v2", null);
                        string iquery = string.Format("select * from Msvm_ComputerSystem where caption = 'Virtual Machine'");
                        ManagementObjectSearcher isearcher = new(ibasescope, new ObjectQuery(iquery));
                        ManagementObjectCollection imyvms = isearcher.Get();

                        bool vmState = false;
                        string isrunning = "Running";
                        foreach (ManagementObject myvm in imyvms)
                        {
                            string vmname = myvm["elementname"].ToString();
                            if (IsVmRunning(myvm))
                            {
                                vmState = true;
                                Console.WriteLine("State of " + vmname + " is " + isrunning);
                            }
                            else
                            {
                                isrunning = "Off";
                                Console.WriteLine("State of " + vmname + " is " + isrunning);
                            }
                        }

                        if (!vmState)
                        {
                            i = 10;
                        }

                        Thread.Sleep(5000);
                    }
                    EventLog.WriteEntry("vmorderedshutdown", "StoppedVm's in preshutdown notification", EventLogEntryType.Information, 1210, short.MaxValue);
                }
                else
                {
                    base.OnCustomCommand(command);
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("vmorderedshutdown", e.Message, EventLogEntryType.Error, 12100, short.MaxValue);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private static int ShutdownViaIC(string vmmachinename)
        {
            // Connect to the Remote Machines Management Scope
            try
            {
                ConnectionOptions options = new ConnectionOptions();

                ManagementScope scope = new ManagementScope(@"\\localhost\root\virtualization\V2");
                scope.Connect();

                // Get the msvm_computersystem for the given VM (Vista)

                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope,
                    new ObjectQuery("SELECT * FROM Msvm_ComputerSystem WHERE ElementName = '" + vmmachinename + "'"));

                // Select the first object in the Searcher collection

                IEnumerator enumr = searcher.Get().GetEnumerator();

                enumr.MoveNext();

                ManagementObject msvm_computersystem = (ManagementObject)enumr.Current;

                // Use the association to get the msvm_shutdowncomponent for the msvm_computersystem

                ManagementObjectCollection collection = msvm_computersystem.GetRelated("Msvm_ShutdownComponent");

                ManagementObjectCollection.ManagementObjectEnumerator enumerator = collection.GetEnumerator();

                enumerator.MoveNext();

                ManagementObject msvm_shutdowncomponent = (ManagementObject)enumerator.Current;

                // Get the InitiateShudown Parameters

                ManagementBaseObject inParams = msvm_shutdowncomponent.GetMethodParameters("InitiateShutdown");

                inParams["Force"] = true;

                inParams["Reason"] = "Need to Shutdown";

                // Invoke the Method

                ManagementBaseObject outParams = msvm_shutdowncomponent.InvokeMethod("InitiateShutdown", inParams, null);

                uint returnValue = (uint)outParams["ReturnValue"];

                // Zero indicates success

                if (returnValue != 0)
                {
                    Console.WriteLine("SHUTDOWN Failed");
                }

                return 1;
            }
            catch
            {
                return 0;
            }
        }

        private static bool IsVmRunning(ManagementObject vm)
        {
            const int Enabled = 2;
            bool running = false;
            ushort operationStatus = (ushort)vm["EnabledState"];

            if (operationStatus == Enabled)
            {
                running = true;
            }
            return running;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "vmorderedshutdown";
        }
    }
}