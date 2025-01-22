using UnityEngine;

/// <summary>
/// BGMをループ再生し、ゲームサイクル中に破棄されないように管理するクラス。
/// </summary>
public class SoundController : MonoBehaviour
{
    /// <summary>
    /// 再生するBGMのAudioClip。
    /// </summary>
    [SerializeField] private AudioClip bgmClip;

    /// <summary>
    /// AudioSourceコンポーネント。
    /// </summary>
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// ゲーム開始時にBGMのループ再生を設定し、SoundControllerを破棄されないようにする。
    /// </summary>
    void Awake()
    {
        // SoundControllerが破棄されないように設定
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
        {
            Debug.LogError("AudioSourceがアサインされていません");
            return;
        }

        if (bgmClip == null)
        {
            Debug.LogError("BGM Clipがアサインされていません");
            return;
        }

        // AudioSourceの設定
        audioSource.clip = bgmClip;
        audioSource.loop = true; // ループ再生を有効化
        audioSource.playOnAwake = false; // 自動再生を無効化
    }

    /// <summary>
    /// ゲーム開始時にBGMを再生する。
    /// </summary>
    void Start()
    {
        if (audioSource != null && bgmClip != null)
        {
            audioSource.Play();
        }
    }
}
