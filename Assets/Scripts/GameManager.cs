using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Player> p = new List<Player>();

    private void Awake()
    {
        int black = PlayerPrefs.GetInt("player1");
        int white = PlayerPrefs.GetInt("player2");
        for (int i = 0; i < p.Count; i++)
        {
            if (black == i) p[i].playChess = ChessType.Black;
            else if (white == i) p[i].playChess = ChessType.White;
            else p[i].playChess = ChessType.Watch;
        }
    }

    public void SetPlayer1(int i)
    {
        PlayerPrefs.SetInt("player1", i);
    }

    public void SetPlayer2(int i)
    {
        PlayerPrefs.SetInt("player2", i);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StartWebGame()
    {
        SceneManager.LoadScene(2);
    }

    public void ChangeChessColor()
    {
        for (int i = 0; i < p.Count; i++)
        {
            if (p[i].playChess == ChessType.Black) SetPlayer2(i);
            else if (p[i].playChess == ChessType.White) SetPlayer1(i);
            else p[i].playChess = ChessType.Watch;
        }
        SceneManager.LoadScene(1);
    }
}