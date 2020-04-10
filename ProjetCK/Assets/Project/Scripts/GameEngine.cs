using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameEngine : MonoBehaviour
{
    #region Inspector
    [SerializeField] private string _playerPrefab;

    [SerializeField] private Transform _spawnPos;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(_playerPrefab, _spawnPos.position, _spawnPos.rotation);
    }
    
}
