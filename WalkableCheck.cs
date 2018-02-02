using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableCheck : MonoBehaviour {

	Ray ray;
	RaycastHit hit;

	// Use this for initialization
	void Start ()
	{
		ray.origin = transform.position;
		ray.direction = -transform.up;
	}

	// Update is called once per frame
	void Update()
	{
        if(GetComponent<Node>().forceWalk == false)
        {
            if (Physics.Raycast(ray, out hit, 200))
            {
                if (hit.transform.tag == "Block")
                {
                    GetComponent<Node>().walkable = true;
                }
                else
                {
                    GetComponent<Node>().walkable = false;
                }
            }

            Debug.DrawRay(ray.origin, ray.direction.normalized * hit.distance, Color.red);
        }
		
	}
}
