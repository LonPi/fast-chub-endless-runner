using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float moveSpeed;
    Vector2 targetPosition;
    Vector2 direction;
    bool hasReachedTarget;

	void Start () {
        hasReachedTarget = false;
	}

	void Update () {
        if (!hasReachedTarget)
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if ((Vector2)transform.position == targetPosition)
        {
            hasReachedTarget = true;
        }

        if (hasReachedTarget)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

    }

    public void SetParams(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
        direction = this.targetPosition.normalized;
    }
}
