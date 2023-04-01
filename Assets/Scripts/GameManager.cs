using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int health = 3 + 5;
    private int score = 0;
    // icons src https://semantic-ui.com/introduction/getting-started.html 
    const string filledHeart = "\uF004 ";
    const string emptyHeart = "\uF08A ";

    public TMP_Text textHealth;
    public TMP_Text textScore;
    public TMP_Text textResult;
    public Transform bricksParent;
    public GameObject brickObject;
    public GameObject powerUpObject;
    public static GameManager instance;
    public int amountPowerUp;

    private int multiplyPoints = 1;
    private string[] powerUpTypes = { "IncreasePaddle", "ExtraLive", "IncreaseBall" };
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
            textResult.text = "Game Over\nPress Space to restart";
        }
    }

    void updateScoreDisplay()
    {
        textScore.text = score.ToString();
    }

    void initLevel1()
    {
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
            bricksParent.GetChild(brick).GetComponent<BrickManager>().powerUp.GetComponent<PowerUp>().type = powerUpTypes[2];

        }
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
            textResult.text = "You won!\nYour score is " + score.ToString();
        }
        if (health == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Transform child in bricksParent.transform)
            {
                Destroy(child.gameObject);
            }
            initLevel1();
            health = 3;
            score = 0;
            textResult.text = "";
            updateHealthDisplay();
            updateScoreDisplay();
        }
    }
}
