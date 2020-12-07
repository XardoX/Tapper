using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
public class GameManager : MonoBehaviour
{
    public Settings settings;
    public GameObject barsParent;
    [ReadOnly] public List<Bar> bars;
    [ReadOnly] public List<Customer> customers;
    
    public static GameManager Instance;

    private bool _isLevelCleared;
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
        GameStart();
    }
    public void GameStart()
    {
        StartCoroutine(CustomersWave(0,0));
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
         if(other.CompareTag("Customer"))
        {
            customers.Remove(other.GetComponent<Customer>());
            other.gameObject.SetActive(false);
        }
    }

    public IEnumerator CustomersWave(int levelID, int waveID)
    {
        yield return new WaitForSeconds(0.5f);
        Levels.Wave currentWave = settings.levels[levelID].wave[waveID];
        while(!_isLevelCleared)
        {
            int _currentLimit = currentWave.spawnLimit;
            float timeLimit = currentWave.spawnDuration + Time.time;
            while(_currentLimit > 0 && Time.time <= timeLimit)
            {
                int currentSpawnChance = currentWave.minBarChance;
                Bar _chosenBar = null;
                bars = bars.OrderBy(i => Random.value).ToList();
                foreach(Bar _bar in bars)
                {
                    if(currentSpawnChance >= Random.Range(0,100))
                    {
                        _chosenBar = _bar;
                        break;
                    } else currentSpawnChance += currentWave.chanceIncrease;
                }
            
                int customersChances = 0;
                CustomerSettings _customerType = null;
                foreach (Levels.Wave.CustomerTypes type in currentWave.customersTypes)
                {
                    customersChances += type.chanceForSpawn;
                }

                int _results = Random.Range(0, customersChances);
                int _localObjectID = 0;
                for (int i = 0; i < currentWave.customersTypes.Length; i++)
                {
                    _results -= currentWave.customersTypes[i].chanceForSpawn;
                    if(_results <=0) 
                    {
                        _customerType = currentWave.customersTypes[i].customerSettings;
                        _localObjectID = i;
                        break;
                    }
                }
                if(_chosenBar != null && _customerType != null)
                {
                    Customer _spawnedCustomer = ObjectPooler.Instance.GetPooledObject(1,_localObjectID).GetComponent<Customer>();
                    if(currentWave.customersTypes[_localObjectID].overridePrefab)
                    {
                        _spawnedCustomer.customerSettings = _customerType;
                    }
                    _currentLimit--;
                    customers.Add(_spawnedCustomer);
                    _spawnedCustomer.transform.position = _chosenBar.customerSpawnPoint.position;
                    _spawnedCustomer.gameObject.SetActive(true);
                } else Debug.Log(_chosenBar +" "+ _customerType);
                yield return new WaitForSeconds(1f);
            } 
            yield return new WaitForSeconds(currentWave.breakDuration);
            if(customers.Count == 0) _isLevelCleared = true;
            break;
        }
        yield return null;
    }
    
}
