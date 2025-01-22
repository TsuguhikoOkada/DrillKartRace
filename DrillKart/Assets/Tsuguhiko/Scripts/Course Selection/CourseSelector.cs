using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening; // DOTweenを使用

/// <summary>
/// CourseObjectの選択を左右キーで切り替え、EnterキーでEventSystemを有効化、戻るボタンで操作を再開するクラス。
/// </summary>
public class CourseSelector : MonoBehaviour
{
    /// <summary>
    /// 操作対象のCourseオブジェクトリスト。
    /// </summary>
    [SerializeField] private Transform[] courseTransforms;

    /// <summary>
    /// セレクト中のオブジェクトが移動するターゲット位置。
    /// </summary>
    [SerializeField] private Transform courseObject;

    /// <summary>
    /// 移動にかかる時間（秒）。
    /// </summary>
    [SerializeField] private float moveDuration = 0.5f;

    /// <summary>
    /// EventSystemを管理するオブジェクト。
    /// </summary>
    [SerializeField] private GameObject eventSystem;

    /// <summary>
    /// 現在選択中のオブジェクトのインデックス。
    /// </summary>
    private int currentIndex = 0;

    /// <summary>
    /// キー操作を有効にするかどうか。
    /// </summary>
    private bool inputEnabled = true;

    /// <summary>
    /// 現在の移動アニメーションが完了しているかどうか。
    /// </summary>
    private int movingObjectsCount = 0;

    /// <summary>
    /// 戻るボタンの参照。
    /// </summary>
    [SerializeField] private Button backButton;

    /// <summary>
    /// 戻るボタンが押されたかを判定するフラグ。
    /// </summary>
    private bool isBackButtonPressed = false;

    void Start()
    {
        // EventSystemを初期状態で無効化
        if (eventSystem != null)
        {
            eventSystem.SetActive(false);
        }

        // 戻るボタンのクリックイベント登録
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonPressed);
        }
    }

    void Update()
    {
        if (inputEnabled && movingObjectsCount == 0)
        {
            // 右キーで右回り
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RotatePositions(-1); // 右回り
            }

            // 左キーで左回り
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RotatePositions(1); // 左回り
            }
        }

        // EnterキーでEventSystemを有効化
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnableEventSystem();
        }

        // 戻るボタンが押された場合の処理
        if (isBackButtonPressed)
        {
            DisableEventSystemAndEnableInput();
            isBackButtonPressed = false; // フラグをリセット
        }
    }

    /// <summary>
    /// 戻るボタンが押された際にフラグを立てる。
    /// </summary>
    private void OnBackButtonPressed()
    {
        isBackButtonPressed = true;
    }

    /// <summary>
    /// Courseオブジェクトの位置を回転させます。
    /// </summary>
    /// <param name="direction">回転方向（1: 左回り, -1: 右回り）。</param>
    private void RotatePositions(int direction)
    {
        Vector3[] currentPositions = new Vector3[courseTransforms.Length];
        for (int i = 0; i < courseTransforms.Length; i++)
        {
            currentPositions[i] = courseTransforms[i].position;
        }

        movingObjectsCount = courseTransforms.Length;
        for (int i = 0; i < courseTransforms.Length; i++)
        {
            int nextIndex = (i + direction + courseTransforms.Length) % courseTransforms.Length;

            courseTransforms[i].DOMove(currentPositions[nextIndex], moveDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    movingObjectsCount--;
                });
        }

        currentIndex = (currentIndex + direction + courseTransforms.Length) % courseTransforms.Length;
    }

    /// <summary>
    /// EventSystemを有効化します。
    /// </summary>
    private void EnableEventSystem()
    {
        if (eventSystem != null)
        {
            eventSystem.SetActive(true);
            inputEnabled = false;
            Debug.Log("EventSystem Enabled");
        }
    }

    /// <summary>
    /// EventSystemを無効化し、キー操作を再開します。
    /// </summary>
    private void DisableEventSystemAndEnableInput()
    {
        if (eventSystem != null)
        {
            eventSystem.SetActive(false);
            inputEnabled = true;
            Debug.Log("EventSystem Disabled, Input Re-enabled");
        }
    }
}
