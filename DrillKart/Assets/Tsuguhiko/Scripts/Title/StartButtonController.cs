using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening; // DOTween���g�p

/// <summary>
/// �X�^�[�g�{�^���̃N���b�N��SE���Đ����A��ʂ��t�F�[�h�A�E�g������N���X�B
/// </summary>
public class StartButtonController : MonoBehaviour
{
    /// <summary>
    /// �t�F�[�h�A�E�g�p��Image�R���|�[�l���g�B
    /// </summary>
    [SerializeField] private Image fadeImage;

    /// <summary>
    /// �t�F�[�h�A�E�g�ɂ����鎞�ԁi�b�j�B
    /// </summary>
    [SerializeField] private float fadeDuration = 1.0f;

    /// <summary>
    /// �Đ�������ʉ��iSE�j�B
    /// </summary>
    [SerializeField] private AudioClip startSE;

    /// <summary>
    /// AudioSource�R���|�[�l���g�B
    /// </summary>
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// �J�ڐ�̃V�[�����B
    /// </summary>
    [SerializeField] private string nextSceneName;

    /// <summary>
    /// �t�F�[�h�p�l���̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    [SerializeField] private GameObject fadePanel;

    /// <summary>
    /// �X�^�[�g�{�^�����N���b�N���ꂽ�ۂ̏����B
    /// </summary>
    public void OnStartButtonClicked()
    {
        // �t�F�[�h�p�l����L����
        if (fadePanel != null)
        {
            fadePanel.SetActive(true);
        }

        // ���ʉ����Đ�
        if (audioSource != null && startSE != null)
        {
            audioSource.PlayOneShot(startSE);
        }

        // �t�F�[�h�A�E�g�����ƐF�ύX
        if (fadeImage != null)
        {
            // �t�F�[�h�A�E�g�����F�����x��ς����ɂ���
            fadeImage.DOColor(Color.black, fadeDuration)
                .OnComplete(() =>
                {
                    // �t�F�[�h�A�E�g������ɃV�[����J��
                    SceneManager.LoadScene(nextSceneName);
                });
        }
    }
}