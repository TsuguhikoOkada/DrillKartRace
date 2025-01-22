using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening; // DOTweenを使用

/// <summary>
/// 決定ボタンの動作クラス
/// </summary>
public class SelectButtonController : MonoBehaviour
{
    /// <summary>
    /// フェードアウト用のImageコンポーネント。
    /// </summary>
    [SerializeField] private Image fadeImage;

    /// <summary>
    /// フェードアウトにかかる時間（秒）。
    /// </summary>
    [SerializeField] private float fadeDuration = 1.0f;

    /// <summary>
    /// 再生する効果音（SE）。
    /// </summary>
    [SerializeField] private AudioClip startSE;

    /// <summary>
    /// AudioSourceコンポーネント。
    /// </summary>
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// 遷移先のシーン名。
    /// </summary>
    [SerializeField] private string nextSceneName;

    /// <summary>
    /// フェードパネルのゲームオブジェクト。
    /// </summary>
    [SerializeField] private GameObject fadePanel;

    /// <summary>
    /// スタートボタンがクリックされた際の処理。
    /// </summary>
    public void OnSelectButtonClicked()
    {
        // フェードパネルを有効化
        if (fadePanel != null)
        {
            fadePanel.SetActive(true);
        }

        // 効果音を再生
        if (audioSource != null && startSE != null)
        {
            audioSource.PlayOneShot(startSE);
        }

        // フェードアウト処理と色変更
        if (fadeImage != null)
        {
            // フェードアウト処理：透明度を変えつつ黒にする
            fadeImage.DOColor(Color.black, fadeDuration)
                .OnComplete(() =>
                {
                    // フェードアウト完了後にシーンを遷移
                    SceneManager.LoadScene(nextSceneName);
                });
        }
    }
}