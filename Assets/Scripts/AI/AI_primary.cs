using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_primary : Player
{
    protected Dictionary<string, int> scoreTable = new Dictionary<string, int>();
    public int[,] score = new int[15, 15];

    protected virtual void Start()
    {
        typeofChess();
    }

    protected virtual void typeofChess()
    {
        scoreTable.Add("_aa_", 100);
        scoreTable.Add("aa_", 5);
        scoreTable.Add("_aa", 5);

        scoreTable.Add("_aaa_", 1000);
        scoreTable.Add("aaa_", 50);
        scoreTable.Add("_aaa", 50);

        scoreTable.Add("_aaaa_", 10000);
        scoreTable.Add("aaaa_", 5000);
        scoreTable.Add("_aaaa", 5000);

        scoreTable.Add("_aaaaa_", 100000);
        scoreTable.Add("aaaaa_", 50000);
        scoreTable.Add("_aaaaa", 50000);
        scoreTable.Add("aaaaa", 50000);
    }

    protected virtual void updateScore(int[] pos, int[] offect, int chess)
    {
        string s = "a";
        //右边
        for (int i = offect[0], j = offect[1]; pos[0] + i >= 0 && pos[0] + i < 15 &&
            pos[1] + j >= 0 && pos[1] + j < 15 && (i < 5 || j < 5); i += offect[0], j += offect[1])
        {
            if (CheckBoard.Instance.grid[pos[0] + i, pos[1] + j] == chess)
            {
                s += 'a';
            }
            else if (CheckBoard.Instance.grid[pos[0] + i, pos[1] + j] == 0)
            {
                s += '_';
                break;
            }
            else break;
        }

        //左边
        for (int i = offect[0], j = offect[1]; pos[0] - i >= 0 && pos[0] - i < 15 &&
            pos[1] - j >= 0 && pos[1] - j < 15 && (i < 5 || j < 5); i += offect[0], j += offect[1])
        {
            if (CheckBoard.Instance.grid[pos[0] - i, pos[1] - j] == chess)
            {
                s = 'a' + s;
            }
            else if (CheckBoard.Instance.grid[pos[0] - i, pos[1] - j] == 0)
            {
                s = '_' + s;
                break;
            }
            else break;
        }

        if (scoreTable.ContainsKey(s))
        {
            score[pos[0], pos[1]] += scoreTable[s];
        }
    }

    protected void checkScore(int[] pos)
    {
        score[pos[0], pos[1]] = 0;
        updateScore(pos, new int[2] { 1, 0 }, 1);
        updateScore(pos, new int[2] { 0, 1 }, 1);
        updateScore(pos, new int[2] { 1, 1 }, 1);
        updateScore(pos, new int[2] { 1, -1 }, 1);

        updateScore(pos, new int[2] { 1, 0 }, 2);
        updateScore(pos, new int[2] { 0, 1 }, 2);
        updateScore(pos, new int[2] { 1, 1 }, 2);
        updateScore(pos, new int[2] { 1, -1 }, 2);
    }

    public override void PlayChess()
    {
        if (CheckBoard.Instance.chessStack.Count == 0)
        {
            CheckBoard.Instance.chessDown(new int[2] { 7, 7 });
            return;
        }
        int maxscore = 0;
        int[] maxpos = new int[2] { 0, 0 };
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (CheckBoard.Instance.grid[i, j] == 0)
                {
                    checkScore(new int[2] { i, j });
                    if (score[i, j] > maxscore)
                    {
                        maxscore = score[i, j];
                        maxpos[0] = i;
                        maxpos[1] = j;
                    }
                }
            }
        }
        CheckBoard.Instance.chessDown(maxpos);
    }
}