
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    const float width = 4.5f;
    const float height = 8.5f;
    public float snakeSpeed = 1;
    public BodyPart bodyPrefab = null;
    public Sprite tailSprite = null;
    public Sprite bodySprite = null;
    public SnakeHead snakeHead = null;
    public GameObject rockPrefab = null;
    public GameObject eggPrefab = null;
    public GameObject goldenEggPrefab = null;
    public GameObject spikesPrefab = null;
    public bool alive = true;
    public bool waitingToPlay = true;
    private Spike spike;

    public int initialSpikeCount = 5;
    public int spikeIncreasePerLevel = 2;

    List<Egg> eggs = new List<Egg>();
    List<Spike> spikes = new List<Spike>();

    int level = 0;
    int noOfEggsForNextLevel = 0;

    public int score = 0;
    public int highScore = 0;

    public Text scoreText = null;
    public Text highScoreText = null;
    public Text tapToPlayText = null;
    public Text gameOverText = null;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Debug.Log("Starting Snake Game");
        CreateWalls();
        alive = false;
        CreateSpikes();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingToPlay)
        {
            foreach(Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    StartGamePlay();
                }
            }
            if (Input.GetMouseButtonUp(0))
                StartGamePlay();
        }
    }

   
    public void GameOver()
    {
        alive = false;
        waitingToPlay = true;

        gameOverText.gameObject.SetActive(true);
        tapToPlayText.gameObject.SetActive(true);
        snakeHead.ResetSnake();
        
    }

    void StartGamePlay()
    {
        score = 0;
        level = 0;

        scoreText.text = "Score = " + score;
        highScoreText.text = "HighScore = " + highScore;

        gameOverText.gameObject.SetActive(false);
        tapToPlayText.gameObject.SetActive(false);

        waitingToPlay = false;
        alive = true;
        KillOldEggs();
        KillOldSpikes();

        LevelUp();
    }

    void LevelUp()
    {
        level++;
        noOfEggsForNextLevel = 4 + (level * 2); // 6 8 10
        snakeSpeed = 1f + (level / 4f);
        if (snakeSpeed > 6) snakeSpeed = 6;
        
        CreateEgg();
        CreateSpikes();
        KillOldSpikes();

    }
    public void EggEaten(Egg egg)
    {
        score++;
        noOfEggsForNextLevel--;
        if (noOfEggsForNextLevel == 0)
        {
            score += 10;
            LevelUp();
        }
        else if (noOfEggsForNextLevel == 1) // last egg
            CreateEgg(true);
        else
            CreateEgg(false);
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "HighScore = " + highScore;
        }
        scoreText.text = "Score = " + score;
        eggs.Remove(egg);
        Destroy(egg.gameObject);
    }
    void CreateWalls()
    {
        float z = -1f;
        Vector3 start = new Vector3(-width, -height, z);
        Vector3 finish = new Vector3(-width, +height, z);
        CreateWall(start, finish);

        start = new Vector3(+width, -height, z);
        finish = new Vector3(+width, +height, z);
        CreateWall(start, finish);

        start = new Vector3(-width, -height, z);
        finish = new Vector3(+width, -height, z);
        CreateWall(start, finish);

        start = new Vector3(-width, +height, z);
        finish = new Vector3(+width, +height, z);
        CreateWall(start, finish);
    }

    void CreateWall(Vector3 start, Vector3 finish)
    {
        float distance = Vector3.Distance(start, finish);
        int noOfRocks = (int)(distance * 3f);
        Vector3 delta = (finish - start) / noOfRocks;

        Vector3 position = start;
        for (int rock = 0; rock <= noOfRocks; rock++)
        {
            float rotation = Random.Range(0, 360f);
            float scale = Random.Range(1.5f, 2f);
            CreateRock(position, scale, rotation);
            position = position + delta;
        }
    }

    void CreateRock(Vector3 position, float scale, float rotation)
    {
        GameObject rock = Instantiate(rockPrefab, position, Quaternion.Euler(0, 0, rotation));
        rock.transform.localScale = new Vector3(scale, scale, 1);
    }

    void CreateEgg(bool golden = false)
    {
        Vector3 position;
        position.x = -width + Random.Range(1f, (width * 2) - 2f);
        position.y = -height + Random.Range(1f, (height * 2) - 2f);
        position.z = -1f;
        Egg egg = null;
        if (golden)
            egg =Instantiate(goldenEggPrefab, position, Quaternion.identity).GetComponent<Egg>();
        else
            egg = Instantiate(eggPrefab, position, Quaternion.identity).GetComponent<Egg>();
        eggs.Add(egg);
    }
    void KillOldEggs()
    {
        foreach(Egg egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }
    void CreateSpikes(bool obstacle = true)
    {
        Vector3 position;
        position.x = -width + Random.Range(1f, (width * 2) - 2f);
        position.y = -height + Random.Range(1f, (height * 2) - 2f);
        position.z = -1f;
        Spike spike = null;
        if (obstacle)
            spike = Instantiate(spikesPrefab, position, Quaternion.identity).GetComponent<Spike>();
        else
            spikes.Clear();
    }
    void KillOldSpikes()
    {
        foreach(Spike spike in spikes)
        {
            Destroy(spike.gameObject);
        }
        spikes.Clear();
    }
}
