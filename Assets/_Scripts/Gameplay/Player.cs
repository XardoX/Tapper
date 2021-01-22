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
    [SerializeField][ReadOnly] private int currentBar = 0;
    private Transform[] _bars;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private Beer _heldBeer;
    private GameManager gm;

    private float _moveTime;
    private bool _canRun = true;

    private void Awake() 
    {
        _rb = this.GetComponent<Rigidbody2D>();
        _renderer = this.GetComponent<SpriteRenderer>();
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
        gm = GameManager.Instance;
        _bars = new Transform[GameManager.Instance.bars.Count];
        for(int i = 0; i < _bars.Length; i++)
        {
            _bars.SetValue(gm.bars[i].playerPoint, i);
        }
        SetBounds();
        
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void Update() 
    {
        if(_canRun)
        {
            Run();
        }
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
            _canRun = false;
            transform.position = _bars[currentBar].position;
            _heldBeer = ObjectPooler.Instance.GetPooledObject(0).GetComponent<Beer>();
            _heldBeer.transform.position = _beerSpawn.position;
            _heldBeer.gameObject.SetActive(true);
            _heldBeer.SetBeerStatus(true);
            _heldBeer.catched = true;
            _renderer.flipX = true;
        }

        if(context.performed && _heldBeer != null)
        {
            _canRun = true;
            _heldBeer.transform.position = gm.bars[currentBar].beerSpawnPoint.position;
            _heldBeer.ThrowBeer(Vector2.left);
            _heldBeer.catched = false;
            _renderer.flipX = false;
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
            currentBar = _bars.Length -1;
        } else if(currentBar >= _bars.Length)
        {
            currentBar = 0;
        }
        if(_heldBeer != null)
        {
            _heldBeer.gameObject.SetActive(false);
            _heldBeer = null;
        }
        SetBounds();
        transform.position = _bars[currentBar].position;
        AudioManager.Instance.Play("BarChange");
        _canRun = true;
    }

    private void SetBounds()
    {
        _bounds = new Vector2 (GameManager.Instance.bars[currentBar].entrancePoint.position.x, _bars[currentBar].position.x);
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
            GameManager.Instance.DistractCustomers();
            Destroy(other.gameObject);
        }
    }
}