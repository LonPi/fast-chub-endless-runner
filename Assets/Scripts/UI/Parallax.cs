using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public float scrollingSpeed;
    public Transform[] childLayers;
    float gapLength;
    float coveredDistance;

    void Start ()
    {
        gapLength = Mathf.Abs(childLayers[0].position.x - childLayers[1].position.x);
        coveredDistance = 0f;
        if (!Player.Instance.TutorialOn())
            scrollingSpeed += 2f;
    }

    void Update()
    {
        ScrollLeft();
        Stitch();
        scrollingSpeed += 0.1f * Time.smoothDeltaTime;
        if (Player.Instance.PlayerDeath()) scrollingSpeed = 0f;
    }

    void UpdateChildLayerIndex()
    {
        transform.GetChild(0).SetAsLastSibling();
    }

    void ScrollLeft()
    {
        for (int i = 0; i < childLayers.Length; i++)
        {
            childLayers[i].Translate(Vector2.left * scrollingSpeed * Time.smoothDeltaTime);
        }
        coveredDistance += scrollingSpeed * Time.smoothDeltaTime;
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
