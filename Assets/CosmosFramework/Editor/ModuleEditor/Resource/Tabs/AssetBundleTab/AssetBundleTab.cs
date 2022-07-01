﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using System.IO;

namespace Cosmos.Editor.Resource
{
    public class AssetBundleTab
    {
        public Func<EditorCoroutine> BuildDataset;
        public const string AssetBundleTabDataName = "ResourceEditor_AsseBundleTabData.json";
        AssetBundleTabData tabData;
        Vector2 scrollPosition;

        public void OnEnable()
        {
            GetTabData();
        }
        public void OnDisable()
        {
            SaveTabData();
        }
        public void OnGUI(Rect rect)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DrawBuildOptions();
            GUILayout.Space(16);
            DrawEncryption();
            GUILayout.Space(16);
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Build assetBundle"))
                {

                }
                if (GUILayout.Button("Reset options"))
                {
                    tabData = new AssetBundleTabData();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
        public void OnDatasetAssign()
        {

        }
        public void OnDatasetUnassign()
        {

        }
        void DrawBuildOptions()
        {
            EditorGUILayout.LabelField("Build Options", EditorStyles.boldLabel);
            bool versionValid = false;
            EditorGUILayout.BeginVertical();
            {
                tabData.BuildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build target", tabData.BuildTarget);
                tabData.BuildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumPopup("Build assetBundle options", tabData.BuildAssetBundleOptions);
                tabData.BuildVersion = EditorGUILayout.TextField("Build version", tabData.BuildVersion);

                EditorGUILayout.BeginHorizontal();
                {
                    tabData.BuildPath = EditorGUILayout.TextField("Build path", tabData.BuildPath.Trim());
                    if (GUILayout.Button("Browse", GUILayout.MaxWidth(128)))
                    {
                        var newPath = EditorUtility.OpenFolderPanel("Bundle output path", tabData.BuildPath, string.Empty);
                        if (!string.IsNullOrEmpty(newPath))
                        {
                            tabData.BuildPath = newPath.Replace("\\", "/");
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                versionValid = !string.IsNullOrEmpty(tabData.BuildVersion);
                if (versionValid)
                {
                    var version = tabData.BuildVersion.Replace(".", "_");
                    tabData.AssetBundleBuildPath = Utility.IO.WebPathCombine(tabData.BuildPath, tabData.BuildTarget.ToString(), version);
                }
                else
                    EditorGUILayout.HelpBox("BuildVersion is invalid !", MessageType.Error);
                EditorGUILayout.LabelField("AssetBundle build path", tabData.AssetBundleBuildPath);
            }
            EditorGUILayout.EndVertical();


        }
        void DrawEncryption()
        {
            EditorGUILayout.LabelField("Encryption Options", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            {
                tabData.BuildedAssetsEncryption = EditorGUILayout.ToggleLeft("Builded assets encryption", tabData.BuildedAssetsEncryption);
                if (tabData.BuildedAssetsEncryption)
                {
                    tabData.BuildIedAssetsEncryptionKey = EditorGUILayout.TextField("Builded assets encryption key", tabData.BuildIedAssetsEncryptionKey);
                    var aesKeyStr = tabData.BuildIedAssetsEncryptionKey;
                    var aesKeyLength = Encoding.UTF8.GetBytes(aesKeyStr).Length;
                    EditorGUILayout.LabelField($"Builded assets AES encryption key, key should be 16,24 or 32 bytes long, current key length is : {aesKeyLength} ");
                    if (aesKeyLength != 16 && aesKeyLength != 24 && aesKeyLength != 32 && aesKeyLength != 0)
                    {
                        EditorGUILayout.HelpBox("Key should be 16,24 or 32 bytes long", MessageType.Error);
                    }
                    //打包输出的资源加密，如buildInfo，assetbundle 文件名加密
                    tabData.BuildedAssetNameType = (BuildedAssetNameType)EditorGUILayout.EnumPopup("Builded assets name type ", tabData.BuildedAssetNameType);
                    GUILayout.Space(16);
                }
                tabData.AssetBundleEncryption = EditorGUILayout.ToggleLeft("AssetBundle encryption", tabData.AssetBundleEncryption);
                if (tabData.AssetBundleEncryption)
                {
                    EditorGUILayout.LabelField("AssetBundle encryption offset");
                    tabData.AssetBundleOffsetValue = EditorGUILayout.IntField("Encryption offset", tabData.AssetBundleOffsetValue);
                    if (tabData.AssetBundleOffsetValue < 0)
                        tabData.AssetBundleOffsetValue = 0;
                }
            }
            EditorGUILayout.EndVertical();
        }
        void GetTabData()
        {
            try
            {
                tabData = EditorUtil.GetData<AssetBundleTabData>(AssetBundleTabDataName);
            }
            catch
            {
                tabData = new AssetBundleTabData();
                EditorUtil.SaveData(AssetBundleTabDataName, tabData);
            }
        }
        void SaveTabData()
        {
            EditorUtil.SaveData(AssetBundleTabDataName, tabData);
        }
    }
}
