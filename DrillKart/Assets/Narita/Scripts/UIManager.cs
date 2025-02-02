using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] TextMeshProUGUI _scoreText;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _timerText.text = $"{((int)(_gameManager.Timer / 60)).ToString("00")}<size=50>M</size> " +
            $"{((int)_gameManager.Timer % 60).ToString("00")}<size=50>S</size>";
        var scoreList = _gameManager.CurrentScore();
        var scoreDic = scoreList.ToDictionary(x => x.Key, x => x.Value);
        var playerID = 1;
        _scoreText.text = $"<size=100>{scoreList.FindIndex(x => x.Key == playerID) + 1}</size> {scoreDic[playerID]}";
    }
}
