using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manager_controller : MonoBehaviour
{

    //private Transform[] spawnpoint = new Transform[0];
    //private Transform[] spawnpointSpawner = new Transform[0];

    [Header("Score Variables")]
    public Text scoreText;
    public Text scoreTextGameOver;
    public int score = 0;
    public int EnemyScore = 100;

    [Header("Other")]
    public int enemysToSpawn = 3;
    [Space(20)]

    private int currentNumberOfEnemys;
    private int currHealth;


    [Header("Lists")]
    public List<Transform> spawnpoint = new List<Transform>();
    public List<Transform> spawnpointSpawner = new List<Transform>();
    public List<GameObject> healthsystem = new List<GameObject>();


    [Header("Game Objects")]
    public GameObject SpawnCirkle;
    public GameObject EnemyClose;
    public GameObject EnemyRange;
    public GameObject GameOver;

    private GameObject player;



    void Start()
    {
        //spawnpoint = GetComponentsInChildren<Transform>();
        spawnEnemys();

        currentNumberOfEnemys = enemysToSpawn;

        player = GameObject.FindGameObjectWithTag("Player");
        currHealth = player.GetComponent<Player_controller>().life;
    }

    private void Update()
    {
        manageLife();
    }

    void spawnEnemys()
    {

        spawnpointSpawner = new List<Transform>(spawnpoint);

        for (int i = 0; i < enemysToSpawn; i++)
        {
            if (enemysToSpawn > spawnpointSpawner.Count) // ser till att inte fler fienden spawnar än spawpositioner
            {
                enemysToSpawn = spawnpointSpawner.Count;
            }

            Random.InitState(System.DateTime.Now.Millisecond);

            int randPoss = (int)Random.Range(0, spawnpointSpawner.Count);
            //Debug.Log(randPoss);
            int randEnemy = (int)Random.Range(1, 2 + 1);

            //Debug.Log(randEnemy);

            if (randEnemy == 1)
            {
                StartCoroutine(SpawnRutine(randPoss, EnemyClose));
                //spawn(randPoss, EnemyRange);

            }
            else if (randEnemy == 2)
            {
                StartCoroutine(SpawnRutine(randPoss, EnemyRange));
                //spawn(randPoss, EnemyRange);
            }
            else
            {
                Debug.Log("Fel Seed");
            }


            spawnpointSpawner.RemoveAt(randPoss);
          
        }
    }

    public void enemyKilled()
    {
        currentNumberOfEnemys--;

        score += EnemyScore;
        scoreText.text = score.ToString();

        if (currentNumberOfEnemys <= 0)
        {
            enemysToSpawn++;

            currentNumberOfEnemys = enemysToSpawn;

            spawnEnemys();
        }
    }

    private IEnumerator SpawnRutine(int spawnIndex, GameObject enemy)
    {
        //spawn tecken
        GameObject temp_cirkle;
        GameObject temp_enemy;

        temp_cirkle = Instantiate(SpawnCirkle, spawnpointSpawner[spawnIndex].gameObject.transform.position, spawnpoint[spawnIndex].gameObject.transform.rotation);

        temp_enemy = Instantiate(enemy, spawnpointSpawner[spawnIndex].gameObject.transform.position, spawnpoint[spawnIndex].gameObject.transform.rotation); //instansieras över för att inte det ska bli fel i listan av att vänta

        temp_enemy.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        Destroy(temp_cirkle);

        temp_enemy.SetActive(true);

        //Debug.Log("spawnIndex   " + spawnIndex + "  count  " + spawnpointSpawner.Count);

       



        //spawn enemy
    }


    void manageLife()
    {
       if(currHealth != player.GetComponent<Player_controller>().life)
        {
            currHealth = player.GetComponent<Player_controller>().life;

            healthsystem[player.GetComponent<Player_controller>().life].SetActive(false);

            if(player.GetComponent<Player_controller>().life <= 0) // game over
            {
                GameOver.SetActive(true);

                scoreTextGameOver.text = score.ToString(); 

                Time.timeScale = 0;
            }
        }
    }
}
