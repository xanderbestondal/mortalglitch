
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace Com.Oisoi.NahShop
{
	public class TriggerVisiblity : MonoBehaviour
	{

		public GameObject[] setObjectObjects;
		public bool disableAtStart;
		public bool disable;

		private void Start()
		{
			if (disableAtStart)
				foreach (GameObject go in setObjectObjects)
				{
					go.SetActive(false);
				}

		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody.GetComponent<PlayerManager>() != null)
			{

				if (other.attachedRigidbody.GetComponent<PhotonView>().IsMine)
				{
					foreach (GameObject go in setObjectObjects)
					{
						go.SetActive(true);
					}
					if (disable)
					{
						foreach (GameObject go in setObjectObjects)
						{
							go.SetActive(false);
						}
					}
				}

			}
		}


	}
}