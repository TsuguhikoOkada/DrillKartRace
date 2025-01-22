using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;

public class KartController : MonoBehaviour
{
    private PostProcessVolume postVolume;
    private PostProcessProfile postProfile;

    public Transform kartModel;
    public Transform kartNormal;
    public Rigidbody sphere;

    public List<ParticleSystem> primaryParticles = new List<ParticleSystem>();
    public List<ParticleSystem> secondaryParticles = new List<ParticleSystem>();

    float currentSpeed;
    float rotate, currentRotate;
    int driftDirection;
    float driftPower;
    int driftMode = 0;
    bool first, second, third;
    Color c;

    [Header("Bools")]
    public bool drifting;

    [Header("Parameters")]
    public float maxForwardSpeed = 100f;    // 前進時の最大速度
    public float maxReverseSpeed = -40f;    // 後進時の最大速度（負の値）
    public float baseAccelerationRate = 20f;    // 基本の加速率を調整
    public float decelerationRate = 30f;        // 減速する速さを調整
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;

    [Header("Steering Control")]
    public float maxSteeringAngleNoInput = 30f; // 他の操作がない時の最大ステアリング角度（度数法）

    [Header("Model Parts")]
    public Transform frontWheels;
    public Transform backWheels;
    public Transform steeringWheel;

    [Header("Particles")]
    public Transform wheelParticles;
    public Transform flashParticles;
    public Color[] turboColors;

    [Header("Camera Control")]
    public Transform cameraTransform; // カメラのTransform参照
    public float cameraPanSpeed = 2f; // カメラのパン速度
    public float cameraPanAmount = 2f; // カメラのパン幅

    [Header("Drift Steering")]
    [Tooltip("ドリフト中のステアリングの強さを制限する係数")]
    public float driftSteeringFactor = 0.5f;

    [Header("Dash Control")]
    public float[] dashBoostFactors = new float[4]; // ドリフト段階ごとのダッシュブースト量（インデックス1,2,3を使用）

    [Header("Acceleration Control")]
    public float accelerationTime = 2.0f;        // 加速が最大になるまでの時間
    public float decelerationTime = 2.0f;        // 減速が最大になるまでの時間
    private float currentAcceleration = 0f;      // 現在の加速度
    private float currentDeceleration = 0f;      // 現在の減速度

    [Header("Drift Control")]
    public float minimumDriftSpeed = 5f; // ドリフトを開始できる最小速度

    // CinemachineImpulseSourceのキャッシュ
    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        postVolume = Camera.main.GetComponent<PostProcessVolume>();
        postProfile = postVolume.profile;

        // カメラTransformの自動設定（オプション）
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // CinemachineImpulseSourceをキャッシュ
        GameObject vcam1 = GameObject.Find("CM vcam1");
        if (vcam1 != null)
        {
            impulseSource = vcam1.GetComponent<CinemachineImpulseSource>();
        }
        else
        {
            Debug.LogWarning("CinemachineImpulseSourceが見つかりません。名前を確認してください。");
        }

        for (int i = 0; i < wheelParticles.GetChild(0).childCount; i++)
        {
            primaryParticles.Add(wheelParticles.GetChild(0).GetChild(i).GetComponent<ParticleSystem>());
        }

        for (int i = 0; i < wheelParticles.GetChild(1).childCount; i++)
        {
            primaryParticles.Add(wheelParticles.GetChild(1).GetChild(i).GetComponent<ParticleSystem>());
        }

        foreach (ParticleSystem p in flashParticles.GetComponentsInChildren<ParticleSystem>())
        {
            secondaryParticles.Add(p);
        }

        // ドリフト段階ごとのダッシュブースト量を設定
        dashBoostFactors[1] = 10f; // ドリフト段階1
        dashBoostFactors[2] = 15f; // ドリフト段階2
        dashBoostFactors[3] = 20f; // ドリフト段階3
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // カートの位置をスムーズに追従
        transform.position = sphere.transform.position - new Vector3(0, 0.4f, 0);

