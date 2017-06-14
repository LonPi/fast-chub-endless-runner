using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

    Animator _animator;

    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!Player.Instance.ShieldOn())
            _animator.SetTrigger("shieldDown");
        if (_animator.GetBool("pop"))
        {
            _animator.SetBool("pop", false);
            gameObject.SetActive(false);
        }
    }
}
