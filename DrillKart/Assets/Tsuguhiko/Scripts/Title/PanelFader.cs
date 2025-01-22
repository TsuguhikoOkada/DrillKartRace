using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

/// <summary>
/// DOTween���g�p���āA�w�肳�ꂽImage�R���|�[�l���g���t�F�[�h�C���܂��̓t�F�[�h�A�E�g������N���X�B
/// �t�F�[�h�C�����I�������炱�̃I�u�W�F�N�g�𖳌����B
/// </summary>
public class PanelFader : MonoBehaviour
{
    /// <summary>
    /// �t�F�[�h�Ώۂ�Image�R���|�[�l���g�B
    /// </summary>
    [SerializeField] private Image _targetImage;

    /// <summary>
    /// �t�F�[�h�ɂ����鎞�ԁi�b�j�B
    /// </summary>
    [SerializeField] private float _fadeDuration = 2.0f;

    /// <summary>
    /// �t�F�[�h�̊J�n���̐F�B
    /// </summary>
    [SerializeField] private Color _startColor = Color.black;

    /// <summary>
    /// �t�F�[�h�̏I�����̐F�B
    /// </summary>
    [SerializeField] private Color _endColor = Color.white;

    /// <summary>
    /// DOTween���g�p���ăt�F�[�h�������J�n����B
    /// �t�F�[�h�C��������������I�u�W�F�N�g�𖳌�������B
    /// </summary>
    void Start()
    {
        if (_targetImage == null)
        {
            Debug.LogError("�t�F�[�h�C��/�A�E�g�Ŏg�����߂�Image���A�T�C������Ă��܂���");
            return;
        }

        // �����F��ݒ�
        _targetImage.color = _startColor;

        // DOTween���g���ăt�F�[�h���������s
        _targetImage.DOColor(_endColor, _fadeDuration)
            .OnComplete(() =>
            {
                // �t�F�[�h������ɃI�u�W�F�N�g�𖳌���
                gameObject.SetActive(false);
            });
    }
}