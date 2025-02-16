using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    BoxCollider _collider;
    [SerializeField, Header("耐久値"), Tooltip("耐久値")] float _enduranceValue = 50;

    [SerializeField, Header("耐久値から攻撃力を引くまでにかける時間"),
        Tooltip("耐久値から攻撃力を引くまでにかける時間"), Range(1, 10)]
    float _attackTime = 1;

    private void Start()
    {
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

    private void DeathAction()
    {
        Destroy(gameObject);
        Debug.Log("DeathActionが実行されました");
    }
}
