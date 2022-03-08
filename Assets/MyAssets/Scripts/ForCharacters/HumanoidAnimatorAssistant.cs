using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HumanoidAnimatorAssistant : MonoBehaviour
{
    Animator animator = default;

    [Header("�Y����Animator�œo�^����Ă���p�����[�^�[��")]
    [SerializeField, Tooltip("�p�����[�^�[�� : �ڂ̏u��")]
    string _ParamNameDoEyeBlink = "DoEyeBlink";

    [SerializeField, Tooltip("�p�����[�^�[�� : �ڂ��J���Ă���x��(0�`1)")]
    string _ParamNameEyeOpenRatio = "EyeOpenRatio";

    [Header("IK�p�p�����[�^")]
    [SerializeField, Tooltip("����^�[�Q�b�g")]
    Transform _LookTarget = default;

    /// <summary> ����^�[�Q�b�g���W���������ǂ������邽�߂ɕۊǂ�����W </summary>
    Vector3 _LeapLookTargetPotition = Vector3.zero;

    /// <summary> �������ǂ������鑬�� </summary>
    float _LeapSpeed = 1f;

    [SerializeField, Tooltip("Look Target������Ƃ��̃��[�h")]
    IKLookAtMode _LookAtMode = IKLookAtMode.NoLook;

    [SerializeField, Tooltip("�ǂꂭ�炢���邩"), Range(0f, 1f)]
    float _LookTargetWeight = 0;

    [SerializeField, Tooltip("�g�̂��ǂꂭ�炢�����邩"), Range(0f, 1f)]
    float _LookTargetBodyWeight = 0;

    [SerializeField, Tooltip("�����ǂꂭ�炢�����邩"), Range(0f, 1f)]
    float _LookTargetHeadWeight = 0;

    [SerializeField, Tooltip("�ڂ��ǂꂭ�炢�����邩"), Range(0f, 1f)]
    float _LookTargetEyesWeight = 0;

    [SerializeField, Tooltip("�֐߂̓������ǂꂭ�炢�������邩"), Range(0f, 1f)]
    float _LookTargetClampWeight = 0;

    #region �v���p�e�B
    public Transform LookTarget { set => _LookTarget = value; }
    public float LeapSpeed { set => _LeapSpeed = value; }
    public IKLookAtMode LookAtMode { set => _LookAtMode = value; }
    public float LookTargetWeight { set => _LookTargetWeight = value; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        StartCoroutine(BlinkOrder());
    }

    /// <summary> �A�j���[�V�������X���[�Y�ɑJ�ڂ����� </summary>
    /// <param name="animName">�Ή�����A�j���[�V������</param>
    public void PlaySmooth(string animName)
    {
        animator.Play(animName, -1, 1f);
    }

    /// <summary> IK���g���w�������������@���A�̂���ɐݒ� </summary>
    public void SetLookAtModeFromBody()
    {
        _LookAtMode = IKLookAtMode.FromBody;
    }

    /// <summary>
    /// 1�b�Ɉ��A�u����v��
    /// </summary>
    /// <param name="rate">�m��(0�`1)</param>
    IEnumerator BlinkOrder(float rate = 0.5f)
    {
        WaitForSeconds wait1Second = new WaitForSeconds(1f);
        float clampedRate = Mathf.Clamp01(rate);

        while (enabled)
        {
            //�m���ŏu���g���K�[�𗧂Ă�
            if (animator && Random.value < clampedRate) animator.SetTrigger(_ParamNameDoEyeBlink);

            //1�b�ҋ@
            yield return wait1Second;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        //���郂�[�h�ɂ����IKLookAt�ݒ�𕪊�
        switch (_LookAtMode)
        {
            case IKLookAtMode.FromBody:
                animator.SetLookAtWeight(_LookTargetWeight, _LookTargetBodyWeight, _LookTargetHeadWeight, _LookTargetEyesWeight, _LookTargetClampWeight);
                break;

            case IKLookAtMode.FromHead:
                animator.SetLookAtWeight(_LookTargetWeight, 0f, _LookTargetHeadWeight, _LookTargetEyesWeight, _LookTargetClampWeight);
                break;

            case IKLookAtMode.OnlyHead:
                animator.SetLookAtWeight(_LookTargetWeight, 0f, _LookTargetHeadWeight, 0f, _LookTargetClampWeight);
                break;

            case IKLookAtMode.OnlyEyes:
                animator.SetLookAtWeight(_LookTargetWeight, 0f, 0f, _LookTargetEyesWeight, _LookTargetClampWeight);
                break;

            default:
                animator.SetLookAtWeight(0f, 0f, 0f, 0f, 0f);
                break;

        }
        //�L�����N�^�[�𒍎������֒���
        if (_LookTarget && _LookTargetWeight > 0f)
        {
            _LeapLookTargetPotition = Vector3.Lerp(_LeapLookTargetPotition, _LookTarget.position, 1f / _LeapSpeed);
            animator.SetLookAtPosition(_LeapLookTargetPotition);
        }
        else
        {
            animator.SetLookAtWeight(0f);
        }
    }
}

/// <summary> IK���g���w�����������Ƃ��A�̂̂ǂ̈ʒu���猩�邩 </summary>
public enum IKLookAtMode : byte
{
    /// <summary> ���Ȃ� </summary>
    NoLook = 0,
    /// <summary> �̂��� </summary>
    FromBody,
    /// <summary> ������ </summary>
    FromHead,
    /// <summary> ������ </summary>
    OnlyHead,
    /// <summary> �ڂ��� </summary>
    OnlyEyes,
}