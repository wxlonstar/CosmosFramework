﻿using Cosmos.Network;
using Cosmos.Reference;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Cosmos
{
    public class UdpClientService : UdpService
    {
        /// <summary>
        /// 轮询委托
        /// </summary>
        UdpClientPeer peer;
        public UdpClientService():base()
        {
            //构造传入0表示接收任意端口收发的数据
            peer = new UdpClientPeer(OnConnectHandler,OnDisconnectHandler);
            peer.SetValue(SendMessageAsync, Disconnect, 0, null);
        }
        void OnConnectHandler()
        {
            onConnect?.Invoke();
        }
        void OnDisconnectHandler()
        {
            onDisconnect?.Invoke();
        }
        public override void SetHeartbeat(IHeartbeat heartbeat)
        {
            peer.Heartbeat = heartbeat;
        }
        public override void Connect(string ip, int port)
        {
            Conv = 0;
            serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            UdpNetMessage udpNetMsg = UdpNetMessage.EncodeMessage(InnerOpCode._Connect, null);
            udpNetMsg.Cmd = KcpProtocol.SYN;
            SendMessageAsync(udpNetMsg);
        }
        public override void Disconnect()
        {
            UdpNetMessage udpNetMsg = UdpNetMessage.EncodeMessage(InnerOpCode._Disconnect, null);
            udpNetMsg.Cmd = KcpProtocol.FIN;
            SendMessageAsync(udpNetMsg);
        }
        public override void SendMessageAsync(INetworkMessage netMsg)
        {
            UdpNetMessage udpNetMsg = netMsg as UdpNetMessage;
            var result = peer.EncodeMessage(ref udpNetMsg);
            if (result)
            {
               SendMessageAsync(udpNetMsg, serverEndPoint);
            }
            else
                Utility.DebugError("INetworkMessage 消息编码失败");
        }
        public override void OnRefresh()
        {
            OnReceive();
            peer.OnRefresh();
            if (awaitHandle.Count > 0)
            {
                UdpReceiveResult data;
                if (awaitHandle.TryDequeue(out data))
                {
                    UdpNetMessage netMsg = Facade.SpawnReference<UdpNetMessage>();
                    netMsg.DecodeMessage(data.Buffer);
                    if (Conv == 0)
                    {
                        Conv = netMsg.Conv;
                        peer.Conv = Conv;
                        peer.AllocateConv(Conv);
                    }
                    if (netMsg.IsFull)
                    {
                        peer.MessageHandler(netMsg);
                    }
                }
            }
        }
    }
}
