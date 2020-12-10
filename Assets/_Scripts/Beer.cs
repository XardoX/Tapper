﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Beer : MonoBehaviour
{
    [ReadOnly]public bool isFull = true;
    [ReadOnly] public bool catched;
    [SerializeField] private Sprite _beerFull = null, _beerEmpty = null;
    
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
        catched = false;
    }
    private void OnDisable() 
    {
        isFull = true;
        _renderer.sprite = _beerFull;
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
            this.gameObject.layer = LayerMask.NameToLayer("Beer");
            this.gameObject.tag = "Beer";
            _renderer.sprite = _beerFull;
        }else 
        {
            _renderer.sprite = _beerEmpty;
        }
    }

    public void StopBeer()
    {
        _rb.velocity = Vector3.zero;
        this.gameObject.layer = LayerMask.NameToLayer("Empty Beer");
        this.gameObject.tag = "Empty Beer";
    }
    public void BreakMug(bool isFool)
    {
        Debug.Log("Mug");
        StopBeer();
    }
}
