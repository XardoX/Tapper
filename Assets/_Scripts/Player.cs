using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    public static Controls controls;
    [SerializeField] private float _speed = 10;
    [SerializeField] private Vector2 _bounds;

    [SerializeField] private GameObject[] _barrels = new GameObject[4];
    [SerializeField][ReadOnly] private int currentBar = 0;

    private void Awake() 
    {
    }
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
    private void Update() 
    {
        Run();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Vector2 moveInput = (context.ReadValue<Vector2>());
            
            if(moveInput == Vector2.up)
            {
                ChangeBar(1);
            } else if(moveInput == Vector2.down)
            {
                ChangeBar(-1);
            } else if (moveInput == Vector2.left)
            {
            }else if (moveInput == Vector2.right)
            {
            }
        }
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log("D");
        }

        if(context.performed)
        {

        }
    }

    private void Run()
    {
        Vector3 position = transform.position ;
        float translation = controls.Player.Move.ReadValue<Vector2>().x * _speed * Time.deltaTime;
        if(transform.position.x + translation < -5 )
        { 
            position.x = _bounds.x;
        }
        else if( transform.position.x + translation > 5 )
        {
            position.x = _bounds.y ;
        }
        else
        {
            position.x += translation;
        }

        transform.position = position;
        
    }
    private void ChangeBar(int moveInput)
    {
        currentBar += moveInput;
        if(currentBar < 0)
        {
            currentBar = _barrels.Length -1;
        } else if(currentBar >= _barrels.Length)
        {
            currentBar = 0;
        }
        transform.position = new Vector3(_bounds.y, _barrels[currentBar].transform.position.y, transform.position.z);
    }
}