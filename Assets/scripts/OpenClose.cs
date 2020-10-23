using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Oisoi.NahShop
{
	public class OpenClose : MonoBehaviourPun
	{

		bool open;

		Vector3 closedAngle;

		Transform cam;

		AudioSource ass;

		// Start is called before the first frame update
		void Start()
		{
			closedAngle = transform.TransformDirection(0, 1, 0);
			cam = Camera.main.transform;
			ass = GetComponent<AudioSource>();

		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit hit;
				// Does the ray intersect any objects
				if (Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, 4)) //  = max distance
				{
					if (hit.transform == transform)
					{
						if (Vector3.Angle(closedAngle, transform.TransformDirection(0, 1, 0)) < 60)
						{
							opendoor(100);
							PhotonView.Get(this).RPC("sendForceToOthers", RpcTarget.OthersBuffered, 100); // send objPhotonViewId and playerPhotonViewId
						}
						else
						{
							opendoor(-100);
							PhotonView.Get(this).RPC("sendForceToOthers", RpcTarget.OthersBuffered, -100); // send objPhotonViewId and playerPhotonViewId
						}
					}
				}
			}
		}

		private void opendoor(int force)
		{
			GetComponent<Rigidbody>().AddTorque(0, force, 0);
			ass.Play();
		}

		[PunRPC]
		void sendForceToOthers(int force, PhotonMessageInfo info)
		{
			opendoor(force);
		}
	}
}