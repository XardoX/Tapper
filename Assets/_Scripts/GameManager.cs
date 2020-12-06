using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class GameManager : MonoBehaviour
{
    public Settings settings;
    public GameObject barsParent;
    [ReadOnly] public List<Bar> bars;
    [SerializeField][ReadOnly] private List<Customer> _customers;
    
    public static GameManager Instance;
    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        } else Destroy(this);

        foreach(Transform child in barsParent.transform)
        {
            if(child.TryGetComponent(out Bar bar)) bars.Add(bar);
        }
    }

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

    public IEnumerator CustomersWave()
    {
        yield return null;
    }
    
}
