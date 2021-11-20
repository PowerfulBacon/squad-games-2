using System;
using System.Text;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace squad_games_2.Assets.Code.Tests.EditTests
{
    public class NetworkTest_MultipleClients
    {
        
        [Test]
        public void TestNetworkConnection()
        {
            //Ignore the error message printed when we shut down the server.
            LogAssert.ignoreFailingMessages = true;
            //Create the host and client and connect
            NetworkHost host = new NetworkHost(NetworkBase.DEFAULT_SERVER_PORT);
            NetworkClient client1 = new NetworkClient(NetworkBase.DEFAULT_SERVER_PORT);
            NetworkClient client2 = new NetworkClient(NetworkBase.DEFAULT_SERVER_PORT);
            try
            {
                client1.AttemptConnect(TestUtility.GetIPAddress(), "test", null, null);
                client2.AttemptConnect(TestUtility.GetIPAddress(), "test", null, null);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                //Shutdown
                host.Shutdown();
                client1.Shutdown();
                client2.Shutdown();
                //Failure
                Assert.Fail("Error: Attempting to connect failed");
                return;
            }
            Thread.Sleep(100);
            //Send a bunch of messages
            byte[] message = Encoding.UTF8.GetBytes("TEST");
            for(int i = 0; i < 100; i ++)
            {
                //Make sure that messages from both clients are handled
                client1.udpClient.Send(message, message.Length);
                client2.udpClient.Send(message, message.Length);
            }
            Thread.Sleep(300);
            //Test
            Assert.AreEqual(true, client1.isConnected);
            Assert.AreEqual(true, client2.isConnected);
            Assert.AreEqual(2, host.connectedClients.Count);
            Assert.AreEqual(202, host.messagesReceived);
            //Shutdown.
            host.Shutdown();
            client1.Shutdown();
            client2.Shutdown();
        }

    }
}