using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening; // DOTween���g�p

/// <summary>
/// CourseObject�̑I�������E�L�[�Ő؂�ւ��AEnter�L�[��EventSystem��L�����ABackspace�L�[�ő�����ĊJ����N���X�B
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

    void Start()
    {
        // EventSystem��������ԂŖ�����
        if (eventSystem != null)
        {
            eventSystem.SetActive(false);
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

        // Enter�L�[��EventSystem��L�����i��Ɏ󂯕t����j
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnableEventSystem();
        }

        // Backspace�L�[�ő�����ĊJ
        if (!inputEnabled && Input.GetKeyDown(KeyCode.Backspace))
        {
            DisableEventSystemAndEnableInput();
        }
    }

    /// <summary>
    /// Course�I�u�W�F�N�g�̈ʒu����]�����܂��B
    /// </summary>
    /// <param name="direction">��]�����i1: �����, -1: �E���j�B</param>
    private void RotatePositions(int direction)
    {
        // ���݂�Transform���W���ꎞ�I�ɕۑ�
        Vector3[] currentPositions = new Vector3[courseTransforms.Length];
        for (int i = 0; i < courseTransforms.Length; i++)
        {
            currentPositions[i] = courseTransforms[i].position;
        }

        // �e�I�u�W�F�N�g��V�����ʒu�Ɉړ�
        movingObjectsCount = courseTransforms.Length;
        for (int i = 0; i < courseTransforms.Length; i++)
        {
            int nextIndex = (i + direction + courseTransforms.Length) % courseTransforms.Length;

            // ���炩�Ɉړ�
            courseTransforms[i].DOMove(currentPositions[nextIndex], moveDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    // �e�I�u�W�F�N�g�̈ړ�������������J�E���g�����炷
                    movingObjectsCount--;
                });
        }

        // �I�𒆂̃C���f�b�N�X���X�V
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
            inputEnabled = false; // �L�[����𖳌���
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
            inputEnabled = true; // �L�[�����L����
            Debug.Log("EventSystem Disabled, Input Re-enabled");
        }
    }
}
