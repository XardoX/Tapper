using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Bar> _bars;
    [SerializeField][ReadOnly] private List<Customer> _customers;
    
    private void Start() 
    {

    }
    public void GameStart()
    {

    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        other.gameObject.SetActive(false);    
    }
    
}
