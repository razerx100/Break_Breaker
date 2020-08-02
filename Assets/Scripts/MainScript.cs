using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using number_gen = Assets.Scripts.NUmberManager;

public class MainScript : MonoBehaviour
{
    bool gameRunning = false;
    bool inputAllowed = true;
    bool quit_confirm = false;
    bool end_screen_confirm = false;
    bool continue_confirm = false;

    [SerializeField]
    GameObject quitPrefab;
    GameObject quit;
    const float ContinueQuitX = 4.33099f, ContinueQuitY = -12.65309f;

    [SerializeField]
    GameObject startScreen, startGame, continueGamePrefab, continueScreenPrefab;
    GameObject continueGame, continueScreen;

    [SerializeField]
    GameObject background;

    [SerializeField]
    GameObject border;
    const float BorderPosX = 0, BorderPosY = -0.1f;

    [SerializeField]
    GameObject ball;
    Vector2 ballPreviousVelocity;
    const float constant_ball_velocity = 25;
    float BallVelocity = constant_ball_velocity;
    int ball_velocity_increase_cap = 100;
    const float BallStartX = 0, BallStartY = -33.8f;
    bool ballMoving = false;

    [SerializeField]
    GameObject platform;
    const float PlatformPosX = 0, PlatformPosY = -36.12555f;
    float platformColliderHalfWidth;

    [SerializeField]
    GameObject bricks1_prefab;
    [SerializeField]
    GameObject bricks2_prefab;
    GameObject[] bricks;
    const float BrickBoundMinX = -14f, BrickBoundMaxX = 14f;
    const float BrickBoundMinY = 2f, BrickBoundMaxY = 37f;
    const float Brick_X_middle = 4.7f, Brick_Y_distance = 3.5f;

    int bricks_number;
    public int bricks_num;

    [SerializeField]
    GameObject zeroPrefab, onePrefab, twoPrefab, threePrefab, fourPrefab, fivePrefab, sixPrefab,
        sevenPrefab, eightPrefab, ninePrefab;
    GameObject sixth_digit, fifth_digit, fourth_digit, third_digit, second_digit, first_digit;
    float in_game_sixth_digit_X = -44.2f, in_game_digit_Y = 39.8f;
    float continue_sixth_digit_X = -12.7f, continue_digit_Y = 23.96f;
    float continue_digit_scale_X = 0.79f, continue_digit_scale_Y = 0.73f;
    public int score;
    public bool score_changed = true;
    bool score_reset = false;


    void Start()
    {
        loadMainMenu();
    }

