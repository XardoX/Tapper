using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    public Transform customerSpawnPoint;
    public Transform playerPoint;

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(GameManager.Instance.isGameActive)
        {
            if(other.CompareTag(Tags.customer))
            {
                GameManager.Instance.TakePlayerHearth();
                other.GetComponent<Customer>().ThrowPlayer();
            }else if(other.CompareTag(Tags.beer))
            {
                Beer beer = other.GetComponent<Beer>();
                if(beer.catched == false)
                {
                    GameManager.Instance.TakePlayerHearth();
                    beer.BreakMug(false);
                }
            }else if(other.CompareTag(Tags.emptyBeer))
            {
                Beer beer = other.GetComponent<Beer>();
                if(beer.catched == false)
                {
                    GameManager.Instance.TakePlayerHearth();
                    beer.BreakMug(false);
                }
            }
        }
    }
}