        // 前進・後進の動作（加速度による制御）
        if (verticalInput > 0)
        {
            // 加速度を徐々に増加
            currentAcceleration += Time.deltaTime / accelerationTime;
            currentAcceleration = Mathf.Clamp(currentAcceleration, 0f, 1f);
            currentDeceleration = 0f; // 減速度をリセット

            // 速度を更新
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxForwardSpeed * currentAcceleration, baseAccelerationRate * Time.deltaTime);
        }
        else if (verticalInput < 0)
        {
            // 加速度を徐々に増加（後進）
            currentAcceleration += Time.deltaTime / accelerationTime;
            currentAcceleration = Mathf.Clamp(currentAcceleration, 0f, 1f);
            currentDeceleration = 0f; // 減速度をリセット

            // 速度を更新
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxReverseSpeed * currentAcceleration, baseAccelerationRate * Time.deltaTime);
        }
        else
        {
            // アクセルが押されていない場合、加速度をリセットし、減速度を徐々に増加
            currentAcceleration = 0f;
            currentDeceleration += Time.deltaTime / decelerationTime;
            currentDeceleration = Mathf.Clamp(currentDeceleration, 0f, 1f);

            // 速度を減速
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, decelerationRate * currentDeceleration * Time.deltaTime);
        }

        // ステアリング制御
        if (horizontalInput != 0)
        {
            int dir = horizontalInput > 0 ? 1 : -1;
            float amount = Mathf.Abs(horizontalInput);

            // 他の操作（前進・後進）がない場合にステアリング角度を制限
            if (!drifting && Mathf.Abs(verticalInput) < 0.1f)
            {
                // 最大ステアリング角度に基づいてamountを制限
                float maxAmount = maxSteeringAngleNoInput / steering; // 例: 30° / 80f = 0.375
                amount = Mathf.Min(amount, maxAmount);
            }

            // ドリフト中はステアリング量を制限
            if (drifting)
            {
                amount *= driftSteeringFactor;
            }

            Steer(dir, amount);
        }

        HandleDrift(horizontalInput);

        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        rotate = 0f;

        UpdateAnimations(horizontalInput, verticalInput);

        // 速度を最大速度に制限
        currentSpeed = Mathf.Clamp(currentSpeed, maxReverseSpeed, maxForwardSpeed);
    }

    void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        UpdateCameraPosition(horizontalInput);
    }

    void UpdateCameraPosition(float horizontalInput)
    {
        // カートの横方向入力に基づいてカメラをパンさせる
        float targetPan = horizontalInput * cameraPanAmount;
        Vector3 targetPosition = cameraTransform.localPosition;

        // Time.unscaledDeltaTimeを使用してTime.timeScaleの影響を受けないようにする
        targetPosition.x = Mathf.Lerp(targetPosition.x, targetPan, Time.unscaledDeltaTime * cameraPanSpeed);
        cameraTransform.localPosition = targetPosition;
    }

    void HandleDrift(float horizontalInput)
    {
        // ドリフト開始時にJumpボタンを使用
        if (Input.GetButtonDown("Jump") && !drifting && horizontalInput != 0 && Mathf.Abs(currentSpeed) > minimumDriftSpeed)
        {
            drifting = true;
            driftDirection = horizontalInput > 0 ? 1 : -1;

            foreach (ParticleSystem p in primaryParticles)
            {
                if (!p.isPlaying)
                {
                    var main = p.main;
                    main.startColor = Color.clear;
                    p.Play();
                }
            }

            kartModel.parent.DOComplete();
            // ドリフト時のジャンプ効果を削除
            // kartModel.parent.DOPunchPosition(transform.up * .2f, .3f, 5, 1);
        }

        if (drifting)
        {
            float control = (driftDirection == 1)
                ? ExtensionMethods.Remap(horizontalInput, -1, 1, 0, 2)
                : ExtensionMethods.Remap(horizontalInput, -1, 1, 2, 0);
            float powerControl = (driftDirection == 1)
                ? ExtensionMethods.Remap(horizontalInput, -1, 1, .2f, 1)
                : ExtensionMethods.Remap(horizontalInput, -1, 1, 1, .2f);

            // ステアリング制限係数を適用
            control *= driftSteeringFactor;

            Steer(driftDirection, control);
            driftPower += powerControl;

            ColorDrift();
        }

        // ドリフト終了時にJumpボタンを使用
        if (Input.GetButtonUp("Jump") && drifting)
        {
            Boost();
        }
    }

    void UpdateAnimations(float horizontalInput, float verticalInput)
    {
        if (!drifting)
        {
            // 他の操作（前進・後進）がない場合、ステアリング角度を制限
            if (Mathf.Abs(verticalInput) < 0.1f)
            {
                float targetYRotation = 90 + (horizontalInput * (maxSteeringAngleNoInput / steering) * steering);
                kartModel.localEulerAngles = Vector3.Lerp(
                    kartModel.localEulerAngles,
                    new Vector3(0, targetYRotation, kartModel.localEulerAngles.z),
                    0.2f
                );
            }
            else
            {
                // 他の操作がある場合、通常のステアリング角度
                float targetYRotation = 90 + (horizontalInput * 15); // 15°程度に制限
                kartModel.localEulerAngles = Vector3.Lerp(
                    kartModel.localEulerAngles,
                    new Vector3(0, targetYRotation, kartModel.localEulerAngles.z),
                    0.2f
                );
            }
        }
        else
        {
            float control = (driftDirection == 1)
                ? ExtensionMethods.Remap(horizontalInput, -1, 1, .5f, 2)
                : ExtensionMethods.Remap(horizontalInput, -1, 1, 2, .5f);

            // ステアリング制限係数を適用
            control *= driftSteeringFactor;

            kartModel.parent.localRotation = Quaternion.Euler(
                0,
                Mathf.LerpAngle(kartModel.parent.localEulerAngles.y, (control * 15) * driftDirection, 0.2f),
                0
            );
        }

        // ホイールの回転アニメーションをcurrentSpeedに基づいて調整
        frontWheels.localEulerAngles = new Vector3(0, (horizontalInput * 15), frontWheels.localEulerAngles.z);
        frontWheels.localEulerAngles += new Vector3(0, 0, currentSpeed * 2);
        backWheels.localEulerAngles += new Vector3(0, 0, currentSpeed * 2);

        steeringWheel.localEulerAngles = new Vector3(-25, 90, ((horizontalInput * 45)));
    }

    private void FixedUpdate()
    {
        // 加速度方向をカートの前方向に固定
        Vector3 accelerationDirection = transform.forward; // カートの前方向

        // ドリフト中でも同じ方向に加速度を適用
        if (drifting)
        {
            accelerationDirection = transform.forward;
        }

        sphere.AddForce(accelerationDirection * currentSpeed, ForceMode.Acceleration);
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        // カートが移動している場合のみ回転を適用
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            transform.eulerAngles = Vector3.Lerp(
                transform.eulerAngles,
                new Vector3(0, transform.eulerAngles.y + currentRotate, 0),
                Time.deltaTime * 5f
            );
        }
        else
        {
            // カートが停止している場合、回転をリセット
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

        RaycastHit hitOn, hitNear;

        if (Physics.Raycast(transform.position + (transform.up * 0.1f), Vector3.down, out hitOn, 1.1f, layerMask) &&
            Physics.Raycast(transform.position + (transform.up * 0.1f), Vector3.down, out hitNear, 2.0f, layerMask))
        {
            kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
            kartNormal.Rotate(0, transform.eulerAngles.y, 0);
        }
    }

    public void Boost()
    {
        drifting = false;

        if (driftMode > 0 && driftMode < dashBoostFactors.Length)
        {
            // ドリフト段階に応じたダッシュブーストを取得
            float dashBoost = dashBoostFactors[driftMode];

            // 新しい速度を計算し、最大速度を超えないように制限
            Vector3 newVelocity = sphere.velocity + transform.forward * dashBoost;
            float speed = newVelocity.magnitude;

            if (speed > maxForwardSpeed)
            {
                newVelocity = newVelocity.normalized * maxForwardSpeed;
            }

            sphere.velocity = newVelocity;

            // 視覚効果を適用
            kartModel.Find("Tube001").GetComponentInChildren<ParticleSystem>().Play();
            kartModel.Find("Tube002").GetComponentInChildren<ParticleSystem>().Play();

            // Cinemachine Impulseを生成
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse();
            }
        }

        // ドリフト関連の変数をリセット
        driftPower = 0;
        driftMode = 0;
        first = false;
        second = false;
        third = false;

        foreach (ParticleSystem p in primaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = Color.clear;
            p.Stop();
        }

        kartModel.parent.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.OutBack);
    }

    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount;
    }

    public void ColorDrift()
    {
        if (!first) c = Color.clear;

        if (driftPower > 50 && driftPower < 100 - 1 && !first)
        {
            first = true;
            c = turboColors[0];
            driftMode = 1;

            PlayFlashParticle(c);
        }

        if (driftPower > 100 && driftPower < 150 - 1 && !second)
        {
            second = true;
            c = turboColors[1];
            driftMode = 2;

            PlayFlashParticle(c);
        }

        if (driftPower > 150 && !third)
        {
            third = true;
            c = turboColors[2];
            driftMode = 3;

            PlayFlashParticle(c);
        }

        foreach (ParticleSystem p in primaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = c;
        }

        foreach (ParticleSystem p in secondaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = c;
        }
    }

    void PlayFlashParticle(Color c)
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }

        foreach (ParticleSystem p in secondaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = c;
            p.Play();
        }
    }

    private void Speed(float x)
    {
        currentSpeed = x;
    }

    void ChromaticAmount(float x)
    {
        if (Mathf.Abs(postProfile.GetSetting<ChromaticAberration>().intensity.value - x) > 0.01f)
        {
            postProfile.GetSetting<ChromaticAberration>().intensity.value = x;
        }
    }
}
