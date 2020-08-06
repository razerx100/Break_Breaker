using UnityEngine;
using Assets.Scripts;

public class GameController : MonoBehaviour
{
    Boundary boundary;

    [SerializeField]
    GameObject brick_prefab;

    [SerializeField]
    Material brick1, brick2;

    [SerializeField]
    int bricks_number;

    private void Start()
    {
        boundary = new Boundary();
        boundary.min_x = -9.33f;
        boundary.max_x = 9.38f;
        boundary.min_z = 19.45f;
        brickGenerator();
    }

    void brickGenerator()
    {
        Vector3[] all_possible_positions = new Vector3[32];
        bool[] is_position_taken = new bool[32];
        int position = 0;
        float z_location = boundary.min_z;
        float[] x_locations = { boundary.min_x, -3.15f, 3.2f, boundary.max_x };
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                all_possible_positions[position] = new Vector3(x_locations[j], 0.0f,z_location);
                is_position_taken[position] = false;
                position++;
            }
            z_location += 2.2f;
        }
        int brick_generated = 0;
        while (brick_generated < bricks_number)
        {
            int random_position = Random.Range(0, 32);
            if (!is_position_taken[random_position])
            {
                int brick_type = Random.Range(0, 2);
                GameObject brick = Instantiate(brick_prefab, all_possible_positions[random_position], Quaternion.identity);
                if (brick_type == 0)
                {
                    brick.GetComponent<MeshRenderer>().material = brick1;
                    brick.tag = "Brick";
                }
                else
                {
                    brick.GetComponent<MeshRenderer>().material = brick2;
                    brick.tag = "Brick2";
                    brick.GetComponent<HitCount>().hits = 0;
                }
                brick.transform.Rotate(0, 90, 0);
                is_position_taken[random_position] = true;
                brick_generated++;
            }
        }
    }
}
