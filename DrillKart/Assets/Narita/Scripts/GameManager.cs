using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("制限時間(Second)")] float _time;
    [SerializeField,Header("残り時間がα分になると、画面を光らせる")] float _lightingTime;
    bool _isLighted;
    private float _timer;
    public float Timer { get => _timer; }

    void Start()
    {
        _timer = _time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= _lightingTime && !_isLighted)
            {
                _isLighted = true;
            }
        }
    }
}
