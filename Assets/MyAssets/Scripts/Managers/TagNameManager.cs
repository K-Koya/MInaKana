using System.Linq;
using UnityEngine;

/// <summary>
/// 使用されるタグ名の文字列を管理
/// </summary>
public class TagNameManager : Singleton<TagNameManager>
{
    [Header("Add Tag… で設定したタグ名の文字列を以下に登録する")]

    #region メンバ
    [SerializeField, Tooltip("メインカメラタグ名")]
    private string _buttonNameMainCamera = "MainCamera";

    [SerializeField, Tooltip("先頭プレイヤータグ名")]
    private string _buttonNamePlayer = "Player";

    [SerializeField, Tooltip("後尾プレイヤータグ名")]
    private string _buttonNameAllies = "Allies";

    [SerializeField, Tooltip("敵タグ名")]
    private string _buttonNameEnemy = "Enemy";
    #endregion

    #region プロパティ
    /// <summary> メインカメラタグ名 </summary>
    public string ButtonNameMainCamera { get => _buttonNameMainCamera; }
    /// <summary> 先頭プレイヤータグ名 </summary>
    public string ButtonNamePlayer { get => _buttonNamePlayer; }
    /// <summary> 後尾プレイヤータグ名 </summary>
    public string ButtonNameAllies { get => _buttonNameAllies; }
    /// <summary> 敵タグ名 </summary>
    public string ButtonNameEnemy { get => _buttonNameEnemy; }
    #endregion

    protected override void Awake()
    {
        IsDontDestroyOnLoad = true;
        base.Awake();
    }
}

/// <summary>
/// 複数のtagとの一致判定をするメソッドを追加
/// </summary>
static partial class Extensions
{
    /// <summary>
    /// CompareTagの複数比較バージョン
    /// </summary>
    /// <param name="obj">対象のゲームオブジェクト</param>
    /// <param name="tags">比較対象タグ</param>
    /// <returns>true : 引数のタグと一つでも一致する</returns>
    public static bool CompareTags(this GameObject obj, string[] tags)
    {
        return tags.Contains(obj.tag);
    }
}