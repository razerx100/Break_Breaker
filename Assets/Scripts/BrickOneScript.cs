using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickOneScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        MainScript main = Camera.main.GetComponent<MainScript>();
        main.bricks_num--;
        main.score += 5;
        main.score_changed = true;
        Destroy(gameObject);
    }
}
