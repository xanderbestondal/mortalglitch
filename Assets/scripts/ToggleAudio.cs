using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ToggleAudio : MonoBehaviourPun
{

	AudioSource ass;
    // Start is called before the first frame update
    void Start()
    {
		// dont play audio from other players
		if (transform.parent.GetComponent<PhotonView>().IsMine == false)
		{
			gameObject.SetActive(false);
		}

		ass = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown("p"))
		{
			if (ass.isPlaying)
				ass.Pause();
			else
				ass.Play();
		}
    }
}
