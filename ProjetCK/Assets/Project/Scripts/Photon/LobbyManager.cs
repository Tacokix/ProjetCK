using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
 
public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Inspector
    [Header("Components")]

    [SerializeField] private GameObject _mainCanvas;

    #endregion

    #region Fields
	protected bool isConnecting = false;

    #endregion


    #region Monobehaviour
    private void Awake()
    {
        _mainCanvas.SetActive(false);
        Connect();
    }

    #endregion

    #region Methods
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            isConnecting = false;
            _mainCanvas.SetActive(true);
        }
    }
    /// <summary>
    /// Creates a photon room by giving an inputfield that gives the player count.
    /// </summary>
    /// <param name="playerCountInputField">The inputfield that gives the player count as an int.</param>
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.MaxPlayers = 2;

        roomOptions.CleanupCacheOnLeave = false; // Network objects won't be automatically destroyed when leaving the room

        PhotonNetwork.CreateRoom(string.Empty, roomOptions);

        _mainCanvas.SetActive(false);
    }

    protected void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            isConnecting = true;
            PhotonNetwork.KeepAliveInBackground = 180;
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = SettingsManager.GameVersion; // must be called AFTER the connection to the server
        }
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(SettingsManager.AppSceneName);
        }
    }

    #endregion
}
