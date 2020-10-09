using Photon.Pun;

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Com.Oisoi.NahShop
{
	public class ChatTextManager : MonoBehaviour
	{
		public TMP_InputField chatTextfield;
		public TextMeshProUGUI chatColumn;
		bool mssgSentThisFrame;

		string userName;
		string[] speakingList;

		saveData SaveData;

		//RectTransform customCursorRect;

		// Start is called before the first frame update
		void Start()
		{
			//customCursorRect = FindObjectOfType<customCursor>().GetComponent<RectTransform>();

			SaveData = GetComponent<saveData>();

			PlayerManager pm = FindObjectOfType<PlayerManager>();
			userName = pm.playerName.text;
			//lInputField.Select();
			//chatTextfield.ActivateInputField();

			speakingList = new string[]{ "says", "yells", "mumbles", "hisses" , "shouts", "muses", "spits", "hisses" };


			PhotonView.Get(this).RPC("ChatMessage", RpcTarget.All, "I joined ", userName);

			chatTextfield.onEndEdit.AddListener(delegate { sendMessage(); });
		}

		Rect GetWorldSapceRect(RectTransform rt)
		{
			var r = rt.rect;
			r.center = rt.TransformPoint(r.center);
			r.size = rt.TransformVector(r.size);
			return r;
		}


		// Update is called once per frame
		void Update()
		{

			// activate chat field with custom cursor.
			//if (GetWorldSapceRect(chatTextfield.GetComponent<RectTransform>()).Contains(GetWorldSapceRect(customCursorRect).center))
			//{
			//	if (Input.GetMouseButtonDown(0))
			//		chatTextfield.ActivateInputField();
			//}

			//print(chatTextfield.isFocused);
			if (Input.GetKeyDown("return"))
			{
				//print("SEBDSADK");
				//if (chatTextfield.text != "")
				//{
				//	// send message
				//	PhotonView.Get(this).RPC("ChatMessage", RpcTarget.All, chatTextfield.text, userName);
				//	chatTextfield.text = "";
				//	chatTextfield.ActivateInputField();
				//}
				//else
				//{
				// is always inactive bcs the textfield deactivates already when pressed enter ?
				if (mssgSentThisFrame)
				{
					mssgSentThisFrame = false;
				}
				else
				{
					if (!chatTextfield.isFocused)
					{
						//print("ACTUVAE");
						chatTextfield.placeholder.gameObject.SetActive(false);
						chatTextfield.ActivateInputField();
					}
				}
				//}
				//else
				//{

				//	chatTextfield.DeactivateInputField();
				//}
			}
		}

		void sendMessage()
		{
			// send message
			if (chatTextfield.text != "")
			{
				PhotonView.Get(this).RPC("ChatMessage", RpcTarget.All, chatTextfield.text, userName);
				chatTextfield.text = "";
			}
			chatTextfield.ReleaseSelection();
			mssgSentThisFrame = true;
			chatTextfield.placeholder.gameObject.SetActive(true);
		}


		[PunRPC]
		void ChatMessage(string b, string sendingUser, PhotonMessageInfo info)
		{
			// the photonView.RPC() call is the same as without the info parameter.
			// the info.Sender is the player who called the RPC.
			Debug.LogFormat("Info: {0} {1} {2}", info.Sender, info.photonView, info.SentServerTime);

			//string addedString =  '\n' + sendingUser + " " + speakingList[Random.Range(0, speakingList.Length)] + "  : " + b;
			string addedString =  '\n' + sendingUser + " " + "  : " + b;
			chatColumn.text = chatColumn.text + addedString;

			SaveData.username = userName;
			SaveData.updateChatMessage(addedString);
		}
	}
}