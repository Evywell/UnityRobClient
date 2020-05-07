using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

namespace Network
{
    public class Client : MonoBehaviour
    {
        public static Client instance;
        public static int dataBufferSize = 4096;

        public string ip = "192.168.1.19";
        public int port = 8888;
        public int myId = 0;
        public TCPClient tcpClient;

        public void ConnectToServer()
        {
            tcpClient.Connect();
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Awake Client, destroying the object...");
                Destroy(this);
            }
        }

        void Start()
        {
            tcpClient = new TCPClient();
        }

        public class TCPClient
        {
            public TcpClient socket;

            private NetworkStream stream;
            private byte[] receveidBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receveidBuffer = new byte[dataBufferSize];
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }

            public void SendData(string packet)
            {
                try
                {
                    if (socket != null)
                    {
                        byte[] buffer = Encoding.ASCII.GetBytes(packet);
                        stream.BeginWrite(buffer, 0, buffer.Length, null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Debug.Log($"Error sending data to server via TCP: {_ex}");
                }
            }

            private void ConnectCallback(IAsyncResult _result)
            {
                socket.EndConnect(_result);

                if (!socket.Connected)
                {
                    return;
                }

                stream = socket.GetStream();
                stream.BeginRead(receveidBuffer, 0, dataBufferSize, ReceiveCallback, null);

                this.SendData("[0,{\"login\":\"admin\",\"password\":\"admin\"}]\n");
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receveidBuffer, _data, _byteLength);

                    // Here is the data
                    Debug.Log(Encoding.UTF8.GetString(_data, 0, _data.Length));

                    stream.BeginRead(receveidBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch
                {
                    // Disconnect
                }
            }

        }

    }
}

