using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGameScript : MonoBehaviour
{
    bool continueClicked = false;

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            continueClicked = true;
        }
    }

    public bool gotClicked()
    {
        return continueClicked;
    }
}
