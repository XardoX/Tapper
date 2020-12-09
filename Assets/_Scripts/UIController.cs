using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText = null;
    [SerializeField] private GameObject _heartsParent = null;
    [SerializeField][ReadOnly] private List<GameObject> _Hearts = null;


    public static UIController Instance;
    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        } else Destroy(this);

        foreach(Transform child in _heartsParent.transform)
        {
            _Hearts.Add(child.gameObject);
        }
        _scoreText.text = "0000";
    }
    public void UpdateHearts(int currentLives)
    {
        foreach(GameObject hearth in _Hearts)
        {
            if(currentLives > 0)
            {
                hearth.SetActive(true);
            }else hearth.SetActive(false);
            currentLives--;
        }
    }
    public void UpdateScore(int newScore)
    {
        if(newScore < 10)
        {
            _scoreText.text = "000" + newScore.ToString();
        } else if(newScore < 100)
        {
            _scoreText.text = "00" + newScore.ToString();
        } else if(newScore < 1000)
        {
            _scoreText.text = "0" + newScore.ToString();
        }else _scoreText.text = newScore.ToString();
    }
}
