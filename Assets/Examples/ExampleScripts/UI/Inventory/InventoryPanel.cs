﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cosmos;
using Cosmos.UI;
namespace Cosmos
{
    /// <summary>
    /// 仅测试
    /// </summary>
    public class InventoryPanel : UILogicResident
    {
        [SerializeField] InventoryDataSet inventoryDataSet;
        public InventoryDataSet InventoryDataSet { get { return inventoryDataSet; } set { inventoryDataSet = value; } }
        LogicEventArgs<InventoryDataSet> uip;
        public LogicEventArgs<InventoryDataSet> Uip
        {
            get
            {
                if (uip == null)
                    uip = new LogicEventArgs<InventoryDataSet>(InventoryDataSet);
                return uip;
            }
        }
        Text txtDescription;
        protected override void OnInitialization()
        {
            GetUIPanel<Button>("BtnLoad").onClick.AddListener(LoadClick);
            GetUIPanel<Button>("BtnQuit").onClick.AddListener(QuitClick);
            GetUIPanel<Button>("BtnSave").onClick.AddListener(SaveClick);
            GetUIPanel<Button>("BtnUpdate").onClick.AddListener(UpdateClick);
            txtDescription = GetUIPanel<Text>("TxtDescription");
            AddUIEventListener(UIImplementCodeParams.UIIMPLEMENT_ITEMDESCRIPTION, UpdateItemHandler);
        }
        protected override void OnTermination()
        {
            GetUIPanel<Button>("BtnLoad").onClick.RemoveAllListeners();
            GetUIPanel<Button>("BtnQuit").onClick.RemoveAllListeners();
            GetUIPanel<Button>("BtnSave").onClick.RemoveAllListeners();
            GetUIPanel<Button>("BtnUpdate").onClick.RemoveAllListeners();
            RemoveUIEventListener(UIImplementCodeParams.UIIMPLEMENT_ITEMDESCRIPTION, UpdateItemHandler);
        }
        private void Start()
        {
            DispatchUIEvent(UIImplementCodeParams.UIIMPLEMENT_UPDATESLOT, this, Uip);
        }
        void LoadClick()
        {
            //这里需要注意，Unity提供的JsonUtility.FromJsonOverwrite方法，官方对这一方法的文档为：
            //it must be a MonoBehaviour, ScriptableObject, or plain class/struct with the Serializable attribute applied

            string json = Facade.Instance.LoadJsonDataFromLocal("Inventory", "InventoryCache.json");
            JsonUtility.FromJsonOverwrite(json, inventoryDataSet);
            Utility.DebugLog("LoadJsonDataFromLocal");
            UpdateClick();
        }
        void SaveClick()
        {
            Facade.Instance.SaveJsonDataToLocal("Inventory", "InventoryCache.json", inventoryDataSet);
            Utility.DebugLog("SaveJsonDataToLocal");
        }
        void QuitClick()
        {
            HidePanel();
        }
        void UpdateClick()
        {
            DispatchUIEvent(UIImplementCodeParams.UIIMPLEMENT_UPDATESLOT, this, Uip);
        }
        void UpdateItemHandler(object sender,GameEventArgs args)
        {
            var stringUip=args as LogicEventArgs<string>;
            txtDescription.text = stringUip.Data;
        }
    }
}