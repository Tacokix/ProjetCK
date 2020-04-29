using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Player.localInstance.TryUseSpell(1);
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            Player.localInstance.TryUseSpell(2);
        }
    }
}
