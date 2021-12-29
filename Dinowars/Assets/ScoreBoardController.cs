using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardController : MonoBehaviour
{
    [SerializeField] GameObject scoreBoard;


    private Controls controls;

    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }


    void Start()
    {
        Controls.Enable();
        Controls.Game.ScoreBoard.performed += ctx => scoreBoard.SetActive(true);
        Controls.Game.ScoreBoard.canceled += ctx => scoreBoard.SetActive(false);
    }

}
