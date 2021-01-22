using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText = null;
    [SerializeField] private GameObject _endWindow = null;
    [SerializeField] private TextMeshProUGUI _endScoreText = null;
    [SerializeField] private GameObject _heartsParent = null;
    [SerializeField][ReadOnly] private List<GameObject> _hearts = null;


    public static UIController Instance;
    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        } else Destroy(this);

        foreach(Transform child in _heartsParent.transform)
        {
            _hearts.Add(child.gameObject);
        }
        _scoreText.text = "0000";
    }
    public void UpdateHearts(int currentLives)
    {
        foreach(GameObject hearth in _hearts)
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

    public void ShowEndScreen(int score) 
    {
        _endWindow.SetActive(true);
        _endScoreText.text = score.ToString();
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
