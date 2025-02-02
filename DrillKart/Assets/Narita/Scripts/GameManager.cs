﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("制限時間(Second)")] float _time;
    [SerializeField, Header("残り時間がα分になると、画面を光らせる")] int _flashMinute;
    [SerializeField, Header("残り時間がα秒になると、画面を光らせる")] int _flashSecond;
    [SerializeField] Image _image;
    bool _isMinuteFlashed;
    bool _isSecondFlashed;
    private float _timer;
    public float Timer { get => _timer; }

    void Start()
    {
        _timer = _time;
    }

    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= _flashMinute * 60 && !_isMinuteFlashed)
            {
                _isMinuteFlashed = true;
                Sequence sequence = DOTween.Sequence();
                for (int i = 1; i <= 2; i++)//ループ回数分画面を光らせる
                {
                    sequence.Append(_image.DOFade(0.3f, 0.3f));
                    sequence.Append(_image.DOFade(0, 0.7f));
                }
                sequence.Play();
            }
            else if (_timer <= _flashSecond && !_isSecondFlashed)
            {
                _isSecondFlashed = true;
                Sequence sequence = DOTween.Sequence();
                for (int i = 1; i < _flashSecond; i++)//ループ回数分画面を光らせる
                {
                    sequence.Append(_image.DOFade(0.3f, 0.3f));
                    sequence.Append(_image.DOFade(0, 0.7f));
                }
                sequence.Play();
            }
        }
    }
}
