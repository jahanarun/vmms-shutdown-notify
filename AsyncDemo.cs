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
using System.Management;

namespace VmOrderedShutdown
{
    public class AsyncDemo
    {
        public string ShutdownViaIC(string vmmachinename)
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

                return "1";
            }
            catch
            {
                return "0";
            }
        }

        public delegate string AsyncMethodCaller(string vmmachinename);
    }
}