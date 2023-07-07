using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverButton : MonoBehaviour
{

    public bool musicPlayed;
    
    public void Awake()
    {
        musicPlayed = false;
    }

    public void Update()
    {
        SoundManager.PlaySound(SoundManager.Sound.SnakeDie);          
    }

    public static void ReloadSnakeScene()
    {
        Loader.Load(Loader.Scene.Snake);
    }

    public static void MainMenu()
    {
        Loader.Load(Loader.Scene.MainMenu);
    }
}
