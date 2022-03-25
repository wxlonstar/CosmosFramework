﻿using Quark;
using UnityEngine;
using UnityEngine.UI;
using Cosmos;
public class QuarkCheckManifestPanel : MonoBehaviour
{
    [SerializeField] Button btnDownload;
    [SerializeField] Button btnCancel;
    [SerializeField] Text txtDownloadInfo;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] QuarkDownloadMonitorPanel monitorPanel;
    bool canDownload = false;
    private void Awake()
    {
        QuarkResources.OnDetectedSuccess += OnDetectedSuccess;
        QuarkResources.OnDetectedFailure += OnDetectedFailure;
    }
    void Start()
    {
        btnDownload?.onClick.AddListener(DownloadClick);
        btnCancel?.onClick.AddListener(CancelClick);

    }
    void OnDetectedFailure(string errorMessage)
    {
        if (txtDownloadInfo != null)
        {
            txtDownloadInfo.text = "检测失败";
            canDownload = false;
        }
    }
    void OnDetectedSuccess(long size)
    {
        if (txtDownloadInfo != null)
        {
            if (size <= 0)
            {
                Utility.Debug.LogInfo("当前为最新内容！");
                monitorPanel.HasNoLatest();
            }
            else
            {
                var byteSize = Utility.Converter.FormatBytesSize(size);
                txtDownloadInfo.text = $"检测到新内容，总计需要下载{byteSize}内容";
                canDownload = true;
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }
    }
    void DownloadClick()
    {
        if (canDownload)
        {
            QuarkResources.LaunchDownload();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            monitorPanel.StartDownload();
        }
    }
    void CancelClick()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
