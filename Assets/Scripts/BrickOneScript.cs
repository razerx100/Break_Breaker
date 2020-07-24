using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickOneScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Camera.main.GetComponent<MainScript>().bricks_num--;
    }
}
