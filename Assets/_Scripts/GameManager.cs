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

    [ReadOnly] public int playerLives = 3;
    [SerializeField] [ReadOnly] private int _score;
    [SerializeField] [ReadOnly] private int _currentLevel, _currentWave;
    [ReadOnly] public bool isGameActive;
    private List<Bar> _randomBars;
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
        _randomBars = bars;
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
        if(isGameActive)
        {
            if(other.CompareTag(Tags.customer) || other.CompareTag(Tags.drunkCustomer))
            {
                customers.Remove(other.GetComponent<Customer>());
                other.gameObject.SetActive(false);
            } 
            else if(other.CompareTag(Tags.beer)|| other.CompareTag(Tags.emptyBeer))
            {
                other.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator CustomersWave(int levelID, int waveID)
    {
        _currentLevel = levelID;
        _currentWave = waveID;
        isGameActive = true;
        yield return new WaitForSeconds(0.5f);
        Levels.Wave currentWave = settings.levels[levelID].wave[waveID];
        int _number = 0;
        while(!_isLevelCleared)
        {
            if(currentWave.loopWave) _number = 0;
            while(_number < currentWave.numberOfCustomers)
            {
                int _currentLimit = currentWave.spawnLimit;
                float timeLimit = currentWave.spawnDuration + Time.time;
                while(_currentLimit > 0 && Time.time <= timeLimit)
                {
                    #region randomly chosing bar
                    int currentSpawnChance = currentWave.minBarChance;
                    Bar _chosenBar = null;
                    _randomBars = _randomBars.OrderBy(i => Random.value).ToList();
                    foreach(Bar _bar in _randomBars)
                    {
                        if(currentSpawnChance >= Random.Range(0,100))
                        {
                            _chosenBar = _bar;
                            break;
                        } else currentSpawnChance += currentWave.chanceIncrease;
                    }
                    #endregion

                    #region Calculating customer type
                    int customersChances = 0;
                    CustomerSettings _customerType = null;
                    foreach (Levels.Wave.CustomerTypes type in currentWave.customersTypes)
                    {
                        customersChances += type.chanceForSpawn;
                    }
                    #endregion

                    #region Calculating spawn chance
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
                    #endregion

                    #region Spawning Customer
                    if(_chosenBar != null && _customerType != null)
                    {
                        Customer _spawnedCustomer = ObjectPooler.Instance.GetPooledObject(1,_localObjectID).GetComponent<Customer>();
                        if(currentWave.customersTypes[_localObjectID].overridePrefab)
                        {
                            _spawnedCustomer.customerSettings = _customerType;
                        }
                        _currentLimit--;
                        _number++;
                        customers.Add(_spawnedCustomer);
                        _spawnedCustomer.currentBar = _chosenBar;
                        _spawnedCustomer.UpdateProperties();
                        _spawnedCustomer.transform.position = _chosenBar.customerSpawnPoint.position;
                        _spawnedCustomer.gameObject.SetActive(true);
                    } else 
                    {
                        if(_chosenBar == null) Debug.Log("_chosenbar null");
                        if(_customerType == null) Debug.Log("_customerType null");
                    }
                    yield return new WaitForSeconds(1f);
                    #endregion
                } 
                yield return new WaitForSeconds(currentWave.breakDuration);
                if(customers.Count == 0) 
                {
                    _isLevelCleared = true;
                    break;
                }
            }
        }
        yield return null;
    }
    
    void ResetLevel()
    {
        StopAllCoroutines();
        isGameActive = false;
        ObjectPooler.Instance.ResetObjects(0);
        ObjectPooler.Instance.ResetObjects(1);
        customers.Clear();
        if(playerLives <=0)
        {
            //PlayerLost();
        } else
        {
            //UpdateUI();
            //ResetPlayer();
            StartCoroutine(CustomersWave(_currentLevel,_currentWave));
        }
    }

    public void TakePlayerHearth()
    {
        playerLives--;
        UIController.Instance.UpdateHearts(playerLives);
        ResetLevel();
    }

    public void AddPoints(int value)
    {
        _score += value;
        UIController.Instance.UpdateScore(_score);
    }

}
