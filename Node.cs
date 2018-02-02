using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public GameObject[] neighbours;
	public bool walkable;
    public bool forceWalk;

	// Use this for initialization
	void Start () {
		walkable = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
