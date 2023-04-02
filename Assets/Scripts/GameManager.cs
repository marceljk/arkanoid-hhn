using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int health = 3;
    private int score = 0;
    // icons src https://semantic-ui.com/introduction/getting-started.html 
    const string filledHeart = "\uF004 ";
    const string emptyHeart = "\uF08A ";

    public TMP_Text textHealth;
    public TMP_Text textScore;
    public TMP_Text textResult;
    public TMP_Text textResultSmall;
    public TMP_Text textMultiplier;
    public Transform bricksParent;
    public GameObject brickObject;
    public GameObject powerUpObject;
    public static GameManager instance;
    public int amountPowerUp;
    public bool IsGameRunning
    {
        get
        {
            return isGameRunning;
        }
    }

    private int runningLevel = 0;
    private bool isGameRunning = true;
    private int multiplyPoints = 1;
    private string[] powerUpTypes = { "IncreasePaddle", "ExtraLive", "IncreaseBall", "DoublePoints" };
    public int Health
    {
        set
        {
            health = value;
            updateHealthDisplay();
        }
        get
        {
            return health;
        }
    }
    public void AddScorePoint(int add)
    {
        score += (add * multiplyPoints);
        updateScoreDisplay();
    }

    public void MultiplyPoints()
    {
        multiplyPoints *= 2;
        updateMultiplierDisplay();
    }

    void updateMultiplierDisplay()
    {
        if (multiplyPoints == 1)
        {
            textMultiplier.text = "";
            return;
        }
        textMultiplier.text = "x" + multiplyPoints;
    }
    void updateHealthDisplay()
    {
        string healthState = "";
        int showLives = health > 3 ? health : 3;
        for (int i = 0; i < showLives; i++)
        {
            if (i < health)
            {
                healthState += filledHeart;
            }
            else
            {
                healthState += emptyHeart;
            }
        }
        textHealth.text = healthState;
        if (health == 0)
        {
            textResult.text = "Game Over!\nYour score is " + score.ToString();
            textResultSmall.text = "Press Space to restart";
            isGameRunning = false;
        }
    }

    void updateScoreDisplay()
    {
        textScore.text = score.ToString();
    }

    void initLevel1()
    {
        runningLevel = 1;
        for (int i = 2; i < 6; i++)
        {
            var blockHealth = (i % 2) + 1;
            for (float j = -6f; j <= 6f; j += 1.5f)
            {
                if (j == 0) continue;
                var brick = Instantiate(brickObject, new Vector3(j, 0, i), Quaternion.identity, bricksParent);
                brick.gameObject.GetComponent<BrickManager>().health = blockHealth;
            }
        }

        int powerUps = amountPowerUp < bricksParent.transform.childCount ? amountPowerUp : 0;
        for (int i = 0; i < powerUps; i++)
        {
            int brick = Random.Range(0, bricksParent.transform.childCount);
            while (bricksParent.GetChild(brick).GetComponent<BrickManager>().powerUp != null)
                brick = Random.Range(0, bricksParent.transform.childCount);
            bricksParent.GetChild(brick).GetComponent<BrickManager>().powerUp = powerUpObject;
            bricksParent.GetChild(brick).GetComponent<BrickManager>().powerUpType = powerUpTypes[i % powerUpTypes.Length];
        }
        isGameRunning = true;
    }

    void initLevel2()
    {
        runningLevel = 2;
        for (int i = 0; i < 8; i++)
        {
            var blockHealth = (i % 3) + 1;
            for (float j = -7.5f; j <= 7.5f; j += 1.5f)
            {
                if (j == 0) continue;
                var brick = Instantiate(brickObject, new Vector3(j, 0, i), Quaternion.identity, bricksParent);
                brick.gameObject.GetComponent<BrickManager>().health = blockHealth;
            }
        }

        int powerUps = amountPowerUp < bricksParent.transform.childCount ? amountPowerUp : 0;
        for (int i = 0; i < powerUps; i++)
        {
            int brick = Random.Range(0, bricksParent.transform.childCount);
            while (bricksParent.GetChild(brick).GetComponent<BrickManager>().powerUp != null)
                brick = Random.Range(0, bricksParent.transform.childCount);
            bricksParent.GetChild(brick).GetComponent<BrickManager>().powerUp = powerUpObject;
            bricksParent.GetChild(brick).GetComponent<BrickManager>().powerUpType = powerUpTypes[i % powerUpTypes.Length];
        }
        isGameRunning = true;
    }

    // Start is called before the first frame update
    void Awake()
    {
        initLevel1();
        updateHealthDisplay();
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (bricksParent.childCount == 0 && health > 0)
        {
            textResult.text = "Your score is " + score.ToString();
            if (runningLevel != 2)
            {
                textResultSmall.text = "Press Space to start next level";
            }
            else
            {
                textResultSmall.text = "Press Space to restart";
            }
            isGameRunning = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (runningLevel == 1)
                {
                    initLevel2();
                    multiplyPoints = 1;
                    health = 3;
                } else if (runningLevel == 2)
                {
                    initLevel1();
                    ResetGameState();
                }
                textResult.text = "";
                textResultSmall.text = "";
            }
        }
        if (health == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Transform child in bricksParent.transform)
            {
                Destroy(child.gameObject);
            }
            initLevel1();
            ResetGameState();
        }
    }

    void ResetGameState()
    {
        health = 3;
        score = 0;
        multiplyPoints = 1;
        textResult.text = "";
        textResultSmall.text = "";
        updateHealthDisplay();
        updateScoreDisplay();
        updateMultiplierDisplay();
    }
}
