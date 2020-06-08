using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Class for SampleScene UI
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text infoTextUI;
    public Text centerTextUI;
    public Image backgroundImage;


    private void Awake() {
        instance = this;
    }

    public void Log(string text) {
        centerTextUI.text = centerTextUI.text + text + "\n";
    }
    public void Info(string infoText) {
        infoTextUI.text = infoText;
    }

    /// <summary>
    /// If there is a file in path, Background sprite changes.
    /// If not, Background set inactive.
    /// </summary>
    /// <param name="imagePath">Image Path (.png , .jpg)</param>
    public void RefreshBackground(string imagePath) {
        if(string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath)) {
            backgroundImage.gameObject.SetActive(false);
            return;
        }
        backgroundImage.gameObject.SetActive(true);
        backgroundImage.sprite = LoadSprite(imagePath);
    }


    private Sprite LoadSprite(string path) {
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
}
