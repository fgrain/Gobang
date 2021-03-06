using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFollow : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        if (CheckBoard.Instance.chessStack.Count == 0) transform.position = new Vector3(1000, 0, 0);
        if (CheckBoard.Instance.chessStack.Count > 0) transform.position = CheckBoard.Instance.chessStack.Peek().position;
    }

    public void Return()
    {
        SceneManager.LoadScene(0);
    }

    public void RePlay()
    {
        SceneManager.LoadScene(1);
    }
}