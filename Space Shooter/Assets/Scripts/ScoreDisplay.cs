using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    Text scoreText;
    GameSession game;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        game = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = game.GetScore().ToString();
    }
}
