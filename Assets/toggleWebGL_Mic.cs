using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrostweepGames.Plugins.Native;

namespace FrostweepGames.WebGLPUNVoice.Examples
{ 
	public class toggleWebGL_Mic : MonoBehaviour
	{


		public Recorder recorder;

		bool isRecording;

		// Start is called before the first frame update
		void Start()
		{

			CustomMicrophone.RequestMicrophonePermission();

		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKey("r"))
			{
				toggleRecording();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			toggleRecording();
		}

		void toggleRecording()
		{

			if (!isRecording)
			{
				recorder.StartRecord();
				isRecording = true;
			}
			else
			{
				recorder.StopRecord();
				isRecording = false;
			}
		}
	}
}