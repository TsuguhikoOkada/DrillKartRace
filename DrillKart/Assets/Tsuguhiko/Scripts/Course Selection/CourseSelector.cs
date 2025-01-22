using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening; // DOTweenを使用

/// <summary>
/// CourseObjectの選択を左右キーで切り替え、EnterキーでEventSystemを有効化、Backspaceキーで操作を再開するクラス。
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

    void Start()
    {
        // EventSystemを初期状態で無効化
        if (eventSystem != null)
        {
            eventSystem.SetActive(false);
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

        // EnterキーでEventSystemを有効化（常に受け付ける）
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnableEventSystem();
        }

        // Backspaceキーで操作を再開
        if (!inputEnabled && Input.GetKeyDown(KeyCode.Backspace))
        {
            DisableEventSystemAndEnableInput();
        }
    }

    /// <summary>
    /// Courseオブジェクトの位置を回転させます。
    /// </summary>
    /// <param name="direction">回転方向（1: 左回り, -1: 右回り）。</param>
    private void RotatePositions(int direction)
    {
        // 現在のTransform座標を一時的に保存
        Vector3[] currentPositions = new Vector3[courseTransforms.Length];
        for (int i = 0; i < courseTransforms.Length; i++)
        {
            currentPositions[i] = courseTransforms[i].position;
        }

        // 各オブジェクトを新しい位置に移動
        movingObjectsCount = courseTransforms.Length;
        for (int i = 0; i < courseTransforms.Length; i++)
        {
            int nextIndex = (i + direction + courseTransforms.Length) % courseTransforms.Length;

            // 滑らかに移動
            courseTransforms[i].DOMove(currentPositions[nextIndex], moveDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    // 各オブジェクトの移動が完了したらカウントを減らす
                    movingObjectsCount--;
                });
        }

        // 選択中のインデックスを更新
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
            inputEnabled = false; // キー操作を無効化
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
            inputEnabled = true; // キー操作を有効化
            Debug.Log("EventSystem Disabled, Input Re-enabled");
        }
    }
}
