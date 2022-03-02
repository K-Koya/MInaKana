using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HumanoidAnimatorAssistant : MonoBehaviour
{
    Animator animator = default;

    [Header("該当のAnimatorで登録されているパラメーター名")]
    [SerializeField, Tooltip("パラメーター名 : 目の瞬き")]
    string _ParamNameDoEyeBlink = "DoEyeBlink";

    [SerializeField, Tooltip("パラメーター名 : 目を開けている度合(0～1)")]
    string _ParamNameEyeOpenRatio = "EyeOpenRatio";

    [Header("IK用パラメータ")]
    [SerializeField, Tooltip("見るターゲット")]
    Transform lookTarget = default;

    [SerializeField, Tooltip("どれくらい見るか"), Range(0f, 1f)]
    float lookTargetWeight = 0;

    [SerializeField, Tooltip("身体をどれくらい向けるか"), Range(0f, 1f)]
    float lookTargetBodyWeight = 0;

    [SerializeField, Tooltip("頭をどれくらい向けるか"), Range(0f, 1f)]
    float lookTargetHeadWeight = 0;

    [SerializeField, Tooltip("目をどれくらい向けるか"), Range(0f, 1f)]
    float lookTargetEyesWeight = 0;

    [SerializeField, Tooltip("関節の動きをどれくらい制限するか"), Range(0f, 1f)]
    float lookTargetClampWeight = 0;

    #region プロパティ
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
    /// 1秒に一回、瞬きを要求
    /// </summary>
    /// <param name="rate">確率(0～1)</param>
    IEnumerator BlinkOrder(float rate = 0.2f)
    {
        WaitForSeconds wait1Second = new WaitForSeconds(1f);
        float clampedRate = Mathf.Clamp01(rate);

        while (enabled)
        {
            //確率で瞬きトリガーを立てる
            if (Random.value < clampedRate) animator.SetTrigger(_ParamNameDoEyeBlink);

            //1秒待機
            yield return wait1Second;
        }
    }

    /// <summary>IKを用いて指定した方向へ、胴体から向かせる</summary>
    public void IKLookAtFromBody()
    {
        //キャラクターの注視方向に関するIKを設定
        animator.SetLookAtWeight(lookTargetWeight, lookTargetBodyWeight, lookTargetHeadWeight, lookTargetEyesWeight, lookTargetClampWeight);
        //キャラクターを注視方向へ注目
        if (lookTarget) animator.SetLookAtPosition(lookTarget.position);
    }

    /// <summary>IKを用いて指定した方向へ、頭から向かせる</summary>
    public void IKLookAtFromHead()
    {
        //キャラクターの注視方向に関するIKを設定
        animator.SetLookAtWeight(lookTargetWeight, 0f, lookTargetHeadWeight, lookTargetEyesWeight, lookTargetClampWeight);
        //キャラクターを注視方向へ注目
        if (lookTarget) animator.SetLookAtPosition(lookTarget.position);
    }

    /// <summary>IKを用いて指定した方向へ、目だけ向かせる</summary>
    public void IKLookAtOnlyEye()
    {
        //キャラクターの注視方向に関するIKを設定
        animator.SetLookAtWeight(lookTargetWeight, 0f, 0f, lookTargetEyesWeight, lookTargetClampWeight);
        //キャラクターを注視方向へ注目
        if (lookTarget) animator.SetLookAtPosition(lookTarget.position);
    }

    /// <summary>IKを用いて指定した方向へ、頭だけ向かせる</summary>
    public void IKLookAtOnlyHead()
    {
        //キャラクターの注視方向に関するIKを設定
        animator.SetLookAtWeight(lookTargetWeight, 0f, lookTargetHeadWeight, 0f, lookTargetClampWeight);
        //キャラクターを注視方向へ注目
        if (lookTarget) animator.SetLookAtPosition(lookTarget.position);
    }
}
