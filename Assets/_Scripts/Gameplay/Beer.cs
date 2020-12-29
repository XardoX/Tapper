using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Beer : MonoBehaviour
{
    [ReadOnly]public bool isFull = true;
    [ReadOnly] public bool catched;
    [SerializeField] private Sprite _beerFull = null, _beerEmpty = null;
    
    [SerializeField] private float _beerSpeed = 1;
    [SerializeField] private float _emptyBeerSpeed = 1;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }
    
    private void OnEnable() 
    {
        catched = false;
    }
    private void OnDisable() 
    {
        isFull = true;
        _renderer.sprite = _beerFull;
    }

    public void ThrowBeer(Vector2 direction)
    {
        if(isFull)
        {
            _rb.velocity = direction * _beerSpeed;
        } else _rb.velocity = direction * _emptyBeerSpeed;
    }

    public void SetBeerStatus(bool _isFull)
    {
        isFull = _isFull;
        if(isFull)
        {
            this.gameObject.layer = Layers.beer;
            this.gameObject.tag = Tags.beer;
            _renderer.sprite = _beerFull;
        }else 
        {
            _renderer.sprite = _beerEmpty;
        }
    }

    public void StopBeer()
    {
        _rb.velocity = Vector3.zero;
        this.gameObject.layer = Layers.emptyBeer;
        this.gameObject.tag = Tags.emptyBeer;
    }
    public void BreakMug(bool isFool)
    {
        StopBeer();
    }
}
