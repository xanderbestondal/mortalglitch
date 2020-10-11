using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviourPun
{

	#region "Variables"
	public Rigidbody Rigid;
	public float MouseSensitivity;
	public float MoveSpeed;
	public float JumpForce;

	public Animator anim;
	public float speed;
	Vector3 prevPos;

	public Transform camRot;
	public Transform camTarg;

	public Transform grabHandTransform;
	float grabLayerWeight = 0;
	Transform itemGrabbed;
	Transform itemUnGrabbed;

	float yRot;

	cameraMovement camMove;

	#endregion

	// Start is called before the first frame update
	void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;

		camMove = GetComponent<cameraMovement>();
	}

    // Update is called once per frame
    void Update()
    {

		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
		{
			return;
		}

		//ANIM
		anim.SetFloat("speed", Mathf.Abs( Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")) );


		// ROT
		//Rigid.MoveRotation(Rigid.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * MouseSensitivity, 0)));
		transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * MouseSensitivity, 0) ) ;

		camRot.Rotate(-Input.GetAxis("Mouse Y") * MouseSensitivity, 0, 0, Space.Self);

		float SignedAngle = Vector3.SignedAngle(camRot.parent.up, camRot.TransformDirection(0, 1, 0), camRot.right);

		// limit rot
		if (SignedAngle < -80)
			camRot.localEulerAngles = new Vector3(-80,0,0);
		else if (SignedAngle > 80)
			camRot.localEulerAngles = new Vector3(80, 0, 0);
		
		// consume
		if (SignedAngle < -30)
		{
			if(itemGrabbed != null)
			{
				anim.SetBool("consume", true);
			}
			else
			{
				anim.SetBool("consume", false);
			}
		}
		else
		{
			anim.SetBool("consume", false);
		}


		// MOVE
		Rigid.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime) + (transform.right * Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime));
		if (Input.GetKeyDown("space"))
			Rigid.AddForce(transform.up * JumpForce);


		// GRAb
		if (Input.GetMouseButtonDown(0))
		{
			grabbstuff();
		}
		if (itemGrabbed == null)
			grabLayerWeight -= .2f * Time.deltaTime;
		anim.SetLayerWeight(1, grabLayerWeight); // should go back to 0 if nothing is grabbed


	}

	void LateUpdate()
	{
		// executed on all clients
		anim.GetBoneTransform(HumanBodyBones.Chest).localRotation = camRot.localRotation;
		

		// after this execute only for local player
		if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
		{
			return;
		}


		// SCALE HEAD
		if (camMove.camDist < .05f)
			anim.GetBoneTransform(HumanBodyBones.Neck).localScale = Vector3.zero;
		else
			anim.GetBoneTransform(HumanBodyBones.Neck).localScale = Vector3.one;
	}

	void grabbstuff() {
		//ANIM
		grabLayerWeight = 1;
		anim.SetTrigger("grab");
			
		// ungrab whatever is grabbed atm
		ungrab_parent();
		if(itemUnGrabbed != null) // also ungrab at clients
			if(itemUnGrabbed.GetComponent<PhotonView>() != null)
				PhotonView.Get(this).RPC("unGrabOnAllClients", RpcTarget.OthersBuffered, itemUnGrabbed.GetComponent<PhotonView>().ViewID); // send objPhotonViewId and playerPhotonViewId

		RaycastHit hit;
		// Does the ray intersect any objects
		if (Physics.Raycast(camTarg.position, camTarg.TransformDirection(Vector3.forward), out hit, 3)) // 3 = max distance
		{
			if (hit.transform.tag == "grabable")
			{
				itemGrabbed = hit.transform;

				// if we are grabbing the itemUnGrabbed, prevent that and set itemGrabbed to null
				if (itemGrabbed == itemUnGrabbed)
				{
					itemGrabbed = null;
				}
				else
				{
					// GRAB !
					grabParent();

					PhotonView photonView_grabbedItem = itemGrabbed.GetComponent<PhotonView>();
					if (photonView_grabbedItem != null)
					{
						photonView_grabbedItem.TransferOwnership(PhotonNetwork.LocalPlayer);
						// send message to other clients that this object should be kinematic. and stopped parenting from hand of player at the other clients side
						PhotonView.Get(this).RPC("grabOnAllClients", RpcTarget.OthersBuffered, photonView_grabbedItem.ViewID, GetComponent<PhotonView>().ViewID); // send objPhotonViewId and playerPhotonViewId
					}
				}
			}
		}
		

	}

	public void grabParent()
	{
		itemGrabbed.SetParent(grabHandTransform);
		itemGrabbed.localPosition = Vector3.zero;
		itemGrabbed.localRotation = Quaternion.identity;

		itemGrabbed.GetComponent<Rigidbody>().isKinematic = true;
		itemGrabbed.GetComponent<Collider>().isTrigger = true;
		if(itemGrabbed.GetComponent<PhotonView>() != null)
			itemGrabbed.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off; // stop syncing, its parented, for now
	}

	[PunRPC]
	void grabOnAllClients(int object_viewId, int player_viewId, PhotonMessageInfo info) // sent to all users including the sending one
	{
		// the photonView.RPC() call is the same as without the info parameter.
		// the info.Sender is the player who called the RPC.

		Transform grabbedObj = PhotonView.Find(object_viewId).GetComponent<Transform>();
		move sendingPlayerMoveScript = PhotonView.Find(player_viewId).GetComponent<move>();

		// force current grabbing player to ungrab
		if(grabbedObj.GetComponentInParent<move>() != null)	
			grabbedObj.GetComponentInParent<move>().ungrab_parent();

		// grab on other clients side
		sendingPlayerMoveScript.itemGrabbed = grabbedObj;
		sendingPlayerMoveScript.grabParent();
	}

	[PunRPC]
	void unGrabOnAllClients(int ungrabobject_viewId, PhotonMessageInfo info) // sent to all other users
	{
		Transform grabbedObj = PhotonView.Find(ungrabobject_viewId).GetComponent<Transform>();
		print("ungrabbing rpc");
		grabbedObj.GetComponentInParent<move>().ungrab_parent();
	}

	public void ungrab_parent()
	{
		
		itemUnGrabbed = null;
		if (itemGrabbed != null)
		{
			itemGrabbed.SetParent(null);
			//if (itemGrabbed.GetComponent<Rigidbody>() != null)
			itemGrabbed.GetComponent<Rigidbody>().isKinematic = false;
			itemGrabbed.GetComponentInChildren<Collider>().isTrigger = false;
			itemGrabbed.GetComponent<Rigidbody>().AddForce(camTarg.TransformDirection(Vector3.forward)*333);
			if(itemGrabbed.GetComponent<PhotonView>() != null)
				itemGrabbed.GetComponent<PhotonView>().Synchronization = ViewSynchronization.UnreliableOnChange; // stop syncing, its parented, for now

			anim.SetTrigger("throw");

			itemUnGrabbed = itemGrabbed;
			itemGrabbed = null;

		}
	}
}
