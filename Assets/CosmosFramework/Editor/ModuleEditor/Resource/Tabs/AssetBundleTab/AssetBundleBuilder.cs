﻿using Cosmos.Resource;
using UnityEditor;
using UnityEngine;
using System.IO;
namespace Cosmos.Editor.Resource
{
    /// <summary>
    /// AB打包构建器
    /// </summary>
    public class AssetBundleBuilder
    {
        public void PrepareBuildAssetBundle(AssetBundleBuildParams buildParams, ResourceDataset dataset, ref ResourceManifest resourceManifest)
        {
            if (Directory.Exists(buildParams.AssetBundleBuildPath))
                Utility.IO.DeleteFolder(buildParams.AssetBundleBuildPath);
            Directory.CreateDirectory(buildParams.AssetBundleBuildPath);

            var assetBundleNameType = buildParams.AssetBundleNameType;

            var bundles = dataset.ResourceBundleList;
            var bundleLength = bundles.Count;

            for (int i = 0; i < bundleLength; i++)
            {
                var bundle = bundles[i];
                var importer = AssetImporter.GetAtPath(bundle.BundlePath);
                var bundleName = string.Empty;
                //这里获取绝对ab绝对路径下，所有资源的bytes，生成唯一MD5 hash
                var path = Path.Combine(EditorUtil.ApplicationPath(), bundle.BundlePath);
                var hash = ResourceUtility.CreateDirectoryMd5(path);
                switch (assetBundleNameType)
                {
                    case AssetBundleNameType.DefaultName:
                        {
                            bundleName = bundle.BundleName;
                        }
                        break;
                    case AssetBundleNameType.HashInstead:
                        {
                            bundleName = hash;
                        }
                        break;
                }
                importer.assetBundleName = bundleName;
                var bundleBuildInfo = new ResourceManifest.ResourceBundleBuildInfo()
                {
                    BundleHash = hash,
                    ResourceBundle = bundle,
                    BundleSize = 0
                };
                //这里存储hash与bundle，打包出来的包体长度在下一个流程处理
                resourceManifest.ResourceBundleBuildInfoDict.Add(bundleName, bundleBuildInfo);
            }
            for (int i = 0; i < bundleLength; i++)
            {
                var bundle = bundles[i];
                bundle.DependList.Clear();
                var importer = AssetImporter.GetAtPath(bundle.BundlePath);
                bundle.DependList.AddRange(AssetDatabase.GetAssetBundleDependencies(importer.assetBundleName, true));
            }
        }
        public void ProcessAssetBundle(AssetBundleBuildParams buildParams, ResourceDataset dataset, AssetBundleManifest unityManifest, ref ResourceManifest resourceManifest)
        {
            var bundleNames = unityManifest.GetAllAssetBundles();
            var bundleNameLength = bundleNames.Length;
            for (int i = 0; i < bundleNameLength; i++)
            {
                var bundleName = bundleNames[i];
                var bundlePath = Path.Combine(buildParams.AssetBundleBuildPath, bundleName);
                long bundleSize = 0;
                if (buildParams.AssetBundleEncryption)
                {
                    var bundleBytes = File.ReadAllBytes(bundlePath);
                    var offset = buildParams.AssetBundleOffsetValue;
                    bundleSize = Utility.IO.AppendAndWriteAllBytes(bundlePath, new byte[offset], bundleBytes);
                }
                else
                {
                    var bundleBytes = File.ReadAllBytes(bundlePath);
                    bundleSize = bundleBytes.LongLength;
                }
                if (resourceManifest.ResourceBundleBuildInfoDict.TryGetValue(bundleName, out var resourceBundleBuildInfo))
                {
                    //这里存储打包出来的AB长度
                    resourceBundleBuildInfo.BundleSize = bundleSize;
                }
                var bundleManifestPath = Utility.Text.Append(bundlePath, ".manifest");
                Utility.IO.DeleteFile(bundleManifestPath);
            }
            //这段生成resourceManifest.json文件
            var encryptionKey = buildParams.BuildIedAssetsEncryptionKey;
            var encrypt = buildParams.BuildedAssetsEncryption;
            resourceManifest.BuildVersion = buildParams.BuildVersion;
            var manifestJson = EditorUtil.Json.ToJson(resourceManifest);
            var manifestContext = manifestJson;
            if (encrypt)
            {
                var key = ResourceUtility.GenerateBytesAESKey(encryptionKey);
                manifestContext = Utility.Encryption.AESEncryptStringToString(manifestJson, key);
            }
            Utility.IO.WriteTextFile(buildParams.AssetBundleBuildPath, ResourceConstants.RESOURCE_MANIFEST, manifestContext);

            //删除生成文对应的主manifest文件
            var buildVersionPath = Path.Combine(buildParams.AssetBundleBuildPath, buildParams.BuildVersion);
            var buildVersionManifestPath = Utility.Text.Append(buildVersionPath, ".manifest");
            Utility.IO.DeleteFile(buildVersionPath);
            Utility.IO.DeleteFile(buildVersionManifestPath);

            var bundles = dataset.ResourceBundleList;
            var bundleLength = bundles.Count;
            for (int i = 0; i < bundleLength; i++)
            {
                var bundle = bundles[i];
                var importer = AssetImporter.GetAtPath(bundle.BundlePath);
                importer.assetBundleName = string.Empty;
            }
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            AssetDatabase.RemoveUnusedAssetBundleNames();
            System.GC.Collect();
        }
    }
}
