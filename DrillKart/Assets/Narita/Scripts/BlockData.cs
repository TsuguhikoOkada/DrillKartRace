using UnityEngine;

[CreateAssetMenu(menuName ="BlockData/BlockData",fileName = "BlockData")]
public class BlockData : ScriptableObject
{
    public float InitialPrice { get => _initialPrice; }
    [SerializeField, Header("初期価格")]
    float _initialPrice = 100;
    public float MinPrice { get => _minPrice; }
    [SerializeField, Header("最低価格")]
    float _minPrice = 50;
    public float RateTrend { get => _rateTrend; }
    [SerializeField, Header("レート傾向(上昇)")]
    float _rateTrend = 50;
    public float TrendShift { get => _trendShift; }
    [SerializeField, Header("傾向変化量")]
    float _trendShift = 10;
    public float ChangeTiming { get => _changeTiming; }
    [SerializeField, Header("変化タイミング")]
    float _changeTiming = 120;
    public float MaxUpperValue { get => _maxUpperValue; }
    [SerializeField, Header("上昇時最大値")]
    float _maxUpperValue = 20;
    public float MinUpperValue { get => _minUpperValue; }
    [SerializeField, Header("上昇時最小値")]
    float _minUpperValue = 10;
    public float MaxLowerValue { get => _maxLowerValue; }
    [SerializeField, Header("下降時最大値")]
    float _maxLowerValue = 10;
    public float MinLowerValue { get => _minLowerValue; }
    [SerializeField, Header("下降時最小値")]
    float _minLowerValue = 5;
    public float SaleDecreaseValue { get => _saleDecreaseValue; }
    [SerializeField, Header("売却減少値")]
    float _saleDecreaseValue = 1;
    public float BlockEnergy { get => _blockEnergy; }
    [SerializeField, Header("鉱石エネルギー")]
    float _blockEnergy = 10;
}