    void Update()
    {
        if (gameRunning)
        {
            if (score_changed)
            {
                int increase_ball_velocity = score / ball_velocity_increase_cap;
                if (increase_ball_velocity != 0) {
                    BallVelocity += 5;
                    ball_velocity_increase_cap += 100;
                }
                clean_up_score();
                show_score();
                score_changed = false;
            }
            if (inputAllowed)
            {
                keyInput();
            }
            ballOut();
            if (bricks_num == 0)
            {
                setUpContinueScreen();
            }
        }
        else
        {
            detectMouseInput();
        }
        if (quit_confirm)
        {
            quitOrNot();
            if (Input.GetKeyDown("space"))
            {
                Destroy(quit);
                resumeGame();
                quit_confirm = false;
            }
        }
        if (end_screen_confirm)
        {
            quitOrNot();
            continue_confirm = continueGame.GetComponent<ContinueGameScript>().gotClicked();
            if (continue_confirm)
            {
                cleanUpContinueScreen();
            }
        }
    }
    void setUpContinueScreen()
    {
        show_continue_screen_score();
        continueGame = Instantiate(continueGamePrefab);
        continueScreen = Instantiate(continueScreenPrefab);
        quit = Instantiate(quitPrefab);
        quit.transform.position = new Vector3(ContinueQuitX, ContinueQuitY);
        reSet();
        inputAllowed = false;
        end_screen_confirm = true;
    }
    void cleanUpContinueScreen()
    {
        inputAllowed = true;
        end_screen_confirm = false;
        continue_confirm = false;
        Destroy(continueGame);
        Destroy(continueScreen);
        Destroy(quit);
        clean_up_score();
        score_changed = true;
        if (score_reset)
        {
            score = 0;
            BallVelocity = constant_ball_velocity;
            score_reset = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 ballOrientation = ball.GetComponent<Rigidbody2D>().velocity.normalized;
        ball.GetComponent<Rigidbody2D>().velocity = ballOrientation * BallVelocity;
    }

    void loadMainMenu()
    {
        startScreen = Instantiate(startScreen);
        startGame = Instantiate(startGame);
        quit = Instantiate(quitPrefab);
    }
    void cleanUpMainMenu()
    {
        Destroy(startScreen);
        Destroy(startGame);
        Destroy(quit);
    }
    void start_game()
    {
        score = 0;
        gameRunning = true;
        background = Instantiate(background);
        border = Instantiate(border);
        platform = Instantiate(platform);
        ball = Instantiate(ball);
        brickSpawner();
        show_score();
        score_changed = false;
    }
    void detectMouseInput()
    {
        if (startGame.GetComponent<StartGameScript>().gotClicked())
        {
            cleanUpMainMenu();
            start_game();
        }
        quitOrNot();
    }
    void quitOrNot()
    {
        if (quit.GetComponent<QuitScript>().gotClicked())
        {
            Application.Quit();
        }
    }
    void launchBall()
    {
        Vector2 angle = new Vector2(Mathf.Sin(45), Mathf.Cos(45));
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce(angle * BallVelocity, ForceMode2D.Impulse);
        ballMoving = true;
    }

    void pauseGame()
    {
        ballPreviousVelocity = ball.GetComponent<Rigidbody2D>().velocity;
        stopBall();
        inputAllowed = false;
    }

    void keyInput()
    {
        if (Input.GetAxis("Submit") > 0 && !ballMoving)
        {
            launchBall();
        }
        Vector3 platformPos = platform.transform.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            platformPos.x += horizontalInput * 25 * Time.deltaTime;
            platform.transform.position = platformPos;
            if (!ballMoving) {
                Vector2 ballPos = ball.transform.position;
                ballPos.x += horizontalInput * 25 * Time.deltaTime;
                ball.transform.position = ballPos;
                clampBall();
            }
            clampPlatform();
        }
        if(Input.GetKeyDown("escape"))
        {
            pauseGame();
            quitConfirm();
        }
    }
    void quitConfirm()
    {
        quit = Instantiate(quitPrefab);
        quit_confirm = true;
    }
    void resumeGame()
    {
        ball.GetComponent<Rigidbody2D>().AddForce(ballPreviousVelocity);
        inputAllowed = true;
    }
    void clampBall()
    {
        const float LeftBound = -15.4f, RightBound = 15.4f;
        Vector3 platformPos = platform.transform.position;
        if(platformPos.x - platformColliderHalfWidth < LeftBound)
        {
            platformPos.x = LeftBound + platformColliderHalfWidth;
        }
        else if(platformPos.x + platformColliderHalfWidth > RightBound)
        {
            platformPos.x = RightBound - platformColliderHalfWidth;
        }
        platformPos.y = ball.transform.position.y;
        ball.transform.position = platformPos;
    }
    void clampPlatform()
    {
        const float LeftBound = -15.4f, RightBound = 15.4f;
        platformColliderHalfWidth = platform.GetComponent<BoxCollider2D>().size.x / 2;
        Vector3 platformPos = platform.transform.position;
        if(platformPos.x - platformColliderHalfWidth < LeftBound)
        {
            platformPos.x = LeftBound + platformColliderHalfWidth;
        }
        else if(platformPos.x + platformColliderHalfWidth > RightBound)
        {
            platformPos.x = RightBound - platformColliderHalfWidth;
        }
        platform.transform.position = platformPos;
    }
    void brickGenerator()
    {
        Vector2[] all_possible_positions = new Vector2[44];
        bool[] is_position_taken = new bool[44];
        int position = 0;
        float y_location = BrickBoundMinY;
        float[] x_locations = { BrickBoundMinX, -4.7f, 4.7f, BrickBoundMaxX };
        for(int i = 0; i < 11; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                all_possible_positions[position] = new Vector2(x_locations[j], y_location);
                is_position_taken[position] = false;
                position++;
            }
            y_location += Brick_Y_distance;
        }
        int brick_generated = 0;
        while(brick_generated < bricks_number)
        {
            int random_position = UnityEngine.Random.Range(0, 44);
            if (!is_position_taken[random_position])
            {
                int brick_type = UnityEngine.Random.Range(0, 2);
                GameObject brick;
                if(brick_type == 0)
                {
                    brick = Instantiate(bricks1_prefab);
                }
                else
                {
                    brick = Instantiate(bricks2_prefab);
                }
                brick.transform.position = new Vector3(all_possible_positions[random_position].x,
                    all_possible_positions[random_position].y, 0);
                bricks[brick_generated] = brick;
                is_position_taken[random_position] = true;
                brick_generated++;
            }
        }
    }
    void brickSpawner()
    {
        bricks_number = UnityEngine.Random.Range(6, 15);
        bricks_num = bricks_number;
        bricks = new GameObject[bricks_number];
        brickGenerator();
        bricks_num = bricks_number;
    }

    void ballOut()
    {
        float boundary = -42;
        if(ball.transform.position.y < boundary)
        {
            for(int i = 0; i < bricks_number; i++)
            {
                Destroy(bricks[i]);
            }
            setUpContinueScreen();
            score_reset = true;
        }
    }

    void stopBall()
    {
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
        ballRb.velocity = Vector3.zero;
        ballRb.Sleep();
    }
    void reSet()
    {
        brickSpawner();
        platform.transform.position = new Vector3(PlatformPosX, PlatformPosY, 0);
        ball.transform.position = new Vector3(BallStartX, BallStartY, 0);
        stopBall();
        ballMoving = false;
    }

    void show_score()
    {
        int[] num_array = new number_gen(score).get_value();
        render_digit(out sixth_digit, num_array[0], new Vector2(in_game_sixth_digit_X, in_game_digit_Y));
        render_digit(out fifth_digit, num_array[1], new Vector2(in_game_sixth_digit_X + 4, in_game_digit_Y));
        render_digit(out fourth_digit, num_array[2], new Vector2(in_game_sixth_digit_X + 8, in_game_digit_Y));
        render_digit(out third_digit, num_array[3], new Vector2(in_game_sixth_digit_X + 12, in_game_digit_Y));
        render_digit(out second_digit, num_array[4], new Vector2(in_game_sixth_digit_X + 16, in_game_digit_Y));
        render_digit(out first_digit, num_array[5], new Vector2(in_game_sixth_digit_X + 20, in_game_digit_Y));
    }

    void clean_up_score()
    {
        Destroy(first_digit);
        Destroy(second_digit);
        Destroy(third_digit);
        Destroy(fourth_digit);
        Destroy(fifth_digit);
        Destroy(sixth_digit);
    }

    void show_continue_screen_score()
    {
        clean_up_score();
        score_changed = false;
        int[] num_array = new number_gen(score).get_value();
        render_digit(out sixth_digit, num_array[0], new Vector2(continue_sixth_digit_X, continue_digit_Y));
        sixth_digit.transform.localScale = new Vector3(continue_digit_scale_X, continue_digit_scale_Y, 1);
        sixth_digit.GetComponent<SpriteRenderer>().sortingOrder = 3;
        render_digit(out fifth_digit, num_array[1], new Vector2(continue_sixth_digit_X + 7, continue_digit_Y));
        fifth_digit.transform.localScale = new Vector3(continue_digit_scale_X, continue_digit_scale_Y, 1);
        fifth_digit.GetComponent<SpriteRenderer>().sortingOrder = 3;
        render_digit(out fourth_digit, num_array[2], new Vector2(continue_sixth_digit_X + 14, continue_digit_Y));
        fourth_digit.transform.localScale = new Vector3(continue_digit_scale_X, continue_digit_scale_Y, 1);
        fourth_digit.GetComponent<SpriteRenderer>().sortingOrder = 3;
        render_digit(out third_digit, num_array[3], new Vector2(continue_sixth_digit_X + 21, continue_digit_Y));
        third_digit.transform.localScale = new Vector3(continue_digit_scale_X, continue_digit_scale_Y, 1);
        third_digit.GetComponent<SpriteRenderer>().sortingOrder = 3;
        render_digit(out second_digit, num_array[4], new Vector2(continue_sixth_digit_X + 28, continue_digit_Y));
        second_digit.transform.localScale = new Vector3(continue_digit_scale_X, continue_digit_scale_Y, 1);
        second_digit.GetComponent<SpriteRenderer>().sortingOrder = 3;
        render_digit(out first_digit, num_array[5], new Vector2(continue_sixth_digit_X + 35, continue_digit_Y));
        first_digit.transform.localScale = new Vector3(continue_digit_scale_X, continue_digit_scale_Y, 1);
        first_digit.GetComponent<SpriteRenderer>().sortingOrder = 3;

    }
    void render_digit(out GameObject gobj, int digit, Vector2 co_ordinate)
    {
        switch (digit)
        {
            case 0:
                {
                    gobj = Instantiate(zeroPrefab);
                    break;
                }
            case 1:
                {
                    gobj = Instantiate(onePrefab);
                    break;
                }
            case 2:
                {
                    gobj = Instantiate(twoPrefab);
                    break;
                }
            case 3:
                {
                    gobj = Instantiate(threePrefab);
                    break;
                }
            case 4:
                {
                    gobj = Instantiate(fourPrefab);
                    break;
                }
            case 5:
                {
                    gobj = Instantiate(fivePrefab);
                    break;
                }
            case 6:
                {
                    gobj = Instantiate(sixPrefab);
                    break;
                }
            case 7:
                {
                    gobj = Instantiate(sevenPrefab);
                    break;
                }
            case 8:
                {
                    gobj = Instantiate(eightPrefab);
                    break;
                }
            case 9:
                {
                    gobj = Instantiate(ninePrefab);
                    break;
                }
            default:
                {
                    gobj = Instantiate(zeroPrefab);
                    break;
                }
        }
        gobj.transform.position = new Vector3(co_ordinate.x, co_ordinate.y, 0);
    }
}
