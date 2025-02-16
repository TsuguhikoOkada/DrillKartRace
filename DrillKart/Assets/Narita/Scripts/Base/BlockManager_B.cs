using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager_B : MonoBehaviour
{
    BoxCollider _collider;
    protected float _initialRate;
    protected float _initialPrice;
    [SerializeField, Header("耐久値")]
    float _enduranceValue = 50;
    [SerializeField, Header("耐久値から攻撃力を引くまでにかける時間")]
    float _attackTime = 1;
    [SerializeField, Header("対応するパラメーターデータ")]
    protected BlockData _data;

    private void Start()
    {
        _initialRate = _data.InitialRate;
        _initialPrice = _data.InitialPrice;

        TryGetComponent(out _collider);

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

    protected virtual void DeathAction()
    {
        Destroy(gameObject);
        Debug.Log("DeathActionが実行されました");
    }
}
