﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cosmos;
using Cosmos.UI;
using System.IO;

public class NetworkPanel : UILogicResident
{
    Button btnConnect;
    Button btnDisconnect;
    Button btnSend;
    Text info;
    InputField inputMsg;
    [SerializeField] string ip = "127.0.0.1";
    [SerializeField] int port = 8511;
    protected override void OnInitialization()
    {
        btnConnect = GetUIPanel<Button>("BtnConnect");
        btnConnect.onClick.AddListener(ConnectClick);
        btnDisconnect = GetUIPanel<Button>("BtnDisconnect");
        btnDisconnect.onClick.AddListener(DisconnectClick);
        btnSend = GetUIPanel<Button>("BtnSend");
        btnSend.onClick.AddListener(SendClick);
        info = GetUIPanel<Text>("Info");
        inputMsg = GetUIPanel<InputField>("InputMsg");
    }
    void ConnectClick()
    {
        Facade.NetworkConnect(ip,port,System.Net.Sockets.ProtocolType.Udp);
    }
    void DisconnectClick()
    {
        Facade.NetworkDisconnect();
    }
    void SendClick()
    {
        string str = inputMsg.text;
        var data= Utility.Encode.ConvertToByte(str);
        UdpNetworkMessage msg = new UdpNetworkMessage(0,0,KcpProtocol.MSG,0,data);
        Facade.SendNetworkMessage(msg);
    }
}
