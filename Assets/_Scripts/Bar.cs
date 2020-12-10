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
            if(other.CompareTag("Customer"))
            {
                GameManager.Instance.TakePlayerHearth();
                other.GetComponent<Customer>().ThrowPlayer();
            }else if(other.CompareTag("Beer"))
            {
                GameManager.Instance.TakePlayerHearth();
                other.GetComponent<Beer>().BreakMug(true);
            }else if(other.CompareTag("Empty Beer"))
            {
                Beer beer = other.GetComponent<Beer>();
                if(beer.catched == false)
                {
                    GameManager.Instance.TakePlayerHearth();
                    other.GetComponent<Beer>().BreakMug(false);
                }
            }
        }
    }
}
