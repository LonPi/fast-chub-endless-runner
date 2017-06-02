using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public float scrollingSpeed;
    public Transform[] childLayers;
    float gapLength;
    Vector2 initialPosition;
    float coveredDistance;

	void Start ()
    {
        gapLength = Mathf.Abs(childLayers[0].position.x - childLayers[1].position.x);
        initialPosition = transform.localPosition;
        coveredDistance = 0f;
	}
	
	void Update ()
    {
        ScrollLeft();
        Stitch();
	}

    void UpdateChildLayerIndex()
    {
        transform.GetChild(0).SetAsLastSibling();
    }

    void ScrollLeft()
    {
        for (int i = 0; i < childLayers.Length; i++)
        {
            childLayers[i].Translate(Vector2.left * scrollingSpeed * Time.deltaTime);
        }
        coveredDistance += scrollingSpeed * Time.deltaTime;
    }

    void Stitch()
    {
        Transform firstChild = transform.GetChild(0);
        Transform lastChild = transform.GetChild(transform.childCount - 1);

        if (coveredDistance >= gapLength)
        {
            firstChild.position = new Vector2(lastChild.position.x + gapLength, lastChild.position.y);
            coveredDistance = 0f;
            UpdateChildLayerIndex();
        }
    }
}
