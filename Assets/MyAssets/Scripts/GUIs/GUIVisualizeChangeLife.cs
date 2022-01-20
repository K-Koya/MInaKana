using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 被ダメージ値や回復値を表示するエフェクト
/// </summary>
[RequireComponent(typeof(Sprite)), RequireComponent(typeof(Animator))]
public class GUIVisualizeChangeLife : MonoBehaviour
{
    /// <summary> ダメージを数値表示するテキスト </summary>
    Text _ValueText = default;

    /// <summary> 演出用アニメーター </summary>
    Animator _Animator = default;

    // Start is called before the first frame update
    void Start()
    {
        _ValueText = GetComponentInChildren<Text>();
        _Animator = GetComponent<Animator>();
    }

    /// <summary> ダメージ数値を場所を指定して演出を出しながら再生 </summary>
    /// <param name="damage">ダメージ数値</param>
    /// <param name="position">表示する場所</param>
    public void Damage(int damage, Vector3 position)
    {
        //数値指定
        _ValueText.text = damage.ToString();

        //指定位置で再生
        transform.position = position;
        _Animator.Play("DamagedEffect", 0, 0);
    }
}
