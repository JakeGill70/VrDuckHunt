using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

    public Camera targetCamera;

	// Use this for initialization
	void Start () {
        targetCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        //Ray ray = targetCamera.ViewportPointToRay( new Vector2(0.5f, 0.5f) );
        //Debug.DrawRay( ray.origin, ray.direction );
	}
}
