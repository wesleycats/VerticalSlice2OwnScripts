using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour {

	public GameObject player;
	public float moveSpeed;

	float step;
	bool move;

	// Use this for initialization
	void Start () {
		step = moveSpeed * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(move)
		{
			Move();
		}
	}

	public void Move()
	{
		if (transform.localPosition.z < 20)
		{
			move = true;
			transform.Translate(Vector3.forward * step);
		}
		else
		{
			move = false;
		}
	}
}
