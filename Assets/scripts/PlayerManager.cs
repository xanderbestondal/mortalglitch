using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

using Photon.Pun;

using System.Collections;

using TMPro;
namespace Com.Oisoi.NahShop
{
	/// <summary>
	/// Player manager.
	/// Handles fire Input and Beams.
	/// </summary>
	public class PlayerManager : MonoBehaviourPunCallbacks
	{
		public TextMeshPro playerName;

		[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
		public static GameObject LocalPlayerInstance;

		#region MonoBehaviour CallBacks

		saveData SaveData;

		void Start()
		{
			cameraMovement _cameraWork = GetComponent<cameraMovement>();

			if (photonView.IsMine)
			{
				_cameraWork.OnStartFollowing();
			}
			//else
			//{
				// if this player is not controlled locally, then let its name face to the camera
				AimConstraint aim = playerName.gameObject.AddComponent<AimConstraint>();
				ConstraintSource css = new ConstraintSource();
				css.weight = 1;
				css.sourceTransform = Camera.main.transform;
				aim.AddSource(css);
				aim.constraintActive = true;	
			//}


		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
		/// </summary>
		void Awake()
		{
			// #Important
			// used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
			if (photonView.IsMine)
			{
				LocalPlayerInstance = gameObject;
			}
			// #Critical
			// we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
			DontDestroyOnLoad(this.gameObject);

			if (photonView.Owner != null)
			{
				playerName.text = photonView.Owner.NickName;

				//SaveData = GetComponent<saveData>();
				//if (SaveData != null)
				//{
				//	SaveData.username = photonView.Owner.NickName;
				//}
			}
		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity on every frame.
		/// </summary>
		void Update()
		{

			ProcessInputs();
			
			//if (SaveData != null)
			//{
			//	SaveData.updatePosRotMessage();
			//}
		}

		#endregion

		#region Custom

		/// <summary>
		/// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
		/// </summary>
		void ProcessInputs()
		{
			//	if (Input.GetButtonDown("Fire1"))
			//	{
			//		if (!IsFiring)
			//		{
			//			IsFiring = true;
			//		}
			//	}
			//	if (Input.GetButtonUp("Fire1"))
			//	{
			//		if (IsFiring)
			//		{
			//			IsFiring = false;
			//		}
			//	}
		}

		#endregion
	}
}