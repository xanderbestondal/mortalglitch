using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Oisoi.NahShop {
	public class ToggleAudio : MonoBehaviourPun
	{
		public AudioClip sideA;
		public AudioClip sideB;
		
		AudioSource ass;

		ChatTextManager chatTextManager;
		// Start is called before the first frame update
		void Start()
		{


			// dont play audio from other players
			if (transform.parent.GetComponent<PhotonView>().IsMine == false)
			{
				gameObject.SetActive(false);
			}

			ass = GetComponent<AudioSource>();


			chatTextManager = FindObjectOfType<ChatTextManager>();
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown("p"))
			{
				if (!chatTextManager.chatTextfield.isFocused)
				{
					if (ass.isPlaying)
						ass.Pause();
					else
						ass.Play();
				}
			}


			//if (!audioSource.isPlaying)
			//{
			//	audioSource.clip = otherClip;
			//	audioSource.Play();
			//}
		}
	}
}