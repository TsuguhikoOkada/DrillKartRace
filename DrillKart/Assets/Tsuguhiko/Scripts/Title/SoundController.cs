using UnityEngine;

/// <summary>
/// BGM�����[�v�Đ����A�Q�[���T�C�N�����ɔj������Ȃ��悤�ɊǗ�����N���X�B
/// </summary>
public class SoundController : MonoBehaviour
{
    /// <summary>
    /// �Đ�����BGM��AudioClip�B
    /// </summary>
    [SerializeField] private AudioClip bgmClip;

    /// <summary>
    /// AudioSource�R���|�[�l���g�B
    /// </summary>
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// �Q�[���J�n����BGM�̃��[�v�Đ���ݒ肵�ASoundController��j������Ȃ��悤�ɂ���B
    /// </summary>
    void Awake()
    {
        // SoundController���j������Ȃ��悤�ɐݒ�
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
        {
            Debug.LogError("AudioSource���A�T�C������Ă��܂���");
            return;
        }

        if (bgmClip == null)
        {
            Debug.LogError("BGM Clip���A�T�C������Ă��܂���");
            return;
        }

        // AudioSource�̐ݒ�
        audioSource.clip = bgmClip;
        audioSource.loop = true; // ���[�v�Đ���L����
        audioSource.playOnAwake = false; // �����Đ��𖳌���
    }

    /// <summary>
    /// �Q�[���J�n����BGM���Đ�����B
    /// </summary>
    void Start()
    {
        if (audioSource != null && bgmClip != null)
        {
            audioSource.Play();
        }
    }
}
