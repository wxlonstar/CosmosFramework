﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cosmos.UI;
namespace Cosmos.Test {
    public class UIEntry : MonoBehaviour
    {
        private void Start()
        {
            var result = Facade.InitMainCanvas("UI/MainUICanvas");
            InitUtility();
        }
        void InitUtility()
        {
            Utility.Json.SetHelper(new JsonUtilityHelper());
        }
    }
}