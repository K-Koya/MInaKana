using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �v���C���[���U�����鎞�ɕ\������A�R�}���h���̓^�C�~���O�̕]����\������G�t�F�N�g
/// </summary>
public class GUIEvaluation : MonoBehaviour
{
    [SerializeField, Tooltip("�]�� OK �̎��ɕ\������摜")]
    Sprite _ImageOK = default;

    [SerializeField, Tooltip("�]�� Good �̎��ɕ\������摜")]
    Sprite _ImageGood = default;

    [SerializeField, Tooltip("�]�� Great �̎��ɕ\������摜")]
    Sprite _ImageGreat = default;

    [SerializeField, Tooltip("�]�� Excellent �̎��ɕ\������摜")]
    Sprite _ImageExcellent = default;


    /// <summary> �]���摜�\���p��Image�R���|�[�l���g </summary>
    Image _EvaluationImage = default;

    /// <summary> ���o�p�A�j���[�^�[ </summary>
    Animator _Animator = default;

    // Start is called before the first frame update
    void Start()
    {
        _EvaluationImage = GetComponentInChildren<Image>();
        _Animator = GetComponentInChildren<Animator>();

        //�񊈐�
        _EvaluationImage.enabled = false;
    }

    /// <summary> �]���p�A�j���[�V���������s </summary>
    /// <param name="output"> �\������]���l </param>
    /// <param name="position"> �\������ʒu </param>
    public void DoAnimation(InputEvaluation output, Vector3 position)
    {
        //�\��������摜��I��
        string animName = "EffectForCommon";
        switch (output)
        {
            case InputEvaluation.OK:
                _EvaluationImage.sprite = _ImageOK;
                break;
            case InputEvaluation.Good:
                _EvaluationImage.sprite = _ImageGood;
                break;
            case InputEvaluation.Great:
                _EvaluationImage.sprite = _ImageGreat;
                break;
            case InputEvaluation.Excellent:
                _EvaluationImage.sprite = _ImageExcellent;
                animName = "EffectForExcellent";
                break;
            default:
                return;
        }

        //�w��ʒu�ōĐ�
        transform.position = position;
        _Animator.Play(animName, 0, 0f);
    }
}
