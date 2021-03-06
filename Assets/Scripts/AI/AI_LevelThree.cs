using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxNode
{
    public int chess;
    public int[] pos;
    public float value;
    public List<MinMaxNode> children;
}

public class AI_LevelThree : Player
{
    private Dictionary<string, int> scoreTable = new Dictionary<string, int>();
    private int depth = 6;

    private void Start()
    {
        scoreTable.Add("aa___", 10);                      //眠二
        scoreTable.Add("a_a__", 10);
        scoreTable.Add("___aa", 10);
        scoreTable.Add("__a_a", 10);
        scoreTable.Add("a__a_", 10);
        scoreTable.Add("_a__a", 10);
        scoreTable.Add("a___a", 10);

        scoreTable.Add("__aa__", 600);
        scoreTable.Add("_aa__", 600);
        scoreTable.Add("__aa_", 600);                   //活二 "_aa___"
        scoreTable.Add("_a_a_", 500);
        scoreTable.Add("_a__a_", 500);

        scoreTable.Add("a_a_a", 1000);
        scoreTable.Add("aa__a", 1000);
        scoreTable.Add("_aa_a", 1000);
        scoreTable.Add("a_aa_", 1000);
        scoreTable.Add("_a_aa", 1000);
        scoreTable.Add("aa_a_", 1000);
        scoreTable.Add("aaa__", 1000);                     //眠三

        scoreTable.Add("_aa_a_", 5000);                    //跳活三
        scoreTable.Add("_a_aa_", 5000);

        scoreTable.Add("_aaa_", 9000);                    //活三

        scoreTable.Add("_aaaa", 15000);
        scoreTable.Add("aaaa_", 15000);
        scoreTable.Add("a_aaa", 15000);                    //冲四
        scoreTable.Add("aaa_a", 15000);
        scoreTable.Add("aa_aa", 15000);

        scoreTable.Add("_aaaa_", 100000);                 //活四

        scoreTable.Add("aaaaa", int.MaxValue);                //连五
    }

    public override void PlayChess()
    {
        if (CheckBoard.Instance.chessStack.Count == 0)
        {
            CheckBoard.Instance.chessDown(new int[2] { 7, 7 });
            return;
        }

        float alpha = int.MinValue;
        float beta = int.MaxValue;
        int[,] g = (int[,])CheckBoard.Instance.grid.Clone();
        List<MinMaxNode> node = findNextLevel(g, (int)playChess);
        MinMaxNode res = new MinMaxNode();
        res.value = float.MinValue;
        foreach (var n in node)
        {
            float t = findMNode(g, 6, n, true, ref alpha, ref beta);
            if (t > res.value) res = n;
        }

        CheckBoard.Instance.chessDown(res.pos);
    }

    private float findMNode(int[,] grid, int level, MinMaxNode node, bool myself, ref float alpha, ref float beta)
    {
        if (level == 0 || node.value >= int.MaxValue)
        {
            return findNextLevel(grid, node.chess == 1 ? 2 : 1)[0].value;
        }

        if (myself)
        {
            //遍历所有可能走法
            grid[node.pos[0], node.pos[1]] = node.chess;
            node.children = findNextLevel(grid, node.chess == 1 ? 2 : 1);//下一步对方的走法
            foreach (MinMaxNode child in node.children)
            {
                //grid[child.pos[0], child.pos[1]] = child.chess;
                float newNode = findMNode((int[,])grid.Clone(), level - 1, child, !myself, ref alpha, ref beta);
                if (newNode > alpha) alpha = newNode;
                if (alpha > beta) return alpha;
            }
            return alpha;
        }
        else
        {
            grid[node.pos[0], node.pos[1]] = node.chess;
            node.children = findNextLevel(grid, node.chess == 1 ? 2 : 1);//下一步对方的走法
            foreach (MinMaxNode child in node.children)
            {
                //grid[child.pos[0], child.pos[1]] = child.chess;
                float newNode = findMNode((int[,])grid.Clone(), level - 1, child, !myself, ref alpha, ref beta);
                if (newNode < beta) beta = newNode;
                if (alpha > beta) return beta;
            }
            return beta;
        }
    }

    private List<MinMaxNode> findNextLevel(int[,] grid, int chess)
    {
        List<MinMaxNode> poslist = new List<MinMaxNode>();

        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (grid[i, j] != 0) continue;

                float p = checkScore(grid, new int[] { i, j });
                MinMaxNode n = new MinMaxNode();
                n.pos = new int[2] { i, j };
                n.chess = chess;

                if (chess == (int)playChess)
                {
                    n.value = p;
                    if (poslist.Count < depth)
                        poslist.Add(n);
                    else
                    {
                        for (int m = 0; m < depth; m++)
                        {
                            if (p > poslist[m].value)
                            {
                                poslist.Insert(m, n);
                                poslist.RemoveAt(depth);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    n.value = -p;
                    if (poslist.Count < depth)
                        poslist.Add(n);
                    else
                    {
                        for (int m = 0; m < depth; m++)
                        {
                            if (n.value < poslist[m].value)
                            {
                                poslist.Insert(m, n);
                                poslist.RemoveAt(depth);
                                break;
                            }
                        }
                    }
                }
            }
        }
        return poslist;
    }

    private float updateScore(int[,] grid, int[] pos, int[] offect, int chess)
    {
        float score = 0;
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
                    if (grid[pos[0] + ri, pos[1] + rj] == chess)
                    {
                        s += 'a';
                        chessNum++;
                    }
                    else if (grid[pos[0] + ri, pos[1] + rj] == 0)
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
                    if (grid[pos[0] - li, pos[1] - lj] == chess)
                    {
                        s = 'a' + s;
                        chessNum++;
                    }
                    else if (grid[pos[0] - li, pos[1] - lj] == 0)
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
            score = scoreTable[compstr];
        return score;
    }

    private float checkScore(int[,] grid, int[] pos)
    {
        float score = 0;
        score += updateScore(grid, pos, new int[2] { 1, 0 }, 1);
        score += updateScore(grid, pos, new int[2] { 0, 1 }, 1);
        score += updateScore(grid, pos, new int[2] { 1, 1 }, 1);
        score += updateScore(grid, pos, new int[2] { 1, -1 }, 1);

        score += updateScore(grid, pos, new int[2] { 1, 0 }, 2);
        score += updateScore(grid, pos, new int[2] { 0, 1 }, 2);
        score += updateScore(grid, pos, new int[2] { 1, 1 }, 2);
        score += updateScore(grid, pos, new int[2] { 1, -1 }, 2);
        return score;
    }
}