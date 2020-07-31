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
            hitCount = 2;
            MainScript main = Camera.main.GetComponent<MainScript>();
            main.bricks_num--;
            main.score += 10;
            main.score_changed = true;
            Destroy(gameObject);
        }
    }
}
