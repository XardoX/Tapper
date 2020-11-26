using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public int beersToDrink = 1;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _waitTime = 0.5f;
    [SerializeField] private float _moveDistance = 1f;

    [HideInInspector] public bool _moving = true;
    private Animator _anim;
    private Beer _beerCurrent = null;
    private float _time;
    private float _nextXPosition;


    void Start()
    {
        _moving = true;
        _anim = GetComponent<Animator>();
        _time = Time.time;
        //_nextXPosition = this.transform.position.x;
    }
    void Update()
    {
        if(_moving)
        {
            if (beersToDrink > 0)
            {
                if (transform.position.x <= _nextXPosition)
                {
                    transform.Translate(new Vector3(_speed*Time.deltaTime, 0f, 0f));
                }else if(_time < _waitTime)
                {
                    _time += Time.deltaTime;
                } else 
                {
                    _time = 0f;
                    _nextXPosition += _moveDistance;
                }
            }else
            {
                transform.Translate(new Vector3(-_speed*Time.deltaTime, 0f, 0f));
            }
        }
    }
    private void OnEnable() 
    {
        _nextXPosition = this.transform.position.x + _moveDistance;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        //if(other.CompareTag("GameController")) this.gameObject.SetActive(false);
        if(other.CompareTag("Beer") && _beerCurrent == null)
        {
            Beer _beer = other.GetComponent<Beer>();
            if(_beer.isFull)
            {
                _beerCurrent = _beer;
                _moving = false;
                _beer.StopBeer();
                _anim.SetTrigger("Drinking");
            }
        }
    }

    public void EndDrinking()
    {
        _moving = true;
        beersToDrink -= 1;
        _beerCurrent.SetBeerStatus(false);
        _beerCurrent.ThrowBeer(Vector2.right);
        _beerCurrent = null;
    }

    public void WatchLadies()
    {
        _moving = false;
        _anim.SetTrigger("Watch");
    }
}
