using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {

    public GameObject[] ObstaclesToGenerate;
    public float minInterval, maxInterval;

    float timer;
    float randomInterval;
    int randomObstacleHeight;

    void Start ()
    {
        GenerateRandomInterval();
	}

    void Update ()
    {
        timer += Time.deltaTime;
        
        if (timer >= randomInterval)
        {
            InstantiateGameObjects();
            timer = 0f;
            GenerateRandomInterval();
        }
    }

    void GenerateRandomInterval()
    {
        randomInterval = Random.Range(minInterval - 0.01f, maxInterval - 0.01f);
    }

    void InstantiateGameObjects()
    {
        float minIndex = 0.01f;
        float maxIndex = ObstaclesToGenerate.Length - 0.01f;
        int randomIndex = (int)Mathf.Floor(Random.Range(minIndex, maxIndex));
        Instantiate(ObstaclesToGenerate[randomIndex], transform.position, Quaternion.identity);
    }
}
