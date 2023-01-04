using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple_spawner : MonoBehaviour
{

    public Snake karakterController;
    private GameObject Spawn;
    public GameObject prefabObstacle;
    public GameObject pointObstacle;
    public double timeSpawn = 3f;
    public List<Sprite> listSpriteObstacle;

    public float spawnTime = 5f;        // The amount of time between each spawn.
    public float spawnDelay = 3f;       // The amount of time before spawning starts.
    public GameObject obstacleObj;
    public float[] heights;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeSpawn -= Time.deltaTime;
        if (timeSpawn <= 0)
        {
            double timeSpawnRand = Random.Range(1.0f, 4.0f);
            //GameObject newObstacle = Instantiate(prefabObstacle, transform.position, transform.rotation);
            //GameObject newObstacle2 = Instantiate(pointObstacle, newObstacle.transform.position, newObstacle.transform.rotation);

            //newObstacle.GetComponent<SpriteRenderer>().sprite = listSpriteObstacle[indexRand];
            //newObstacle2.GetComponent<SpriteRenderer>().sprite = listSpriteObstacle[indexRand];

            Spawn_Obstacle();

            timeSpawn = timeSpawnRand;
        }
    }


    void dup_cactus()
    {
        // duplikat kaktus
        Spawn = Instantiate(prefabObstacle, transform.position, Quaternion.identity) as GameObject;
    }

    public void StartSpawning()
    {
        //int indexRand = Random.Range(0, listSpriteObstacle.Count);


        //InvokeRepeating("Spawn_Obstacle", spawnDelay, spawnTime);
    }


    void Spawn_Obstacle()
    {
        //int heightIndex = Random.Range(0, heights.Length);
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        Instantiate(obstacleObj, pos, Quaternion.identity);
    }

    public void GameOver()
    {
        CancelInvoke("Spawn_Obstacle");
    }

}
