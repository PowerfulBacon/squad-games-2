using System;
using System.Text;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace squad_games_2.Assets.Code.Tests.EditTests
{
    public class NetworkTest_BadConnection
    {
        
        [Test]
        public void TestExploitPackets()
        {
            //Ignore the error message printed when we shut down the server.
            LogAssert.ignoreFailingMessages = true;
            //Create the host and client and connect
            NetworkHost host = new NetworkHost(NetworkBase.DEFAULT_SERVER_PORT);
            NetworkClient client = new NetworkClient(NetworkBase.DEFAULT_SERVER_PORT);
            
            //BAD CODE SIMULATION
            //ATTEMPTS TO JOIN THE SERVER AND SEND EXPLOIT PACKETS
            //Also simulates clients getting kicked, should be prevented from sending data
            byte[] badMessage = Encoding.UTF8.GetBytes("exploit packet");
            client.udpClient.Connect(TestUtility.GetIPAddress(), NetworkBase.DEFAULT_SERVER_PORT);
            client.udpClient.Send(badMessage, badMessage.Length);

            //Sleep a bit
            Thread.Sleep(300);
            //Test
            Assert.AreEqual(0, host.connectedClients.Count);
            Assert.AreEqual(0, host.messagesReceived);
            //Shutdown
            host.Shutdown();
            client.Shutdown();
        }

    }
}