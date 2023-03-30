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
    public GameObject textGameover;
    public GameObject brickObject;
    public static GameManager instance;

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
    public int Score
    {
        set
        {
            score = value;
            updateScoreDisplay();
        }
        get
        {
            return score;
        }
    }
    void updateHealthDisplay()
    {
        string healthState = "";
        for (int i = 0; i < 3; i++)
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
            textGameover.gameObject.SetActive(true);
        }
    }

    void updateScoreDisplay() => textScore.text = score.ToString();

    void initLevel1()
    {
        for (int j = 0; j < 8; j++)
        {
            var blockHealth = (j % 3) + 1;
            for (float i = -7.5f; i <= 7.5f; i += 1.5f)
            {
                if (i == 0) continue;
                var brick = Instantiate(brickObject, new Vector3(i, 0, j), Quaternion.identity);
                brick.gameObject.GetComponent<BrickManager>().health = blockHealth;
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        initLevel1();
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
