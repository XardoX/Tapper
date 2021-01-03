using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    public static Controls controls;
    [SerializeField] private Transform _beerSpawn = null;
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _moveCoolDown = 0.1f;
    [SerializeField] private Vector2 _bounds = Vector2.zero;
    //[SerializeField] private GameObject _bars = null;
    [SerializeField][ReadOnly] private int currentBar = 0;
    private Transform[] _barrels;
    private Rigidbody2D _rb;
    private Beer _heldBeer;

    private float _moveTime;

    private void Awake() 
    {
        _rb = this.GetComponent<Rigidbody2D>();
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
    private void Start() 
    {
        _barrels = new Transform[GameManager.Instance.bars.Count];
        for(int i = 0; i < _barrels.Length; i++)
        {
            _barrels.SetValue(GameManager.Instance.bars[i].playerPoint, i);
        }
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
        if(context.performed && Time.time > _moveTime + _moveCoolDown)
        {
            _moveTime = Time.time;
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
           _heldBeer = ObjectPooler.Instance.GetPooledObject(0).GetComponent<Beer>();
           _heldBeer.transform.position = _beerSpawn.position;
           _heldBeer.gameObject.SetActive(true);
           _heldBeer.SetBeerStatus(true);
           _heldBeer.catched = true;
        }

        if(context.performed && _heldBeer != null)
        {
            _heldBeer.ThrowBeer(Vector2.left);
            _heldBeer = null;
        }
    }

    private void Run()
    {
        Vector3 position = transform.position;
        float translation = Mathf.Round(controls.Player.Move.ReadValue<Vector2>().x) * _speed * Time.deltaTime;
        if(transform.position.x + translation < _bounds.x )
        { 
            position.x = _bounds.x;
        }
        else if(transform.position.x + translation > _bounds.y )
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
        if(_heldBeer != null)
        {
            _heldBeer.gameObject.SetActive(false);
            _heldBeer = null;
        }
        _bounds.y = _barrels[currentBar].position.x;
        transform.position = new Vector3(_bounds.y, _barrels[currentBar].position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag(Tags.emptyBeer))
        {
            Beer _beer = other.GetComponent<Beer>();
            if(!_beer.isFull)
            {
                _beer.catched = true;
                GameManager.Instance.AddPoints(100);
                other.gameObject.SetActive(false);
            }
        }
        else if(other.CompareTag(Tags.tip))
        {

        }
    }
}