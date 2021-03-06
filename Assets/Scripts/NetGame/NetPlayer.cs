﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayer : MonoBehaviour
{
    public ChessType playChess = ChessType.Black;

    public void Update()
    {
        if (NetCheckBoard.Instance.isGameOver) return;

        PlayChess();

        Debug.Log(CheckBoard.Instance.timer.ToString());
        //Invoke("PlayChess", 0.3f);
    }

    public virtual void PlayChess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CheckBoard.Instance.chessDown(new int[2] { (int)(pos.x * 2 + 7 + 0.5), (int)(pos.y * 2 + 7 + 0.5) });
            CheckBoard.Instance.timer = 0;
        }
    }
}