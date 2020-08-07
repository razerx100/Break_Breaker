using UnityEngine;

public class DestroyOutOfBoundary : MonoBehaviour
{
    [SerializeField]
    GameController game_controller;
    private void OnTriggerExit(Collider other)
    {
        game_controller.game_over_fun("Game over!");
        Destroy(other.gameObject);
    }
}
