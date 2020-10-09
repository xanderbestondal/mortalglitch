using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class moveAgents : MonoBehaviour
{


	NavMeshAgent[] agents;
	Vector3 origPos;

	void Start()
	{
		agents = FindObjectsOfType<NavMeshAgent>();
		origPos = transform.position;
		
	}

	// Update is called once per frame
	void Update()
    {
		
		for (int i = 0; i < agents.Length; i++)
		{
			//if((transform.position - agents[i].transform.position).magnitude > 1) // if distance is bigger than this . set a new destination
			agents[i].destination = transform.position;
		}


		if(Random.value < .003f)
		{
			transform.position = origPos + new Vector3(Random.value * 20 - 10, 0, Random.value * 20 - 10);
		}
	}
}
