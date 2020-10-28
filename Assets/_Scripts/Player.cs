using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    public static Controls controls;
    private void OnEnable()
    {
       if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        Debug.Log("D");
    }
}