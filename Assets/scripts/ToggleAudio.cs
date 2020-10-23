using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Oisoi.NahShop {
	public class ToggleAudio : MonoBehaviourPun
	{
		public AudioClip sideA;
		public AudioClip sideB;

		bool isPaused;

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
					{
						ass.Pause();
						isPaused = true;
					}
					else
					{
						ass.Play();
						isPaused = false;
					}
				}
			}


			if (!ass.isPlaying && !isPaused)
			{

				if (ass.clip == sideA)
				{
					ass.clip = sideB;
					print("endoOfTrack");
				}else
				{
					ass.clip = sideA;
					print("endoOfTrack _B");
				}

				ass.Play();
			}

		}
	}
}