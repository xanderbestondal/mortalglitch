using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using UnityEngine.AI;

public class moveAgents : MonoBehaviourPun
{


	NavMeshAgent[] agents;
	Vector3 origPos;

	void Start()
	{
		agents = FindObjectsOfType<NavMeshAgent>();
		origPos = transform.position;
	}


	private void Update()
	{
		for (int i = 0; i < agents.Length; i++)
		{
			agents[i].destination = transform.position;
		}

		if (PhotonNetwork.IsMasterClient)
			UpdatePos();
	}


	private void UpdatePos()
	{
		if(Random.value < .13f*Time.deltaTime)
		{
			transform.position = origPos + new Vector3(Random.value * 20 - 10, 0, Random.value * 20 - 10);
			PhotonView.Get(this).RPC("updateOnClients", RpcTarget.OthersBuffered, transform.position); // send objPhotonViewId and playerPhotonViewId
		}
	}


	[PunRPC]
	void updateOnClients(Vector3 pos, PhotonMessageInfo info)
	{
		transform.position = pos;
	}
}
