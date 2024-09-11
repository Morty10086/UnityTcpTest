using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cr_LevelManager : MonoBehaviour
{
    private static Cr_LevelManager instance;
    public static Cr_LevelManager Instance => instance;



    #region UI信息

    public TextMeshProUGUI gameOverText;
    public GameObject gameOverPanel;
    
    #endregion
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
       gameOverText.gameObject.SetActive(false);
       gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        
        
    }

    /// <summary>
    /// Call this func when any player is dead
    /// </summary>
    /// <param name="isP1"></param>
    public void GameOver(bool isP1)
    {
        string winner = isP1 ? "Player 1" : "Player 2";
        
        gameOverText.text = $"{winner} Wins!";
        gameOverPanel.SetActive(true);
        gameOverText.gameObject.SetActive(true);

    }
}
