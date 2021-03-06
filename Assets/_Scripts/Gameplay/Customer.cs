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
    [ReadOnly] [SerializeField]private bool _moving = true;
    private Animator _anim;
    private Collider2D _coll;
    private Beer _beerCurrent = null;
    private float _time;
    private float _nextXPosition;

    private bool _canCatch;
    private bool _distacted = false;


    private void Awake() 
    {
        
    }
    private void Start()
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
            UpdateProperties();
        }
        _moving = true;
        _anim = GetComponent<Animator>();
        _coll = GetComponent<Collider2D>();
        _time = Time.time;
    }
    private void Update()
    {
        if(_distacted)
        {
            return;
        }
        if(_moving)
        {
            if (_beersToDrink > 0)
            {
                if (transform.position.x < _nextXPosition)//moving towards player
                {
                    Vector3 offset = new Vector3(_coll.bounds.size.x/ 2, 1f,0);
                    RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, Vector2.right, 0.3f);
                    Debug.DrawRay(transform.position + offset, new Vector3(0.3f, 0f, 0f),Color.red);
                    if(hit.collider != null)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        GameObject customer = hit.collider.gameObject;
                        if(!customer.CompareTag(Tags.customer))
                        {
                            Debug.Log(hit.collider.gameObject.name);
                            transform.Translate(new Vector3(customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
                        }
                    } 
                    else
                    {
                        transform.Translate(new Vector3(customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
                    }
                }
                else if(_time < customerSettings.waitDuration)//waits
                {
                    _time += Time.deltaTime; 
                } else //setting next position
                {
                    _time = 0f;
                    _nextXPosition += customerSettings.moveDistance; 
                }
            }else //leaves bar
            {
                transform.Translate(new Vector3(-customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
                _canCatch = false;
                gameObject.tag = Tags.drunkCustomer;
            }
        } else if (_beersToDrink > 1) // goes back a little bit and drinks
        {
            gameObject.tag = Tags.drunkCustomer;
            float _backingPos = _nextXPosition - customerSettings.moveDistance * 3f;
            if(_backingPos < currentBar.transform.position.x +5.0f) _backingPos = currentBar.transform.position.x +5.0f;
            if(transform.position.x > _backingPos)
            {
                transform.Translate(new Vector3(-customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
            } else 
            {
                _anim.SetTrigger("Drinking");
                _beerCurrent.HideSprite(false);
            }
        }
    }

    private void FixedUpdate() 
    {
    }
    private void OnEnable() 
    {
        _nextXPosition = this.transform.position.x + customerSettings.moveDistance;
        gameObject.tag = Tags.customer;
        _canCatch = true;
        UpdateProperties();
    }
    private void OnDisable() 
    {
        GameManager.Instance.customers.Remove(this);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Tags.beer) && _beerCurrent == null && _canCatch)
        {
            Beer _beer = other.GetComponent<Beer>();
            if (_beer.isFull)
            {
                _beerCurrent = _beer;
                _moving = false;
                _beerCurrent.StopBeer();
                _beerCurrent.transform.parent = this.transform;
                gameObject.tag = Tags.drunkCustomer;
                if (_beersToDrink == 1)
                {
                    _anim.SetTrigger("Drinking");
                    _beerCurrent.HideSprite(false);
                }
                else if (_beersToDrink > 1)
                {

                }
            }
        }
    }

    public void EndDrinking()
    {
        if (_beerCurrent == null)
        {
            return;
        }
        _anim.ResetTrigger("Drinking");
        _beerCurrent.HideSprite(true);
        _beersToDrink -= 1;
        if (_beersToDrink > 0) gameObject.tag = "Customer";
        if(_beersToDrink <= 0)
        {
            if(Random.Range(0,100) <10)
            {
                Vector3 spawnPos = new Vector3(transform.position.x, currentBar.beerSpawnPoint.position.y + 0.03f, transform.position.z);
                GameManager.Instance.SpawnTip(spawnPos);
            }
        }
        _moving = true;
        _beerCurrent.transform.parent = ObjectPooler.Instance.objectsToPool[0].objectsParent.transform;
        _beerCurrent.SetBeerStatus(false);
        _beerCurrent.ThrowBeer(Vector2.right);
        _beerCurrent = null;
        _time = 0f;
        _nextXPosition = transform.position.x;
    }

    public void WatchLadies()
    {
        if(_beersToDrink <= 0 || _beerCurrent != null || !_canCatch)
        {
            return;
        }
        _moving = false;
        _distacted = true;
        _anim.SetTrigger("Watch");
    }
    public void EndWatching()
    {
        _moving = true;
        _distacted = false;
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
