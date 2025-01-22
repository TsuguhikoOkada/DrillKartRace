using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening; // DOTween���g�p

/// <summary>
/// CourseObject�̑I�������E�L�[�Ő؂�ւ��AEnter�L�[��EventSystem��L�����A�߂�{�^���ő�����ĊJ����N���X�B
/// </summary>
public class CourseSelector : MonoBehaviour
{
    /// <summary>
    /// ����Ώۂ�Course�I�u�W�F�N�g���X�g�B
    /// </summary>
    [SerializeField] private Transform[] courseTransforms;

    /// <summary>
    /// �Z���N�g���̃I�u�W�F�N�g���ړ�����^�[�Q�b�g�ʒu�B
    /// </summary>
    [SerializeField] private Transform courseObject;

    /// <summary>
    /// �ړ��ɂ����鎞�ԁi�b�j�B
    /// </summary>
    [SerializeField] private float moveDuration = 0.5f;

    /// <summary>
    /// EventSystem���Ǘ�����I�u�W�F�N�g�B
    /// </summary>
    [SerializeField] private GameObject eventSystem;

    /// <summary>
    /// ���ݑI�𒆂̃I�u�W�F�N�g�̃C���f�b�N�X�B
    /// </summary>
    private int currentIndex = 0;

    /// <summary>
    /// �L�[�����L���ɂ��邩�ǂ����B
    /// </summary>
    private bool inputEnabled = true;

    /// <summary>
    /// ���݂̈ړ��A�j���[�V�������������Ă��邩�ǂ����B
    /// </summary>
    private int movingObjectsCount = 0;

    /// <summary>
    /// �߂�{�^���̎Q�ƁB
    /// </summary>
    [SerializeField] private Button backButton;

    /// <summary>
    /// �߂�{�^���������ꂽ���𔻒肷��t���O�B
    /// </summary>
    private bool isBackButtonPressed = false;

    void Start()
    {
        // EventSystem��������ԂŖ�����
        if (eventSystem != null)
        {
            eventSystem.SetActive(false);
        }

        // �߂�{�^���̃N���b�N�C�x���g�o�^
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonPressed);
        }
    }

    void Update()
    {
        if (inputEnabled && movingObjectsCount == 0)
        {
            // �E�L�[�ŉE���
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RotatePositions(-1); // �E���
            }

            // ���L�[�ō����
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RotatePositions(1); // �����
            }
        }

        // Enter�L�[��EventSystem��L����
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnableEventSystem();
        }

        // �߂�{�^���������ꂽ�ꍇ�̏���
        if (isBackButtonPressed)
        {
            DisableEventSystemAndEnableInput();
            isBackButtonPressed = false; // �t���O�����Z�b�g
        }
    }

    /// <summary>
    /// �߂�{�^���������ꂽ�ۂɃt���O�𗧂Ă�B
    /// </summary>
    private void OnBackButtonPressed()
    {
        isBackButtonPressed = true;
    }

    /// <summary>
    /// Course�I�u�W�F�N�g�̈ʒu����]�����܂��B
    /// </summary>
    /// <param name="direction">��]�����i1: �����, -1: �E���j�B</param>
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
    /// EventSystem��L�������܂��B
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
    /// EventSystem�𖳌������A�L�[������ĊJ���܂��B
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
