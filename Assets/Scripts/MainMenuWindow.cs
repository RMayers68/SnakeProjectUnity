using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MainMenuWindow : MonoBehaviour
{


    private void Awake()
    {
        transform.Find("howToPlaySub").Find("howToPlay").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        ShowSub("mainSub");
        transform.Find("mainSub").Find("playBtn").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.Snake);
        transform.Find("mainSub").Find("howToPlayBtn").GetComponent<Button_UI>().ClickFunc = () => ShowSub("howToPlaySub");
        transform.Find("mainSub").Find("quitBtn").GetComponent<Button_UI>().ClickFunc = () => Application.Quit();
        transform.Find("howToPlaySub").Find("howToPlay").Find("backToMenu").GetComponent<Button_UI>().ClickFunc = () => ShowSub("mainSub");
    }

    private void ShowSub(string subName)
    {
        transform.Find("mainSub").gameObject.SetActive(false);
        transform.Find("howToPlaySub").gameObject.SetActive(false);

        transform.Find(subName).gameObject.SetActive(true);
    }
}
