﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cosmos;
using Cosmos.UI;
using Cosmos.Network;
using System.IO;
namespace Cosmos.Test
{
    public class KCPNetworkPanel : UIResidentForm
    {
        Button btnConnect;
        Button btnDisconnect;
        Button btnSend;
        InputField inputMsg;
    
        protected override void OnInitialization()
        {
            btnConnect = GetUIForm<Button>("BtnConnect");
            btnConnect.onClick.AddListener(ConnectClick);
            btnDisconnect = GetUIForm<Button>("BtnDisconnect");
            btnDisconnect.onClick.AddListener(DisconnectClick);
            btnSend = GetUIForm<Button>("BtnSend");
            btnSend.onClick.AddListener(SendClick);
            inputMsg = GetUIForm<InputField>("InputMsg");
        }
        void ConnectClick()
        {
            KCPNetwork.Instance.Connect();
        }
        void DisconnectClick()
        {
            KCPNetwork .Instance.Disconnect();
        }
        void SendClick()
        {
            string str = inputMsg.text;
            KCPNetwork .Instance.SendKcpMessage(str);
        }
    }
}