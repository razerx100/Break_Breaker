using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    bool startGameClicked = false;
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startGameClicked = true;
        }
    }
    public bool gotClicked()
    {
        return startGameClicked;
    }
}
