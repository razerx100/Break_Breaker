using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickTwoScript : MonoBehaviour
{
    int hitCount = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitCount++;
        if (hitCount == 2)
        {
            Destroy(gameObject);
            hitCount = 2;
            Camera.main.GetComponent<MainScript>().bricks_num--;
        }
    }
}
