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
    Transform lookTarget = default;

    [SerializeField, Tooltip("�ǂꂭ�炢���邩"), Range(0f, 1f)]
    float lookTargetWeight = 0;

    [SerializeField, Tooltip("�g�̂��ǂꂭ�炢�����邩"), Range(0f, 1f)]
    float lookTargetBodyWeight = 0;

    [SerializeField, Tooltip("�����ǂꂭ�炢�����邩"), Range(0f, 1f)]
    float lookTargetHeadWeight = 0;

    [SerializeField, Tooltip("�ڂ��ǂꂭ�炢�����邩"), Range(0f, 1f)]
    float lookTargetEyesWeight = 0;

    [SerializeField, Tooltip("�֐߂̓������ǂꂭ�炢�������邩"), Range(0f, 1f)]
    float lookTargetClampWeight = 0;

    #region �v���p�e�B
    public Transform LookTarget { set => lookTarget = value; }
    public float LookTargetWeight { set => lookTargetWeight = value; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine(BlinkOrder());
    }

    /// <summary>  </summary>
    /// <param name="animName"></param>
    public void PlaySmooth(string animName)
    {
        animator.Play(animName, -1, 0.7f);
    }

    /// <summary>
    /// 1�b�Ɉ��A�u����v��
    /// </summary>
    /// <param name="rate">�m��(0�`1)</param>
    IEnumerator BlinkOrder(float rate = 0.2f)
    {
        WaitForSeconds wait1Second = new WaitForSeconds(1f);
        float clampedRate = Mathf.Clamp01(rate);

        while (enabled)
        {
            //�m���ŏu���g���K�[�𗧂Ă�
            if (Random.value < clampedRate) animator.SetTrigger(_ParamNameDoEyeBlink);

            //1�b�ҋ@
            yield return wait1Second;
        }
    }

    /// <summary>IK��p���Ďw�肵�������ցA���̂����������</summary>
    public void IKLookAtFromBody()
    {
        //�L�����N�^�[�̒��������Ɋւ���IK��ݒ�
        animator.SetLookAtWeight(lookTargetWeight, lookTargetBodyWeight, lookTargetHeadWeight, lookTargetEyesWeight, lookTargetClampWeight);
        //�L�����N�^�[�𒍎������֒���
        if (lookTarget) animator.SetLookAtPosition(lookTarget.position);
    }

    /// <summary>IK��p���Ďw�肵�������ցA�������������</summary>
    public void IKLookAtFromHead()
    {
        //�L�����N�^�[�̒��������Ɋւ���IK��ݒ�
        animator.SetLookAtWeight(lookTargetWeight, 0f, lookTargetHeadWeight, lookTargetEyesWeight, lookTargetClampWeight);
        //�L�����N�^�[�𒍎������֒���
        if (lookTarget) animator.SetLookAtPosition(lookTarget.position);
    }

    /// <summary>IK��p���Ďw�肵�������ցA�ڂ�����������</summary>
    public void IKLookAtOnlyEye()
    {
        //�L�����N�^�[�̒��������Ɋւ���IK��ݒ�
        animator.SetLookAtWeight(lookTargetWeight, 0f, 0f, lookTargetEyesWeight, lookTargetClampWeight);
        //�L�����N�^�[�𒍎������֒���
        if (lookTarget) animator.SetLookAtPosition(lookTarget.position);
    }

    /// <summary>IK��p���Ďw�肵�������ցA��������������</summary>
    public void IKLookAtOnlyHead()
    {
        //�L�����N�^�[�̒��������Ɋւ���IK��ݒ�
        animator.SetLookAtWeight(lookTargetWeight, 0f, lookTargetHeadWeight, 0f, lookTargetClampWeight);
        //�L�����N�^�[�𒍎������֒���
        if (lookTarget) animator.SetLookAtPosition(lookTarget.position);
    }
}
