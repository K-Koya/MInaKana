using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーが攻撃する時に表示する、コマンド入力タイミングの評価を表示するエフェクト
/// </summary>
public class GUIEvaluation : MonoBehaviour
{
    [SerializeField, Tooltip("評価 OK の時に表示する画像")]
    Sprite _ImageOK = default;

    [SerializeField, Tooltip("評価 Good の時に表示する画像")]
    Sprite _ImageGood = default;

    [SerializeField, Tooltip("評価 Great の時に表示する画像")]
    Sprite _ImageGreat = default;

    [SerializeField, Tooltip("評価 Excellent の時に表示する画像")]
    Sprite _ImageExcellent = default;


    /// <summary> 評価画像表示用のImageコンポーネント </summary>
    Image _EvaluationImage = default;

    /// <summary> 演出用アニメーター </summary>
    Animator _Animator = default;

    // Start is called before the first frame update
    void Start()
    {
        _EvaluationImage = GetComponentInChildren<Image>();
        _Animator = GetComponentInChildren<Animator>();

        //非活性
        _EvaluationImage.enabled = false;
    }

    /// <summary> 評価用アニメーションを実行 </summary>
    /// <param name="output"> 表示する評価値 </param>
    /// <param name="position"> 表示する位置 </param>
    public void DoAnimation(InputEvaluation output, Vector3 position)
    {
        //表示させる画像を選択
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

        //指定位置で再生
        transform.position = position;
        _Animator.Play(animName, 0, 0f);
    }
}
