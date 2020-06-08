using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Class for SampleScene Play
/// </summary>
public class TestManager : MonoBehaviour
{
    public static TestManager instance;
    /// <summary>
    /// Download Http Link (Can be modified to other Http path)
    /// </summary>
    public const string DOWNLOAD_FILE_LINK = "https://drive.google.com/u/0/uc?id=1uUTO-oPwLQ8GluSJZZHgblIdK0Ed5JnF&export=download";
    /// <summary>
    /// Download detination file name
    /// </summary>
    public const string FILE_NAME = "dev_image.jpg";
    /// <summary>
    /// Download detination path
    /// </summary>
    public string filePath=> $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}{FILE_NAME}";

    public const int VERSION = 49;

    public Text versionTextUI;
    public Text textUI;
    public Text errorTextUI;

    public Button downButton;
    public Button deleteButton;

    private bool isDownStart;
    private bool isDownloading;
    private long downId = -1;

    private void Awake() {
        instance = this;
        versionTextUI.text = "v"+VERSION.ToString().ToString();
    }

    void Start()
    {
        UIManager.instance.Info("Assets폴더의 README 문서를 참고해주세요.");
        UIManager.instance.RefreshBackground(filePath);
        RefreshButtons();
    }
    private void OnEnable() {
        StartCoroutine(WaitDownloadUpdate());
    }
    private void OnDisable() {
        StopAllCoroutines();
    }


    public void DownloadStart() {
#if UNITY_EDITOR
        UIManager.instance.Info("에디터 상에서는 플레이가 불가능합니다.\n실제 안드로이드 디바이스에서 실행해주세요.");
        return;
#else
        UIManager.instance.Log($"Download Waiting... =>" + System.DateTime.Now);
        UIManager.instance.Info("백그라운드로 진입 시 다운로드가 시작됩니다.");
        isDownStart = true; //The actual download runs on 'OnApplicationPause'
        RefreshButtons();
#endif
    }

    public void DeleteFile() {
        if(File.Exists(filePath)) {
            File.Delete(filePath);
            UIManager.instance.Info("");
        }
        UIManager.instance.RefreshBackground(filePath);
        RefreshButtons();
    }


    void OnApplicationPause(bool pause){
        if(pause) {//When returning to Unity Activity from application background.
            if(isDownStart) {
                //Actual Download
                isDownStart = false;
                isDownloading = true;
                UIManager.instance.Log($"Download Start =>" + System.DateTime.Now);
                UIManager.instance.Info("다운로드중...\n 완료되면 배경이 바뀝니다.");
                downId = NativeManager.instance.DownloadData(DOWNLOAD_FILE_LINK, FILE_NAME);
            }
        } 
    }

    IEnumerator WaitDownloadUpdate() {
        while (true) {
            if (isDownloading && downId!=-1) {
                DownloadStatus status=NativeManager.instance.GetDownloadStatus(downId);
                if (status==DownloadStatus.Pending || status==DownloadStatus.Running) {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                } else if (status == DownloadStatus.Successful) {
                    isDownloading = false;
                    DownloadComplete(true);
                } else {
                    isDownloading = false;
                    DownloadComplete(false);
                }
            }
            yield return null;
        }
    }

    private void DownloadComplete(bool success) {
        if (success) {
            UIManager.instance.Log($"Download Success =>" + System.DateTime.Now);
            UIManager.instance.Info("다운로드완료\n 배경이 변경되었습니다.");
        } else {
            UIManager.instance.Log($"Download Fail =>" + System.DateTime.Now);
            UIManager.instance.Info("다운로드실패\n 다시 시도해주세요.");
            if(File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }
        downId = -1;
        RefreshButtons();
        UIManager.instance.RefreshBackground(filePath);
    }

    private void RefreshButtons() {
        if (isDownStart) {
            downButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(false);
            return;   
        }
        if(File.Exists(filePath)) {
            downButton.gameObject.SetActive(false);
            deleteButton.gameObject.SetActive(true);
        } else {
            downButton.gameObject.SetActive(true);
            deleteButton.gameObject.SetActive(false);
        }
    }

}
