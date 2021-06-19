using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Assets.script.Mir.log;
using C = ClientPackets;


namespace Client.MirNetwork
{
    public static class MirNetwork
    {
        private const string tag = "Network";

        private static TcpClient _client;
        public static int ConnectAttempt = 0;
        public static bool Connected;
        public static DateTime TimeOutTime, TimeConnected;

        private static ConcurrentQueue<Packet> _receiveList;
        private static ConcurrentQueue<Packet> _sendList;

        static byte[] _rawData = new byte[0];
        static readonly byte[] _rawBytes = new byte[8 * 1024];

        public static ProcessPacket loginScens;

        public static ProcessPacket gameScens;

        public static void Connect()
        {
            if (_client != null)
                Disconnect();

            ConnectAttempt++;

            _client = new TcpClient { NoDelay = true };
            _client.BeginConnect(Settings.IPAddress, Settings.Port, Connection, null);

        }

        private static void Connection(IAsyncResult result)
        {
            try
            {
                _client.EndConnect(result);

                if (!_client.Connected)
                {
                    Connect();
                    return;
                }
                _receiveList = new ConcurrentQueue<Packet>();
                _sendList = new ConcurrentQueue<Packet>();
                _rawData = new byte[0];
                TimeConnected = DateTime.Now;
                TimeOutTime = TimeConnected + TimeSpan.FromSeconds(5);
                BeginReceive();
            }
            catch (SocketException ex)
            {
                LogUtil.log(tag, ex.ToString());
                Connect();
            }
            catch (Exception ex)
            {
                LogUtil.log(tag, ex.ToString());
                Disconnect();
            }
        }

        private static void BeginReceive()
        {
            if (_client == null || !_client.Connected) return;

            try
            {

                _client.Client.BeginReceive(_rawBytes, 0, _rawBytes.Length, SocketFlags.None, ReceiveData, _rawBytes);
            }
            catch (Exception ex)
            {

                Disconnect();
            }
        }
        private static void ReceiveData(IAsyncResult result)
        {
            if (_client == null || !_client.Connected) return;

            int dataRead;

            try
            {
                dataRead = _client.Client.EndReceive(result);
            }
            catch (Exception ex)
            {
                LogUtil.log(tag, "ReceiveData " + ex.ToString());
                Disconnect();
                return;
            }

            if (dataRead == 0)
            {
                Disconnect();
            }

            byte[] rawBytes = result.AsyncState as byte[];

            byte[] temp = _rawData;
            _rawData = new byte[dataRead + temp.Length];
            Buffer.BlockCopy(temp, 0, _rawData, 0, temp.Length);
            Buffer.BlockCopy(rawBytes, 0, _rawData, temp.Length, dataRead);

            Packet p;
            List<byte> data = new List<byte>();

            while ((p = Packet.ReceivePacket(_rawData, out _rawData)) != null)
            {
                _receiveList.Enqueue(p);
                data.AddRange(p.GetPacketBytes());
            }



            BeginReceive();
        }

        private static void BeginSend(List<byte> data)
        {
            if (_client == null || !_client.Connected || data.Count == 0) return;

            try
            {
                _client.Client.BeginSend(data.ToArray(), 0, data.Count, SocketFlags.None, SendData, null);
            }
            catch
            {
                Disconnect();
            }
        }
        private static void SendData(IAsyncResult result)
        {
            try
            {
                _client.Client.EndSend(result);
            }
            catch
            { }
        }


        public static void Disconnect()
        {
            if (_client == null) return;

            _client.Close();

            TimeConnected = DateTime.MinValue;
            Connected = false;
            _sendList = null;
            _client = null;

            _receiveList = null;
        }

        public static void Process()
        {
            if (_client == null || !_client.Connected)
            {
                if (Connected)
                {
                    while (_receiveList != null && !_receiveList.IsEmpty)
                    {
                        if (!_receiveList.TryDequeue(out Packet p) || p == null) continue;
                        if (!(p is ServerPackets.Disconnect) && !(p is ServerPackets.ClientVersion)) continue;

                        ProcessPacket(p);
                        _receiveList = null;
                        return;
                    }


                    LogUtil.log(tag, "Lost connection with the server.");
                    Disconnect();
                    return;
                }
                return;
            }

            if (!Connected && TimeConnected > DateTime.MinValue && DateTime.Now > TimeConnected + TimeSpan.FromSeconds(5))
            {
                Disconnect();
                Connect();
                return;
            }



            while (_receiveList != null && !_receiveList.IsEmpty)
            {
                if (!_receiveList.TryDequeue(out Packet p) || p == null) continue;
                ProcessPacket(p);
            }


            if (DateTime.Now > TimeOutTime && _sendList != null && _sendList.IsEmpty)
                _sendList.Enqueue(new C.KeepAlive());

            if (_sendList == null || _sendList.IsEmpty) return;

            TimeOutTime = DateTime.Now + TimeSpan.FromSeconds(5);

            List<byte> data = new List<byte>();
            while (!_sendList.IsEmpty)
            {
                if (!_sendList.TryDequeue(out Packet p)) continue;
                data.AddRange(p.GetPacketBytes());
            }



            BeginSend(data);
        }

        private static void ProcessPacket(Packet p)
        {
            LogUtil.log(tag, p.ToString());
            if (loginScens != null)
                loginScens.process(p);
            if (gameScens != null)
                gameScens.process(p);
        }

        public static void Enqueue(Packet p)
        {
            if (_sendList != null && p != null)
                _sendList.Enqueue(p);
        }
    }


    public interface ProcessPacket
    {


        void process(Packet p);
    }
}
