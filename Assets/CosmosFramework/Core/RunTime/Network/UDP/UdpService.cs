﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Cosmos;
using Cosmos.Reference;
using TMPro;

namespace Cosmos.Network
{
    /// <summary>
    /// UDP socket服务；
    /// 这里管理其他接入的远程对象；
    /// </summary>
    public class UdpService : INetworkService
    {
        /// <summary>
        /// 对象IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 对象端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 由服务器分配的会话ID
        /// </summary>
        public uint Conv { get; protected set; } = 0;
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Available { get; protected set; } = false;
        /// <summary>
        /// udpSocket对象
        /// </summary>
        protected UdpClient udpSocket;
        /// <summary>
        /// IP对象；
        /// </summary>
        protected IPEndPoint serverEndPoint;
        protected ConcurrentQueue<UdpReceiveResult> awaitHandle = new ConcurrentQueue<UdpReceiveResult>();
        public UdpService()
        {
            //构造传入0表示接收任意端口收发的数据
            udpSocket = new UdpClient(0);
        }
        /// <summary>
        /// 非空虚函数；
        /// 开启这个服务；
        /// </summary>
        public virtual void OnActive()
        {
            Available = true;
        }
        /// <summary>
        /// 非空虚函数；
        /// 关闭这个服务；
        /// </summary>
        public virtual void OnDeactive()
        {
            Available = false;
            Conv = 0;
        }
        /// <summary>
        /// 异步接收网络消息接口
        /// </summary>
        public virtual async void OnReceive()
        {
            if (!Available)
                return;
            if (udpSocket != null)
            {
                try
                {
                    UdpReceiveResult result = await udpSocket.ReceiveAsync();
                    awaitHandle.Enqueue(result);
                }
                catch (Exception e)
                {
                    Utility.Debug.LogError($"网络消息接收异常：{e}");
                }
            }
        }
        /// <summary>
        /// 非空虚函数;
        /// 发送报文信息；
        /// 发送给特定的endpoint对象，可不局限于一个服务器点；
        /// </summary>
        /// <param name="netMsg">消息体</param>
        /// <param name="endPoint">远程对象</param>
        public virtual async void SendMessage(INetworkMessage netMsg, IPEndPoint endPoint)
        {
            if (!Available)
                return;
            UdpNetworkMessage udpNetMsg = netMsg as UdpNetworkMessage;
            udpNetMsg.Conv = Conv;
            Utility.Debug.LogInfo($"发送网络消息 : Conv : {udpNetMsg.Conv} ; Cmd {udpNetMsg.Cmd} ; OperationCode:{udpNetMsg.OperationCode}");
            if (udpSocket != null)
            {
                try
                {
                    var buffer = udpNetMsg.GetBuffer();
                    int length = await udpSocket.SendAsync(buffer, buffer.Length, endPoint);
                    if (length != buffer.Length)
                    {
                        //消息未完全发送，则重新发送
                        SendMessage(udpNetMsg, endPoint);
                    }
                }
                catch (Exception e)
                {
                    Utility.Debug.LogError($"发送异常:{e.Message}");
                }
            }
        }
        /// <summary>
        /// 非空虚函数
        /// </summary>
        /// <param name="netMsg">消息体</param>
        public virtual void SendMessage(INetworkMessage netMsg)
        {
            SendMessage(netMsg, serverEndPoint);
        }
        /// <summary>
        /// 空虚函数；
        /// 轮询更新;
        /// </summary>
        public virtual void OnRefresh(){}
    }
}
