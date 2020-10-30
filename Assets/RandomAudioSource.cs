using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioSource : MonoBehaviour
{

	public AudioClip[] audioClips;
	AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = GetComponent<AudioSource>();

	}

    // Update is called once per frame
    void Update()
    {

		if (Random.value < .13f * Time.deltaTime)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.clip = audioClips[(int)(audioClips.Length * Random.value)];
				audioSource.Play();
			}
		}
	}
}
