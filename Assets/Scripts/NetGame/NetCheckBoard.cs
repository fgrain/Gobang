using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetCheckBoard : MonoBehaviour
{
    private static NetCheckBoard _instance;
    public bool gameStart = false;
    public bool isGameOver = false;
    protected float steph = 21.5f;
    protected float stepv = 22;
    public Stack<Transform> chessStack = new Stack<Transform>();
    public int[,] grid = new int[15, 15];
    public ChessType turn;

    protected Transform checkBoard;
    public Transform[] player;
    public Text gameOverText;

    public static NetCheckBoard Instance { get => _instance; }

    private void Awake()
    {
        _instance = this;
        checkBoard = GameObject.Find("CheckBoard").transform;
        turn = ChessType.Black;
    }

    public void chessDown(int[] pos)
    {
        if (gameStart)
        {
            if (turn == ChessType.Black && (pos[0] >= 0 && pos[0] < 15) && (pos[1] >= 0 && pos[1] < 15))
            {
                if (grid[pos[0], pos[1]] == 0)
                {
                    Transform chess = Instantiate(player[0], checkBoard, false);
                    chess.localPosition = new Vector2((pos[0] - 7) * steph, (pos[1] - 7) * stepv);
                    grid[pos[0], pos[1]] = 1;
                    chessStack.Push(chess);
                    if (checkWiner(pos))
                    {
                        GameOver();
                    }
                    turn = ChessType.White;
                }
            }
            else if (turn == ChessType.White && (pos[0] >= 0 && pos[0] < 15) && (pos[1] >= 0 && pos[1] < 15))
            {
                if (grid[pos[0], pos[1]] == 0)
                {
                    Transform chess = Instantiate(player[1], checkBoard, false);
                    chess.localPosition = new Vector2((pos[0] - 7) * steph, (pos[1] - 7) * stepv);
                    grid[pos[0], pos[1]] = 2;
                    chessStack.Push(chess);
                    if (checkWiner(pos))
                    {
                        GameOver();
                    }
                    turn = ChessType.Black;
                }
            }
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        gameOverText.transform.parent.gameObject.SetActive(true);
        gameOverText.text = turn == ChessType.Black ? "黑棋胜" : "白棋胜";
        return;
    }

    public bool checkWiner(int[] pos)
    {
        if (checkLine(pos, new int[2] { 1, 0 })) return true;
        if (checkLine(pos, new int[2] { 0, 1 })) return true;
        if (checkLine(pos, new int[2] { 1, 1 })) return true;
        if (checkLine(pos, new int[2] { 1, -1 })) return true;
        else return false;
    }

    public bool checkLine(int[] pos, int[] offect)
    {
        int linkNum = 1;
        //右边
        for (int i = offect[0], j = offect[1]; pos[0] + i >= 0 && pos[0] + i < 15 &&
            pos[1] + j >= 0 && pos[1] + j < 15 && (i < 5 || j < 5); i += offect[0], j += offect[1])
        {
            if (grid[pos[0] + i, pos[1] + j] == (int)turn)
            {
                linkNum++;
            }
            else break;
        }

        //左边
        for (int i = offect[0], j = offect[1]; pos[0] - i >= 0 && pos[0] - i < 15 &&
            pos[1] - j >= 0 && pos[1] - j < 15 && (i < 5 || j < 5); i += offect[0], j += offect[1])
        {
            if (grid[pos[0] - i, pos[1] - j] == (int)turn)
            {
                linkNum++;
            }
            else break;
        }
        if (linkNum > 4) return true;

        return false;
    }

    public void RegretChess()
    {
        if (chessStack.Count > 1 && isGameOver == false)
        {
            Transform re = chessStack.Pop();
            grid[(int)(re.localPosition.x / steph + 7), (int)(re.localPosition.y / stepv + 7)] = 0;
            Destroy(re.gameObject);
            re = chessStack.Pop();
            grid[(int)(re.localPosition.x / steph + 7), (int)(re.localPosition.y / stepv + 7)] = 0;
            Destroy(re.gameObject);
        }
    }
}