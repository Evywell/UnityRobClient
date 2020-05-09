using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using SimpleJSON;
using Opcode;
using Unity.UNetWeaver;

namespace Network
{
    public class Client : MonoBehaviour
    {
        public static int dataBufferSize = 4096;

        public string ip = "192.168.1.19";
        public int port = 8888;
        public TCPClient tcpClient;

        private Packet _authenticationPacket;

        public void ConnectToServer()
        {
            tcpClient.Connect();
        }

        public void SetAuthenticationPacket(Packet packet)
        {
            _authenticationPacket = packet;
        }

        public void IsSuccessfullyConnected()
        {
            tcpClient.SendData(_authenticationPacket);
        }

        private void Start()
        {
            tcpClient = new TCPClient(this, new AuthOpcodeHandler());
        }

        public class TCPClient
        {
            public TcpClient socket;

            private readonly OpcodeHandler handler;
            private readonly Client client;
            private NetworkStream stream;
            private byte[] receveidBuffer;

            public TCPClient(Client client, OpcodeHandler handler)
            {
                this.handler = handler;
                this.client = client;
                handler.RegisterHandler();
            }

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receveidBuffer = new byte[dataBufferSize];
                socket.BeginConnect(client.ip, client.port, ConnectCallback, socket);
            }

            public void SendData(Packet packet)
            {
                SendData(packet.ToString());
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

                client.IsSuccessfullyConnected();
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

                    ThreadManager.ExecuteOnMainThread(() => {
                        string textPacket = Encoding.UTF8.GetString(_data, 0, _data.Length);
                        JSONNode node = JSON.Parse(textPacket);
                        int opcode = node[0];

                        int leftPart = 3;
                        if (opcode > 9)
                        {
                            leftPart = (int)Math.Floor(Math.Log10(opcode)) + 3;
                        }
                        string payload = textPacket.Substring(leftPart, textPacket.Length - leftPart - 1);
                        handler.Handle(opcode, payload);
                    });
                
                    stream.BeginRead(receveidBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception e)
                {
                    // Disconnect
                    Debug.Log(e.Message);
                }
            }

        }

    }
}

