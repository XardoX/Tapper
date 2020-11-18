using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour
{
    [SerializeField] private float _beerSpeed;
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        rb.velocity = direction * _beerSpeed;
    }
}
