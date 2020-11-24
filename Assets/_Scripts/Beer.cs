using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Beer : MonoBehaviour
{
    [ReadOnly]public bool isFull = true;
    [SerializeField] private Sprite _beerFull, _beerEmpty;
    
    [SerializeField] private float _beerSpeed = 1;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    
    private void OnEnable() 
    {
        
    }
    private void OnDisable() 
    {
        
    }

    public void ThrowBeer(Vector2 direction)
    {
        _rb.velocity = direction * _beerSpeed;
    }

    public void SetBeerStatus(bool _isFull)
    {
        isFull = _isFull;
        if(isFull)
        {
            _renderer.sprite = _beerFull;
        }else 
        {
            _renderer.sprite = _beerEmpty;
        }
    }
}
