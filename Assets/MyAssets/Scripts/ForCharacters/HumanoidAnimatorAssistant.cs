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
    Transform _LookTarget = default;

    /// <summary> 見るターゲット座標をゆっくり追いかけるために保管する座標 </summary>
    Vector3 _LeapLookTargetPotition = Vector3.zero;

    /// <summary> ゆっくり追いかける速さ </summary>
    float _LeapSpeed = 1f;

    [SerializeField, Tooltip("Look Targetを見るときのモード")]
    IKLookAtMode _LookAtMode = IKLookAtMode.NoLook;

    [SerializeField, Tooltip("どれくらい見るか"), Range(0f, 1f)]
    float _LookTargetWeight = 0;

    [SerializeField, Tooltip("身体をどれくらい向けるか"), Range(0f, 1f)]
    float _LookTargetBodyWeight = 0;

    [SerializeField, Tooltip("頭をどれくらい向けるか"), Range(0f, 1f)]
    float _LookTargetHeadWeight = 0;

    [SerializeField, Tooltip("目をどれくらい向けるか"), Range(0f, 1f)]
    float _LookTargetEyesWeight = 0;

    [SerializeField, Tooltip("関節の動きをどれくらい制限するか"), Range(0f, 1f)]
    float _LookTargetClampWeight = 0;

    #region プロパティ
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

    /// <summary> アニメーションをスムーズに遷移させる </summary>
    /// <param name="animName">対応するアニメーション名</param>
    public void PlaySmooth(string animName)
    {
        animator.Play(animName, -1, 1f);
    }

    /// <summary> IKを使い指定方向を見る方法を、体からに設定 </summary>
    public void SetLookAtModeFromBody()
    {
        _LookAtMode = IKLookAtMode.FromBody;
    }

    /// <summary>
    /// 1秒に一回、瞬きを要求
    /// </summary>
    /// <param name="rate">確率(0～1)</param>
    IEnumerator BlinkOrder(float rate = 0.5f)
    {
        WaitForSeconds wait1Second = new WaitForSeconds(1f);
        float clampedRate = Mathf.Clamp01(rate);

        while (enabled)
        {
            //確率で瞬きトリガーを立てる
            if (animator && Random.value < clampedRate) animator.SetTrigger(_ParamNameDoEyeBlink);

            //1秒待機
            yield return wait1Second;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        //見るモードによってIKLookAt設定を分岐
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
        //キャラクターを注視方向へ注目
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

/// <summary> IKを使い指定方向を見るとき、体のどの位置から見るか </summary>
public enum IKLookAtMode : byte
{
    /// <summary> 見ない </summary>
    NoLook = 0,
    /// <summary> 体から </summary>
    FromBody,
    /// <summary> 頭から </summary>
    FromHead,
    /// <summary> 頭だけ </summary>
    OnlyHead,
    /// <summary> 目だけ </summary>
    OnlyEyes,
}