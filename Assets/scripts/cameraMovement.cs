﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{

	[Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
	[SerializeField]
	private bool followOnStart = false;

	// cached transform of the target
	Transform cameraTransform;

	public Transform cameraTransform_target;
	public Transform cameraTransform_rot;
	public float camDist = 0;
	public float camDistFromCollission = 0.01f;

	// maintain a flag internally to reconnect if target is lost or camera is switched
	bool isFollowing;

	void Start()
	{
		// Start following the target if wanted.
		if (followOnStart)
		{
			OnStartFollowing();
		}
	}


	void LateUpdate()
	{
		// The transform target may not destroy on level load, 
		// so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
		if (cameraTransform == null && isFollowing)
		{
			OnStartFollowing();
		}

		// only follow is explicitly declared
		if (isFollowing)
		{
			Follow();
		}
	}

	public void OnStartFollowing()
	{

		cameraTransform = Camera.main.transform;
		isFollowing = true;

	}

	void Follow()
	{

		camDist -= Input.mouseScrollDelta.y;
		camDist = Mathf.Clamp(camDist, 0, 10);

		cameraTransform_target.localPosition = Vector3.Lerp(cameraTransform_target.localPosition, new Vector3(0, 0, -camDist) , .1f) ;

		cameraTransform.position = cameraTransform_target.position;
		cameraTransform.rotation = cameraTransform_target.rotation;

		RaycastHit hit;
		// Does the ray intersect any objects
		if (Physics.Raycast(cameraTransform_target.parent.position, cameraTransform_target.parent.TransformDirection(Vector3.back), out hit, camDist+.2f))
		{
			
			Debug.DrawRay(cameraTransform_target.parent.position, cameraTransform_target.parent.TransformDirection(Vector3.back) * hit.distance, Color.yellow);

			if (hit.distance > camDist) // we raycast a bit further(camdist) to be sure were not barely touching a perpendicular wall which we migh see through
			{ hit.distance = camDist; }
			cameraTransform.position =	cameraTransform_target.parent.position + 
										cameraTransform_target.parent.TransformDirection(Vector3.back) * hit.distance * camDistFromCollission;
		}
		//else
		//{
		//	Debug.DrawRay(cameraTransform_target.position, cameraTransform_target.TransformDirection(Vector3.back) * 1000, Color.white);
		//}




	}
}
