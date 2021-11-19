using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_SocketExample_Core
{

    public class UDPSocket
    {
        public const int SIO_UDP_CONNRESET = -1744830452;
        Socket _socket;
        const int BufferSize = 1024;

        public UDPSocket(string ipAddress, int portNum)
        {
            _socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _socket.IOControl(
                (IOControlCode)SIO_UDP_CONNRESET,
                new byte[] { 0, 0, 0, 0 },
                null);
            IPAddress parsedIpAddress = IPAddress.Parse(ipAddress);
            IPEndPoint serverEndPoint = new IPEndPoint(parsedIpAddress, portNum);
            _socket.Bind(serverEndPoint);
        }

        public string Listen()
        {
            byte[] receivedBytes = new byte[BufferSize];
            _socket.Receive(receivedBytes);

            string receivedMessage = Encoding.ASCII.GetString(receivedBytes);
            return receivedMessage;
        }

        public void Send(string messageToSend, string ipAddress, int portNum)
        {
            IPAddress serverIpAdress = IPAddress.Parse("127.0.0.1");
            IPEndPoint serverEndPoint = new IPEndPoint(serverIpAdress, portNum);
            byte[] bytesToSend = Encoding.ASCII.GetBytes(messageToSend);
            _socket.SendTo(bytesToSend, serverEndPoint);
        }

        public string Echo()
        {
            byte[] receivedBytes = new byte[BufferSize];
            EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _socket.ReceiveFrom(receivedBytes, ref clientEndPoint);

            // log received message to user
            string receivedMessage = Encoding.ASCII.GetString(receivedBytes);

            return receivedMessage;
        }
    }
}
