using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SettingsManager : MonoBehaviour
{

    #region Inspector
    [Header("Settings")]

    [Tooltip("Sync the scene between the master client and the other clients in the room.")]
    [SerializeField] private bool _syncScene;

    [Tooltip("Users are separated from each other by photon's gameVersion.")]
    [SerializeField] private string _gameVersion;

    /// <summary>
    /// Users are separated from each other by photon's gameVersion.
    /// </summary>
    public static string GameVersion
    {
        get
        {
            return Instance._gameVersion;
        }
    }

    [Tooltip("Set to true if the users try to reconnect to the play scene after a disconnection. Otherwise, players will all leave the play scene when someone gets disconnected.")]
    [SerializeField] private bool _tryReconnectWhenDisconnected;

    /// <summary>
    /// Set to true if the users try to reconnect to the play scene after a disconnection. Otherwise, players will all leave the play scene when someone gets disconnected.
    /// </summary>
    public static bool TryReconnectWhenDisconnected
    {
        get
        {
            return Instance._tryReconnectWhenDisconnected;
        }
    }

    [Tooltip("Show some debug informations.")]
    [SerializeField] private bool _verbose;

    /// <summary>
    /// Show some debug informations.
    /// </summary>
    public static bool Verbose
    {
        get { return Instance._verbose; }
    }

    [Tooltip("The remaining duration of a player in a room after its disconnection.")]
    [SerializeField] private int _playerTTL;

    /// <summary>
    /// The remaining duration of a player in a room after its disconnection.
    /// </summary>
    public static int PlayerTTL
    {
        get { return Instance._playerTTL; }
    }

    [Tooltip("The scene to load when running a session.")]
    [SerializeField] private string _appSceneName;

    /// <summary>
    /// The scene to load when running a session.
    /// </summary>
    public static string AppSceneName
    {
        get { return Instance._appSceneName; }
    }
    #endregion

    #region Fields
    private static SettingsManager _instance;

    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.FindObjectsOfTypeAll<SettingsManager>()[0];
            }
            return _instance;
        }
    }

    /// <summary>
    /// Tracks if the game is being closed.
    /// </summary>
    public static bool appIsQuitting = false;

    #endregion

    #region Key Fields
    /// <summary>
    /// The key used in the photon player properties that indicates whether the player is ready in the lobby.
    /// </summary>
    public const string PLAYER_READY = "isReady";

    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(Instance);

        // this makes sure the events are still received event if the timescale is set to 0
        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 0f;

        // this makes sure the cilents will not sync their level automatically when the master client uses PhotonNetwork.LoadLevel()
        PhotonNetwork.AutomaticallySyncScene = _syncScene;
    }

    private void OnApplicationQuit()
    {
        appIsQuitting = true;

        if (Verbose)
        {
            Debug.LogFormat("[SettingsManager] On Application Quit - Is {0}connected", (PhotonNetwork.IsConnected ? string.Empty : "dis"));
        }

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    #endregion
    

}
