using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] TextMeshProUGUI _timerText;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _timerText.text = $"{((int)(_gameManager.Timer / 60)).ToString("00")}<size=50>M</size> " +
            $"{((int)_gameManager.Timer % 60).ToString("00")}<size=50>S</size>";
    }
}
