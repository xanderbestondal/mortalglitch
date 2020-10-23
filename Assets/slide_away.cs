using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace Com.Oisoi.NahShop
{
	public class slide_away : MonoBehaviour
	{

		bool slide;
		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if (slide)
				transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition+ transform.up, Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{

			if (other.attachedRigidbody.GetComponent<PlayerManager>() != null)
			{
				if (other.attachedRigidbody.GetComponent<PhotonView>().IsMine)
				{
					slide = true;
				}
			}

		}
		private void OnTriggerExit(Collider other)
		{

			if (other.attachedRigidbody.GetComponent<PlayerManager>() != null)
			{
				if (other.attachedRigidbody.GetComponent<PhotonView>().IsMine)
				{
					slide = false;
				}
			}

		}
	}
}
