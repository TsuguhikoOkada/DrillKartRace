using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager_B : MonoBehaviour
{
    protected float InitialPrice { get => _initialPrice; }
    [SerializeField, Header("初期価格")]
    float _initialPrice = 100;
    protected float MinPrice { get => _minPrice; }
    [SerializeField, Header("最低価格")]
    float _minPrice = 50;
    protected float RateTrend { get => _rateTrend; }
    [SerializeField, Header("レート傾向(上昇)")]
    float _rateTrend = 50;
    protected float TrendShift { get => _trendShift; }
    [SerializeField, Header("傾向変化量")]
    float _trendShift = 10;
    protected float ChangeTiming { get => _changeTiming; }
    [SerializeField, Header("変化タイミング")]
    float _changeTiming = 120;
    protected float MaxUpperValue { get => _maxUpperValue; }
    [SerializeField, Header("上昇時最大値")]
    float _maxUpperValue = 20;
    protected float MinUpperValue { get => _minUpperValue; }
    [SerializeField, Header("上昇時最小値")]
    float _minUpperValue = 10;
    protected float MaxLowerValue { get => _maxLowerValue; }
    [SerializeField, Header("下降時最大値")]
    float _maxLowerValue = 10;
    protected float MinLowerValue { get => _minLowerValue; }
    [SerializeField, Header("下降時最小値")]
    float _minLowerValue = 5;
    protected float SaleDecreaseValue { get => _saleDecreaseValue; }
    [SerializeField, Header("売却減少値")]
    float _saleDecreaseValue = 1;
    protected float BlockEnergy { get => _blockEnergy; }
    [SerializeField, Header("鉱石エネルギー")]
    float _blockEnergy = 10;

    [SerializeField, Header("耐久値")]
    float _enduranceValue = 50;

    [SerializeField, Header("耐久値から攻撃力を引くまでにかける時間")]
    float _attackTime = 1;

    private void Start()
    {

        //Debug用
        Rigidbody rb = GetComponent<Rigidbody>();

        // RigidbodyのsleepThresholdを0に設定して、Sleepを無効化
        rb.sleepThreshold = 0f;

    }

    private void Update()
    {
        if (_enduranceValue <= 0)
        {
            DeathAction();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        DrillDig(5);//Debug用の処理
    }

    public void DrillDig(float attackValue)
    {
        _enduranceValue -= attackValue / _attackTime * Time.deltaTime;
        Debug.Log("DrillDigが実行中です");
    }

    private void DeathAction()
    {
        Destroy(gameObject);
        Debug.Log("DeathActionが実行されました");
    }
}
