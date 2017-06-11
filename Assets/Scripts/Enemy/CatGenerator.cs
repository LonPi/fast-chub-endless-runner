using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour {

    public GameObject CatPrefab;
    public float minInterval, maxInterval;

    float timer;
    float randomInterval;

    void Start()
    {
        GenerateRandomInterval();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (minInterval > 0.5f) minInterval -= 0.02f * Time.deltaTime;
        if (maxInterval > minInterval + 0.5f) maxInterval -= 0.02f * Time.deltaTime;

        if (timer >= randomInterval && !Player.Instance.PlayerDeath())
        {
            InstantiateEnemyObjects();
            GenerateRandomInterval();
            timer = 0f;
        }
    }

    void GenerateRandomInterval()
    {
        randomInterval = Random.Range(minInterval - 0.01f, maxInterval - 0.01f);
    }

    bool ObstacleOverlap(Bounds bounds)
    {
        // Perform area cast and make sure we are not on top of obstacles
        Collider2D[] hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max, 1 << LayerMask.NameToLayer("Ground"));
        foreach(Collider2D _collider in hits)
        {
            if (_collider.gameObject.tag == "Obstacle")
            {
                return true;
            }
        }
        return false;
    }

   
    bool InstantiateEnemyObjects()
    {
        GameObject catObject = PoolManager.instance.GetObjectfromPool(CatPrefab);
        catObject.GetComponent<Cat>().SetParams(transform.position);
        Bounds spriteBounds = catObject.GetComponent<SpriteRenderer>().bounds;
        if (!ObstacleOverlap(spriteBounds))
        {
            return true;
        }
        else
        {
            PoolManager.instance.ReturnObjectToPool(catObject);
            return false;
        }
    }
}
