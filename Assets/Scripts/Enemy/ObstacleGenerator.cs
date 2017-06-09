using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {

    public GameObject[] ObstaclesToGenerate;
    public float minInterval, maxInterval;

    float timer;
    float randomInterval;
    float minScale = 1f;
    float maxScale = 1.8f;

    void Start ()
    {
        GenerateRandomInterval();
	}

    void Update ()
    {
        timer += Time.deltaTime;
        if (minInterval > 0.5f) minInterval -= 0.04f * Time.deltaTime;
        if (maxInterval > minInterval + 0.5f) maxInterval -= 0.04f * Time.deltaTime;

        if (timer >= randomInterval && !Player.Instance.PlayerDeath())
        {
            InstantiateObstacleObjects();
            timer = 0f;
            GenerateRandomInterval();
        }
    }

    void GenerateRandomInterval()
    {
        randomInterval = Random.Range(minInterval - 0.01f, maxInterval - 0.01f);
    }

    bool ObstacleOverlap(Bounds bounds)
    {
        // Perform area cast and make sure we are not on top of obstacles
        Collider2D[] hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider2D _collider in hits)
        {
            if (_collider.gameObject.tag == "Enemy" )
            {
                return true;
            }
        }
        return false;
    }

    bool InstantiateObstacleObjects()
    {
        float minIndex = 0.01f;
        float maxIndex = ObstaclesToGenerate.Length - 0.01f;
        int randomIndex = (int)Mathf.Floor(Random.Range(minIndex, maxIndex));
        GameObject obstaclePrefab = ObstaclesToGenerate[randomIndex];
        GameObject obstacleObject = PoolManager.instance.GetObjectfromPool(obstaclePrefab);
        obstacleObject.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
        Bounds spriteBounds = obstacleObject.GetComponent<SpriteRenderer>().bounds;
        if (!ObstacleOverlap(spriteBounds))
        {
             obstacleObject.GetComponent<Obstacles>().SetParams(transform.position);
            //Instantiate(enemyClone, transform.position, Quaternion.identity);
            return true;
        }
        else
        {
            PoolManager.instance.ReturnObjectToPool(obstacleObject);
            return false;
        }
    }
}
