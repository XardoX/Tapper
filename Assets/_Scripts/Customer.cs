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
    [HideInInspector] public bool _moving = true;
    private Animator _anim;
    private Beer _beerCurrent = null;
    private float _time;
    private float _nextXPosition;


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
        } else 
        {
            UpdateProperties();
        }
        _moving = true;
        _anim = GetComponent<Animator>();
        _time = Time.time;
        //_nextXPosition = this.transform.position.x;
    }
    void Update()
    {
        if(_moving)
        {
            if (_beersToDrink > 0)
            {
                if (transform.position.x <= _nextXPosition)
                {
                    transform.Translate(new Vector3(customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
                }else if(_time < customerSettings.waitDuration)
                {
                    _time += Time.deltaTime;
                } else 
                {
                    _time = 0f;
                    _nextXPosition += customerSettings.moveDistance;
                }
            }else
            {
                transform.Translate(new Vector3(-customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
            }
        }
    }
    private void OnEnable() 
    {
        _nextXPosition = this.transform.position.x + customerSettings.moveDistance;
    }
    private void OnDisable() 
    {
        GameManager.Instance.customers.Remove(this);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        //if(other.CompareTag("GameController")) this.gameObject.SetActive(false);
        if(other.CompareTag("Beer") && _beerCurrent == null)
        {
            Beer _beer = other.GetComponent<Beer>();
            if(_beer.isFull)
            {
                _beerCurrent = _beer;
                _moving = false;
                _beer.StopBeer();
                _anim.SetTrigger("Drinking");
                //_anim.ResetTrigger("Drinking");
                Debug.Log("trigger");
            }
        }
    }

    public void EndDrinking()
    {
        if(_beerCurrent != null)
        {
            Debug.Log(gameObject.name);
            _anim.ResetTrigger("Drinking");
            _beersToDrink -= 1;
            if(_beersToDrink == 0)
            {
                _moving = true;
            } else BackToEntrance();
            _beerCurrent.SetBeerStatus(false);
            _beerCurrent.ThrowBeer(Vector2.right);
            _beerCurrent = null;
        }
    }

    public void WatchLadies()
    {
        _moving = false;
        _anim.SetTrigger("Watch");
    }

    [Button]
    private void UpdateProperties()
    {
        _beersToDrink = customerSettings.thirst;
    }

    private void BackToEntrance()
    {
        while(transform.position.x <= -8f)
        {
            transform.Translate(new Vector3(-customerSettings.moveSpeed*Time.deltaTime, 0f, 0f));
        }
        _moving = true;
    }
}
