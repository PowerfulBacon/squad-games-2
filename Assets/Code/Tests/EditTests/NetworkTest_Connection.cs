using System;
using System.Net.Sockets;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace squad_games_2.Assets.Code.Tests.EditTests
{
    public class NetworkTest_Connection
    {
        
        [Test]
        public void TestNetworkConnection()
        {
            //Ignore the error message printed when we shut down the server.
            LogAssert.ignoreFailingMessages = true;
            //Create the host and client and connect
            NetworkHost host = new NetworkHost(NetworkBase.DEFAULT_SERVER_PORT);
            NetworkClient client = new NetworkClient(NetworkBase.DEFAULT_SERVER_PORT);
            try
            {
                client.AttemptConnect(TestUtility.GetIPAddress(), "test", null, null);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                //Shutdown
                host.Shutdown();
                client.Shutdown();
                //Failure
                Assert.Fail("Error: Attempting to connect failed");
                return;
            }
            Thread.Sleep(300);
            //Test
            Assert.AreEqual(true, client.isConnected);
            Assert.AreEqual(1, host.connectedClients.Count);
            //Shutdown
            host.Shutdown();
            client.Shutdown();
        }

    }
}