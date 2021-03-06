﻿using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


namespace Com.Oisoi.NahShop
{
	public class GameManager : MonoBehaviourPunCallbacks
	{
		[Tooltip("The prefab to use for representing the player")]
		public GameObject playerPrefab;
		public GameObject instantiatedPlayer;

		#region Photon Callbacks

		string currentRoom;

		Vector3 posOnDisconnect;
		Quaternion rotOnDisconnect;

		public GameObject gameData;

		public GameObject BLOCKSTUFF;

		/// <summary>
		/// Called when the local player left the room. We need to load the launcher scene.
		/// </summary>
		public override void OnLeftRoom()
		{
			//print("Left room");
			//SceneManager.LoadScene(0);
			//Cursor.lockState = CursorLockMode.None;
		}


		#endregion


		#region Public Methods

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}

		private void Update()
		{

			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
				LeaveRoom();

			if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.D))
				if(gameData.activeSelf)
					gameData.SetActive(false);
				else
					gameData.SetActive(true);


			if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.B))
				if (BLOCKSTUFF.activeSelf)
					BLOCKSTUFF.SetActive(false);
				else
					BLOCKSTUFF.SetActive(true);
		}


		#endregion

		#region Private Methods

		private void Start()
		{
			currentRoom = PhotonNetwork.CurrentRoom.Name;
			
			// check if player is not remaining from recent disconnect
			//foreach (PlayerManager p in FindObjectsOfType<PlayerManager>())
			//{
			//	if (p.GetComponent<PhotonView>().IsMine)
			//	{
			//		print("RECOVERED PREVIOUS PLAYER INSTANCE");
			//		PlayerManager.LocalPlayerInstance = p.gameObject;
			//	}
			//}

			//Instantiate(playerPrefab);
			if (PlayerManager.LocalPlayerInstance == null)
			{
				Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				instantiatedPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(-3f, 0f, 0f), Quaternion.Euler(0,90,0), 0);
			}
			else
			{
				Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
			}
			
		}


		void LoadArena()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
			}
			Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
			//PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
			PhotonNetwork.LoadLevel("nahShop");
		}


		#endregion

		#region Photon Callbacks


		public override void OnPlayerEnteredRoom(Player other)
		{
			Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


				//LoadArena(); // this is from the tutorial.. no need to reload level when player joins in our game
			}
		}


		public override void OnPlayerLeftRoom(Player other)
		{
			Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


				//LoadArena();// this is from the tutorial.. no need to reload level when player leaves in our game
			}
		}

		


		public override void OnDisconnected(DisconnectCause cause)
		{
			print(cause + "TRYING TO REJOIN room:" + currentRoom);

			if (PlayerManager.LocalPlayerInstance != null)
			{
				posOnDisconnect = PlayerManager.LocalPlayerInstance.transform.position;
				rotOnDisconnect = PlayerManager.LocalPlayerInstance.transform.rotation;
			}

			StartCoroutine(MainReconnect());

			print("END OF REJOIN");

		}

		public override void OnErrorInfo(ErrorInfo errorInfo)
		{
			print(errorInfo.Info);
			//base.OnErrorInfo(errorInfo);
		}

		private IEnumerator MainReconnect()
		{
			while (PhotonNetwork.NetworkingClient.LoadBalancingPeer.PeerState != ExitGames.Client.Photon.PeerStateValue.Disconnected)
			{
				print("Waiting for client to be fully disconnected..");

				yield return new WaitForSeconds(0.2f);
			}

			print("Client is disconnected!");

			if (!PhotonNetwork.ReconnectAndRejoin())
			{
				if (PhotonNetwork.Reconnect())
				{
					print("Successful reconnected!");
				}
			}
			else
			{
				print("Successful reconnected and joined!");

				//if (PlayerManager.LocalPlayerInstance == null)
				//{
				//	instantiatedPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, posOnDisconnect, rotOnDisconnect, 0);

				//	print("RE-instantiated playerPrefab!");
				//}
				//print(PlayerManager.LocalPlayerInstance.name);
				//print(instantiatedPlayer.name);
				//PlayerManager.LocalPlayerInstance.transform.position = new Vector3(0, 0, 100);
			}
		}

		#endregion
	}
}