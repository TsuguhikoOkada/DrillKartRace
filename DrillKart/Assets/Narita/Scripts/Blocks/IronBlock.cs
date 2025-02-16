using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBlock : BlockManager_B
{
    int _soldCount;
    float _maxRateTrend = 70;
    [SerializeField, Header("çáåvîÑãpêî")]
    int _initialIronRateStep = 1000;
    int _ironRateStep;
    GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _ironRateStep = _initialIronRateStep;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.IronCount >= _ironRateStep)
        {
            _data.RateTrend = Mathf.Min(_maxRateTrend, _data.RateTrend + 5);
            _ironRateStep += _initialIronRateStep;
        }
    }
}
