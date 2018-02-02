using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
	Ray ray;
	RaycastHit hit;
	PathFinding pfScript;
	PlayerMovement pmScript;

	private void Start()
	{
		pfScript = this.GetComponent<PathFinding>();
		pmScript = this.GetComponent<PlayerMovement>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			pfScript.clear = true;
		}
		else
		{
			ray.origin = new Vector3(0, 0, 0);
			pfScript.clickedNode = null;
		}
		
		if (Physics.Raycast(ray, out hit, 100))
		{
			if (hit.transform.tag == "WalkNode")
			{
				Debug.LogWarning("Clicked node: " + hit.transform.name);
				pfScript.ClearList(pfScript.path);
				pmScript.pathIndex = 0;
				pmScript.path = pfScript.FindPath(hit.transform.gameObject.GetComponent<Node>());
				if (pfScript.correct) { pmScript.move = true; }
			}
		}
	}
}
