using UnityEngine;
using NaughtyAttributes;

public class Customer : MonoBehaviour
{
    public int customerIndex = 0;
    public bool overrideSettings;
    [EnableIf("overrideSettings")]
    public CustomerSettings customerSettings;
    [Space]
    [SerializeField][ReadOnly][Label("Beers left to drink")]
    private int _beersToDrink = 1;
    [ReadOnly]
    public Bar currentBar;
    [ReadOnly] [SerializeField]private bool _moving = true;
    private Animator _anim;
    private Beer _beerCurrent = null;
    private float _time;
    private float _nextXPosition;

    private bool _canCatch;
    private bool _distacted = false;


    private void Awake() 
    {
        
    }
    void Start()
    {
        if(customerIndex > GameManager.Instance.settings.customerTypes.Length)
        {
            customerIndex = 0;
            Debug.Log("CustomerIndexOutOfRAnge");
        }
        if(overrideSettings == false)
        {
            customerSettings = GameManager.Instance.settings.customerTypes[customerIndex];
            _beersToDrink = customerSettings.thirst;
        } else 
        {
            UpdateProperties();
        }
        _moving = true;
        _anim = GetComponent<Animator>();
        _time = Time.time;
    }
    void Update()
    {
        if(_distacted)
        {
            return;
        }
        if(_moving)
        {
            if (_beersToDrink > 0)
            {
                if (transform.position.x <= _nextXPosition)//moving towards player
                {
                    transform.Translate(new Vector3(customerSettings.moveSpeed*Time.deltaTime, 0f, 0f)); 
                }else if(_time < customerSettings.waitDuration)//waits
                {
                    _time += Time.deltaTime; 
                } else //setting next position
                {
                    _time = 0f;
                    _nextXPosition += customerSettings.moveDistance; 
                }
            }else //leaves bar
            {
                transform.Translate(new Vector3(-customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
                _canCatch = false;
                gameObject.tag = "Drunk Customer";
            }
        } else if (_beersToDrink > 1) // goes back a little bit and drinks
        {
            gameObject.tag = "Drunk Customer";
            float _backingPos = _nextXPosition - customerSettings.moveDistance * 3f;
            if(_backingPos < 7.5f) _backingPos = 7.5f;
            if(transform.position.x > _backingPos)
            {
                transform.Translate(new Vector3(-customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
            } else 
            {
                _anim.SetTrigger("Drinking");
            }
        }
    }
    private void OnEnable() 
    {
        _nextXPosition = this.transform.position.x + customerSettings.moveDistance;
        gameObject.tag = "Customer";
        _canCatch = true;
        UpdateProperties();
    }
    private void OnDisable() 
    {
        GameManager.Instance.customers.Remove(this);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Beer") && _beerCurrent == null && _canCatch)
        {
            Beer _beer = other.GetComponent<Beer>();
            if (_beer.isFull)
            {
                _beerCurrent = _beer;
                _moving = false;
                _beerCurrent.StopBeer();
                _beerCurrent.transform.parent = this.transform;
                gameObject.tag = "Drunk Customer";
                if (_beersToDrink == 1)
                {
                    _anim.SetTrigger("Drinking");
                }
                else if (_beersToDrink > 1)
                {

                }
            }
        }
    }

    public void EndDrinking()
    {
        if (_beerCurrent == null)
        {
            return;
        }
        _anim.ResetTrigger("Drinking");
        _beersToDrink -= 1;
        if (_beersToDrink > 0) gameObject.tag = "Customer";
        _moving = true;
        _beerCurrent.transform.parent = ObjectPooler.Instance.objectsToPool[0].objectsParent.transform;
        _beerCurrent.SetBeerStatus(false);
        _beerCurrent.ThrowBeer(Vector2.right);
        _beerCurrent = null;
        _time = 0f;
        _nextXPosition = transform.position.x;
    }

    public void WatchLadies()
    {
        if(_beersToDrink <= 0 || _beerCurrent != null || !_canCatch)
        {
            return;
        }
        _moving = false;
        _distacted = true;
        _anim.SetTrigger("Watch");
    }
    public void EndWatching()
    {
        _moving = true;
        _distacted = false;
    }

    [Button]
    public void UpdateProperties()
    {
        _beersToDrink = customerSettings.thirst;
        //Debug.Log(gameObject.name +" Properties updated " +customerSettings.thirst);
    }

    private void BackToEntrance()
    {
        while(transform.position.x >= currentBar.customerSpawnPoint.transform.position.x)
        {
            transform.Translate(new Vector3(-customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
        }
        _moving = true;
    }

    public void ThrowPlayer()
    {
        Debug.Log("Player");
    }
}
