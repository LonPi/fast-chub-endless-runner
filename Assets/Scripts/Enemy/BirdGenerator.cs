using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGenerator : MonoBehaviour {

    public GameObject[] enemiesToGenerate;
    public float minInterval, maxInterval;

    float timer,
        randomInterval,
        _minHeight = -1f,
        _maxHeight = 1f;
    Vector2 _birdSpawn;

    void Start()
    {
        GenerateRandomInterval();
        if (!Player.Instance.TutorialOn())
        {
            minInterval -= 1f;
            maxInterval -= 1f;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (!Player.Instance.BirdHorde() && minInterval > 0.5f) minInterval -= 0.02f * Time.deltaTime;
        if (!Player.Instance.BirdHorde() && maxInterval > minInterval + 0.5f) maxInterval -= 0.02f * Time.deltaTime;
        if (timer >= randomInterval && !Player.Instance.PlayerDeath())
        {
            InstantiateEnemyObjects();
            GenerateRandomInterval();
            timer = 0f;
        }
    }

    void GenerateRandomInterval()
    {
        if (Player.Instance.BirdHorde())
            randomInterval = 0.5f;
        else
            randomInterval = Random.Range(minInterval - 0.01f, maxInterval - 0.01f);
    }

    bool ObstacleOverlap(Bounds bounds)
    {
        // Perform area cast and make sure we are not on top of obstacles
        Collider2D[] hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max, 1 << LayerMask.NameToLayer("Ground"));
        foreach(Collider2D _collider in hits)
        {
            if (_collider.gameObject.tag == "Enemy")
            {
                return true;
            }
        }
        return false;
    }

   
    bool InstantiateEnemyObjects()
    {
        _birdSpawn = new Vector2(transform.position.x, transform.position.y + Random.Range(_minHeight, _maxHeight));
        float minIndex = 0.01f;
        float maxIndex = enemiesToGenerate.Length - 0.01f;
        int randomIndex = (int)Mathf.Floor(Random.Range(minIndex, maxIndex));
        GameObject enemyPrefab = enemiesToGenerate[randomIndex];
        GameObject enemyObject = PoolManager.instance.GetObjectfromPool(enemyPrefab);
        enemyObject.GetComponent<Bird>().SetParams(_birdSpawn);
        Bounds spriteBounds = enemyObject.GetComponent<SpriteRenderer>().bounds;
        if (!ObstacleOverlap(spriteBounds))
        {
            //Instantiate(enemyClone, transform.position, Quaternion.identity);
            return true;
        }
        else
        {
            PoolManager.instance.ReturnObjectToPool(enemyObject);
            return false;
        }
    }
}
