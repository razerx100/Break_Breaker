using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    Boundary boundary;

    public static int total_score = 0;

    [SerializeField]
    GameObject brick_prefab;

    [SerializeField]
    Material brick1, brick2;

    [SerializeField]
    int bricks_number;

    int score = 0, bricks;

    [SerializeField]
    GameObject game_over_text, score_text, restart_text;

    [SerializeField]
    BaseController basee;

    [SerializeField]
    BallController ball;

    bool game_over = false;

    private void Start()
    {
        score = total_score;
        bricks = bricks_number;
        boundary = new Boundary();
        boundary.min_x = -9.33f;
        boundary.max_x = 9.38f;
        boundary.min_z = 19.45f;
        brickGenerator();
        game_over_text.GetComponent<Text>().text = "";
        restart_text.GetComponent<Text>().text = "";
        score_text.GetComponent<Text>().text = "Score : " + score;
    }

    private void Update()
    {
        if (game_over)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if(bricks == 0)
        {
            game_over_fun("Level cleared!");
            total_score = score;
            pause_game();
        }
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

    void pause_game()
    {
        basee.is_running(false);
        ball.is_running(false);
    }
    public void game_over_fun(string text)
    {
        game_over = true;
        game_over_text.GetComponent<Text>().text = text;
        restart_text.GetComponent<Text>().text = "Press 'R' for restart";
        basee.is_running(false);
        total_score = 0;
    }

    public void add_score(int new_score)
    {
        score += new_score;
        score_text.GetComponent<Text>().text = "Score : " + score;
    }

    public void decrease_bricks()
    {
        bricks--;
    }
}
