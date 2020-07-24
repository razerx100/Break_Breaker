using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitScript : MonoBehaviour
{
    bool quitClicked = false;

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            quitClicked = true;
        }
    }

    public bool gotClicked()
    {
        return quitClicked;
    }
}
