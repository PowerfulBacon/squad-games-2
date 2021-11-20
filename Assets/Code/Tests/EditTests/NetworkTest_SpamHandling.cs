using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace squad_games_2.Assets.Code.Tests.EditTests
{
    public class NetworkTest_SpamHandling
    {
        
        [Test]
        public void TestSpamHandling()
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
            Thread.Sleep(100);
            //Send a bunch of messages
            for(int i = 0; i < 500; i ++)
            {
                host.connectedClients[host.connectedClients.Keys.ElementAt(0)].SendMessage(host.udpClient, "TEST");
            }
            //It shouldn't be equal at this point since they are still being handled.
            Assert.AreNotEqual(501, client.messagesReceived);
            Thread.Sleep(300);
            //Test
            Assert.AreEqual(true, client.isConnected);
            Assert.AreEqual(1, host.connectedClients.Count);
            Assert.AreEqual(501, client.messagesReceived);
            //Shutdown
            host.Shutdown();
            client.Shutdown();
        }

    }
}