using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    #region Inspector

    [Tooltip("The offset from the player's position.")]
    [SerializeField] private Vector3 _offset;
    #endregion

    #region Fields
    /// <summary>
    /// The player to track.
    /// </summary>
    private Player _player;

    #endregion

    #region MonoBehaviour
    void OnEnable()
    {
        _player = Player.localInstance;

        if (_player == null)
        {
            Player.OnLocalPlayerInstantiated += InitializeCamera;
        }
    }

    void OnDisable()
    {
        Player.OnLocalPlayerInstantiated -= InitializeCamera;
    }

    void Update()
    {
        if(_player = null)
        {
            transform.position = _player.transform.position + _offset;
        }
    }

    #endregion

    #region Methods
    /// <summary>
    /// Indicates the player to track.
    /// </summary>
    /// <param name="player">The player to track.</param>
    public void InitializeCamera(Player player)
    {
        _player = player;
    }

    #endregion
}
