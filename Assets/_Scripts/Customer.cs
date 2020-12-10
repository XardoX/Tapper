﻿using UnityEngine;
using NaughtyAttributes;

public class Customer : MonoBehaviour
{
    public int customerIndex = 0;
    public bool overrideSettings;
    [EnableIf("overrideSettings")]
    public CustomerSettings customerSettings;
    [Space]
    [SerializeField][ReadOnly][Label("Beers left to drink")]
    private int _beersToDrink = 1;
    [ReadOnly]
    public Bar currentBar;
    [HideInInspector] public bool _moving = true;
    private Animator _anim;
    private Beer _beerCurrent = null;
    private Transform _beerParent = null;
    private float _time;
    private float _nextXPosition;

    private bool _canCatch;


    private void Awake() 
    {
        
    }
    void Start()
    {
        if(customerIndex > GameManager.Instance.settings.customerTypes.Length)
        {
            customerIndex = 0;
            Debug.Log("CustomerIndexOutOfRAnge");
        }
        if(overrideSettings == false)
        {
            customerSettings = GameManager.Instance.settings.customerTypes[customerIndex];
            _beersToDrink = customerSettings.thirst;
        } else 
        {
            Debug.Log("Update na starcie");
            UpdateProperties();
        }
        _moving = true;
        _anim = GetComponent<Animator>();
        _time = Time.time;
        //_nextXPosition = this.transform.position.x;
    }
    void Update()
    {
        if(_moving)
        {
            if (_beersToDrink > 0)
            {
                if (transform.position.x <= _nextXPosition)
                {
                    transform.Translate(new Vector3(customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
                }else if(_time < customerSettings.waitDuration)
                {
                    _time += Time.deltaTime; //waits
                } else 
                {
                    _time = 0f;
                    _nextXPosition += customerSettings.moveDistance;
                }
            }else
            {
                transform.Translate(new Vector3(-customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
                _canCatch = false;
                gameObject.tag = "Drunk Customer";
            }
        } else if (_beersToDrink > 1)
        {
            gameObject.tag = "Drunk Customer";
            if(transform.position.x > _nextXPosition - customerSettings.moveDistance * 3f)
            {
                transform.Translate(new Vector3(-customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
            } else 
            {
                _anim.SetTrigger("Drinking");
            }
        }
    }
    private void OnEnable() 
    {
        _nextXPosition = this.transform.position.x + customerSettings.moveDistance;
        gameObject.tag = "Customer";
        _canCatch = true;
        UpdateProperties();
    }
    private void OnDisable() 
    {
        GameManager.Instance.customers.Remove(this);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        //if(other.CompareTag("GameController")) this.gameObject.SetActive(false);
        if(other.CompareTag("Beer") && _beerCurrent == null && _canCatch)
        {
            Beer _beer = other.GetComponent<Beer>();
            if(_beer.isFull)
            {
                _beerCurrent = _beer;
                _moving = false;
                _beerCurrent.StopBeer();
                _beerParent = ObjectPooler.Instance.objectsToPool[0].objectsParent.transform;
                _beerCurrent.transform.parent = this.transform;
                if(_beersToDrink == 1)
                {
                    _anim.SetTrigger("Drinking");
                } 
                else if (_beersToDrink > 1)
                {
                    
                }
            }
        }
    }

    public void EndDrinking()
    {
        if(_beerCurrent != null)
        {
            _anim.ResetTrigger("Drinking");
            _beersToDrink -= 1;
            gameObject.tag = "Customer";
            _moving = true;
            _beerCurrent.transform.parent = _beerParent;
            _beerCurrent.SetBeerStatus(false);
            _beerCurrent.ThrowBeer(Vector2.right);
            _beerCurrent = null;
            _time = 0f;
            _nextXPosition = transform.position.x;
        }
    }

    public void WatchLadies()
    {
        _moving = false;
        _anim.SetTrigger("Watch");
    }

    [Button]
    public void UpdateProperties()
    {
        _beersToDrink = customerSettings.thirst;
        //Debug.Log(gameObject.name +" Properties updated " +customerSettings.thirst);
    }

    private void BackToEntrance()
    {
        while(transform.position.x >= currentBar.customerSpawnPoint.transform.position.x)
        {
            transform.Translate(new Vector3(-customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
        }
        _moving = true;
    }

    public void ThrowPlayer()
    {
        Debug.Log("Player");
    }
}
