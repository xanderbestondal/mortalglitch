using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class animateAgent : MonoBehaviour
{

	Animator anim;
	NavMeshAgent agent;

	public float veleocity;

	//LineRenderer lineTmp;

	// Start is called before the first frame update
	void Start()
	{
		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();

		//lineTmp = gameObject.AddComponent<LineRenderer>();
	}

    // Update is called once per frame
    void Update()
    {
		veleocity = agent.desiredVelocity.magnitude;
		//if (agent.remainingDistance > 5)

		anim.SetFloat("speed", veleocity);
		//anim.SetInteger("random",Random.Range(0, 2));

		//if (agent.remainingDistance > 1.5f)
		//	agent.speed = 1.5f;
		//else
		//	agent.speed = 1;


		//lineTmp.SetPositions(agent.path.corners);
	}
}
