using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameEngine : MonoBehaviour
{
    #region Inspector
    [Header("Settings")]
    [SerializeField] private string _playerPrefab;

    [Header("Links")]
    [SerializeField] private Transform _spawnPos;

    #endregion

    #region Fields
    public static GameEngine Instance { get; private set; }

    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(_playerPrefab, _spawnPos.position, _spawnPos.rotation);
    }

    #endregion

    #region Methods

    #endregion
}
