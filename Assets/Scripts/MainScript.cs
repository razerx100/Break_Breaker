using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    const float BallVelocity = 25;
    const float BallStartX = 0, BallStartY = -33.8f;
    bool ballMoving = false;

    [SerializeField]
    GameObject platform;
    const float PlatformPosX = 0, PlatformPosY = -36.12555f;
    float platformColliderHalfWidth;

    [SerializeField]
    GameObject bricks1;
    [SerializeField]
    GameObject bricks2;
    float brickHalfWidth, brickHalfHeight;
    Vector3[] brickPositions;
    GameObject[] bricks;
    const float BrickBoundMinX = -14f, BrickBoundMaxX = 14f;
    const float BrickBoundMinY = 11f, BrickBoundMaxY = 38f;

    const int bricks_number = 6;
    public int bricks_num = bricks_number;

    void Start()
    {
        loadMainMenu();
    }

    void Update()
    {
        if (gameRunning)
        {
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
        gameRunning = true;
        background = Instantiate(background);
        border = Instantiate(border);
        platform = Instantiate(platform);
        ball = Instantiate(ball);
        brickSpawner();
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
        int bricksNum = 1;
        while(bricksNum < bricks_number)
        {
            Vector3 pos1 = new Vector3(UnityEngine.Random.Range(BrickBoundMinX, BrickBoundMaxX),
                    UnityEngine.Random.Range(BrickBoundMinY, BrickBoundMaxY), 0);
            bool collide = false;
            for (int i = 0; i < bricksNum; i++)
            {
                if ((pos1.x - brickHalfWidth < brickPositions[i].x) &&
                (pos1.x + brickHalfWidth > brickPositions[i].x) &&
                (pos1.y - brickHalfHeight < brickPositions[i].y) &&
                (pos1.y + brickHalfHeight > brickPositions[i].y))
                {
                    collide = true;
                }
            }
            if (!collide)
            {
                bricksNum++;
                int brickType = UnityEngine.Random.Range(0, 2);
                GameObject brick;
                if(brickType == 0)
                {
                    brick = Instantiate(bricks1);
                }
                else
                {
                    brick = Instantiate(bricks2);
                }
                brick.transform.position = pos1;
                bricks[bricksNum - 1] = brick;
                brickPositions[bricksNum - 1] = pos1;
            }
        }
    }
    void brickSpawner()
    {
        Vector3 pos1 = new Vector3(UnityEngine.Random.Range(BrickBoundMinX, BrickBoundMaxX),
            UnityEngine.Random.Range(BrickBoundMinY, BrickBoundMaxY), 0);
        GameObject brick = Instantiate(bricks1);
        brick.transform.position = pos1;
        brickPositions = new Vector3[bricks_number];
        bricks = new GameObject[bricks_number];
        bricks[0] = brick;
        brickPositions[0] = pos1;
        BoxCollider2D brickCollider = bricks1.GetComponent<BoxCollider2D>();
        brickHalfWidth = brickCollider.transform.position.x / 2;
        brickHalfHeight = brickCollider.transform.position.y / 2;
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
}
