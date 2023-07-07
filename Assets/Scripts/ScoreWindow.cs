using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    private Text scoreText;

    // Awake is called before the first frame update
    void Awake()
    {
        scoreText = transform.Find("scoreText").GetComponent<Text>();

        int highscore = Score.GetHighscore();
        transform.Find("highscoreText").GetComponent<Text>().text = "HIGHSCORE\n" + highscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = Score.GetScore().ToString();
        if (Score.TrySetNewHighscore(Score.GetScore()))
        {
            int highscore = Score.GetHighscore();
            transform.Find("highscoreText").GetComponent<Text>().text = "HIGHSCORE\n" + highscore.ToString();
        }
    }
}