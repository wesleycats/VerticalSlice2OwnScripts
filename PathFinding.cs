using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinding : MonoBehaviour
{
	public List<Node> path;
	public List<GameObject> allNodes;
	public Node clickedNode;
	public Node[] upperTeleportNodes;
	public Node[] lowerTeleportNodes;
	public GameObject boat;
	public bool clear;
	public bool correct;
	public int floorLevel, nodeIndex;
	public float seconds;

	public Node startNode, currentNode, nextNode, targetNode;

	bool stay, teleport;
	int direction;

	private void Awake()
	{
		path = new List<Node>();
	}

	public List<Node> FindPath(Node node)
	{
		teleport = true;
		if (node.walkable)
		{
			if (node == upperTeleportNodes[2])
			{
				stay = true;
				switch (floorLevel)
				{
					case 4:
						node = upperTeleportNodes[2];
						break;

					case 3:
						node = lowerTeleportNodes[2];
						break;
				}
			}
			else if (node == upperTeleportNodes[1])
			{
				stay = true;
				switch (floorLevel)
				{
					case 3:
						node = upperTeleportNodes[1];
						break;

					case 2:
						node = lowerTeleportNodes[1];
						break;
				}
			}
			else if (node == upperTeleportNodes[0])
			{
				stay = true;
				switch (floorLevel)
				{
					case 2:
						node = upperTeleportNodes[0];
						break;

					case 1:
						node = lowerTeleportNodes[0];
						break;
				}
			}

			direction = 0;
			clickedNode = node;
			startNode = clickedNode;
			currentNode = startNode;
			AddNodeToPath(currentNode);

			try
			{
				do
				{
					ChangeNextToNeighbour(currentNode);
					
					if (currentNode.neighbours.Length > startNode.neighbours.Length) startNode = currentNode;

					if (nextNode.walkable)
					{
						currentNode = nextNode;
						AddNodeToPath(currentNode);

						if (currentNode == targetNode)
						{
							correct = true;
							break;

						}
					}
					else
					{
						direction = 2;
						ChangeNextToNeighbour(currentNode);
						
						if (nextNode.walkable)
						{
							currentNode = nextNode;
							AddNodeToPath(currentNode);

							if (currentNode == targetNode)
							{
								correct = true;
								break;
							}
							else
							{
								clear = false;
								if (currentNode.neighbours[direction]) ; // To break out this loop and continue in the catch loop
							}
						}
						else
						{
							correct = false;
							ClearList(path);
							return path;
						}
					}
				} while (clear && currentNode.walkable && currentNode.neighbours[1]);
			}
			catch
			{
				direction = 1;
				if (clear)
				{
					ClearList(path);
					currentNode = clickedNode;
					AddNodeToPath(currentNode);
					currentNode = startNode;
					AddNodeToPath(currentNode);

					ChangeNextToNeighbour(currentNode);

					if (nextNode.walkable)
					{
						currentNode = nextNode;
						AddNodeToPath(currentNode);

						if (currentNode == clickedNode)
						{
							startNode = currentNode;
							ClearList(path);
							AddNodeToPath(currentNode);
						}

						clear = false;
						try
						{
							if (currentNode.neighbours[direction]) ; // To break out this loop and continue in the catch loop
						}
						catch
						{
							correct = false;
							ClearList(path);
							return path;
						}
					}
					else
					{
						correct = false;
						ClearList(path);
						return path;
					}
				}
				else
				{
					correct = false;
					ClearList(path);
					return path;
				}
				try
				{
					do
					{
						ChangeNextToNeighbour(currentNode);
						
						if (currentNode.neighbours.Length > startNode.neighbours.Length) startNode = currentNode;
						
						if (nextNode.walkable)
						{
							if (currentNode == targetNode)
							{
								correct = true;
								break;
							}
							else
							{
								currentNode = nextNode;
								AddNodeToPath(currentNode);

								if (currentNode == clickedNode)
								{
									startNode = currentNode;
									ClearList(path);
									AddNodeToPath(currentNode);
								}
							}

							if (currentNode == targetNode)
							{
								correct = true;
								break;
							}
						}
						else
						{
							direction = 2;
							ChangeNextToNeighbour(currentNode);
							
							if (nextNode.walkable)
							{
								currentNode = nextNode;
								AddNodeToPath(currentNode);

								if (currentNode == targetNode)
								{
									correct = true;
									break;
								}
								else
								{
									direction = 1;
									ChangeNextToNeighbour(currentNode);
									
									if (nextNode.walkable)
									{
										currentNode = nextNode;
										AddNodeToPath(currentNode);

										if (currentNode == targetNode)
										{
											correct = true;
											break;
										}
									}
								}
							}
							else
							{
								correct = false;
								ClearList(path);
								return path;
							}
						}
					} while (currentNode != targetNode);
				}
				catch
				{
					correct = false;
					ClearList(path);
					return path;
				}
			}
		}

		path.Reverse();
		PrintList(path);
		return path;
	}

	private void AddNodeToPath(Node node)
	{
		path.Add(node);
	}

	private void ChangeCurrentTo(Node node)
	{
		currentNode = node;
	}

	private void ChangeNextToNeighbour(Node node)
	{
		nextNode = node.GetComponent<Node>().neighbours[direction].GetComponent<Node>();
	}

	public int CheckFloor()
	{
		for (int i = 0; i < allNodes.Count; i++)
		{
			if (targetNode.gameObject == allNodes[i])
			{
				nodeIndex = i;
			}
		}

		if (nodeIndex >= 45)
		{
			floorLevel = 4;
		}
		else if (nodeIndex >= 32)
		{
			floorLevel = 3;
		}
		else if (nodeIndex >= 18)
		{
			floorLevel = 2;
		}
		else
		{
			floorLevel = 1;
		}
		
		return floorLevel;
	}

	private Node OneLessIndexNode()
	{
		Node node;
		node = null;
		for (int i = 0; i < allNodes.Count; i++)
		{
			if (targetNode.gameObject == allNodes[i])
			{
				try	{ node = allNodes[i - 1].GetComponent<Node>(); }
				catch {	}
			}
		}
		
		return node;
	}

	private Node OneMoreIndexNode()
	{
		Node node;
		node = null;
		for (int i = 0; i < allNodes.Count; i++)
		{
			if (targetNode.gameObject == allNodes[i])
			{
				try { node = allNodes[i + 1].GetComponent<Node>(); }
				catch { }
			}
		}

		return node;
	}

	public void Teleport(Node node)
	{
		if (teleport)
		{
			transform.position = node.transform.position;
			GetComponent<PlayerMovement>().pathIndex++;
			teleport = false;
		}
	}

	private IEnumerator TeleportDelay(Node node)
	{
		yield return new WaitForSeconds(seconds);
		Teleport(node);
	}

	public void ClearList(List<Node> list)
	{
		list.Clear();
	}

	void PrintList(List<Node> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			print(list[i].name);
		}
	}



	private void OnTriggerEnter(Collider other)
	{
		targetNode = other.GetComponent<Node>();
		if (!stay)
		{
			for (int i = 0; i < upperTeleportNodes.Length; i++)
			{
				if (targetNode.transform.position == upperTeleportNodes[i].transform.position)
				{
					StartCoroutine(TeleportDelay(OneLessIndexNode()));
				}
			}

			for (int i = 0; i < lowerTeleportNodes.Length; i++)
			{
				if (targetNode.transform.position == lowerTeleportNodes[i].transform.position)
				{
					StartCoroutine(TeleportDelay(OneMoreIndexNode()));
				}
			}
		}

		if (targetNode.transform.position == allNodes[allNodes.Count-1].transform.position)
		{
			transform.parent = boat.transform;
			boat.GetComponent<BoatMovement>().Move();
		}
		CheckFloor(); 
	}
}