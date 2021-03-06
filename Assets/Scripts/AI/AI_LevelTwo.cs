using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_LevelTwo : AI_primary
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void typeofChess()
    {
        scoreTable.Add("aa___", 100);                      //眠二
        scoreTable.Add("a_a__", 100);
        scoreTable.Add("___aa", 100);
        scoreTable.Add("__a_a", 100);
        scoreTable.Add("a__a_", 100);
        scoreTable.Add("_a__a", 100);
        scoreTable.Add("a___a", 100);

        scoreTable.Add("__aa__", 500);                     //活二 "_aa___"
        scoreTable.Add("_a_a_", 500);
        scoreTable.Add("_a__a_", 500);
        scoreTable.Add("_aa__", 500);
        scoreTable.Add("__aa_", 500);

        scoreTable.Add("a_a_a", 1000);
        scoreTable.Add("aa__a", 1000);
        scoreTable.Add("_aa_a", 1000);
        scoreTable.Add("a_aa_", 1000);
        scoreTable.Add("_a_aa", 1000);
        scoreTable.Add("aa_a_", 1000);
        scoreTable.Add("aaa__", 1000);                     //眠三

        scoreTable.Add("_aa_a_", 9000);                    //跳活三
        scoreTable.Add("_a_aa_", 9000);

        scoreTable.Add("_aaa_", 10000);                    //活三

        scoreTable.Add("_aaaa", 15000);
        scoreTable.Add("aaaa_", 15000);
        scoreTable.Add("a_aaa", 15000);                    //冲四
        scoreTable.Add("aaa_a", 15000);
        scoreTable.Add("aa_aa", 15000);

        scoreTable.Add("_aaaa_", 50000);                 //活四

        scoreTable.Add("aaaaa", 1000000);                //连五
    }

    protected override void updateScore(int[] pos, int[] offect, int chess)
    {
        int chessNum = 1;
        bool lstop = false, rstop = false;
        bool rfirst = true;
        string s = "a";
        int ri = offect[0], rj = offect[1];
        int li = offect[0], lj = offect[1];
        while (chessNum < 8 && (!lstop || !rstop))
        {
            if (rfirst)
            {
                //right
                if (pos[0] + ri >= 0 && pos[0] + ri < 15 && pos[1] + rj >= 0 && pos[1] + rj < 15 && !rstop)
                {
                    if (CheckBoard.Instance.grid[pos[0] + ri, pos[1] + rj] == chess)
                    {
                        s += 'a';
                        chessNum++;
                    }
                    else if (CheckBoard.Instance.grid[pos[0] + ri, pos[1] + rj] == 0)
                    {
                        s += '_';
                        chessNum++;
                        if (!lstop) rfirst = false;
                    }
                    else
                    {
                        rstop = true;
                        if (!lstop) rfirst = false;
                    }
                    ri += offect[0]; rj += offect[1];
                }
                else
                {
                    rstop = true;
                    if (!lstop) rfirst = false;
                }
            }
            else
            {
                //left
                if (pos[0] - li >= 0 && pos[0] - li < 15 && pos[1] - lj >= 0 && pos[1] - lj < 15 && !lstop)
                {
                    if (CheckBoard.Instance.grid[pos[0] - li, pos[1] - lj] == chess)
                    {
                        s = 'a' + s;
                        chessNum++;
                    }
                    else if (CheckBoard.Instance.grid[pos[0] - li, pos[1] - lj] == 0)
                    {
                        s = '_' + s;
                        chessNum++;
                        if (!rstop) rfirst = true;
                    }
                    else
                    {
                        lstop = true;
                        if (!rstop) rfirst = true;
                    }
                    li += offect[0]; lj += offect[1];
                }
                else
                {
                    lstop = true;
                    if (!rstop) rfirst = true;
                }
            }
        }

        string compstr = "";
        foreach (string str in scoreTable.Keys)
        {
            if (s.Contains(str))
            {
                if (compstr == "")
                {
                    compstr = str;
                }
                else
                {
                    if (scoreTable[str] > scoreTable[compstr])
                        compstr = str;
                }
            }
        }

        if (compstr != "")
            score[pos[0], pos[1]] += scoreTable[compstr];
    }
}