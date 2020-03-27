using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    #region Fields
    /// <summary>
    /// The player to track.
    /// </summary>
    [SerializeField] private Player _player;

    /// <summary>
    /// The offset from the player's position.
    /// </summary>
    private Vector3 _offset;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - _player.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.transform.position + _offset;
    }
}
