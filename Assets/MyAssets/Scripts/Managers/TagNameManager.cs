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
    private string _TagNameMainCamera = "MainCamera";

    [SerializeField, Tooltip("先頭プレイヤータグ名")]
    private string _TagNamePlayer = "Player";

    [SerializeField, Tooltip("後尾プレイヤータグ名")]
    private string _TagNameAllies = "Allies";

    [SerializeField, Tooltip("敵タグ名")]
    private string _TagNameEnemy = "Enemy";

    [SerializeField, Tooltip("先頭プレイヤーの原点位置タグ名")]
    private string _TagNamePlayerBase = "PlayerBase";

    [SerializeField, Tooltip("後尾プレイヤーの原点位置タグ名")]
    private string _TagNameAlliesBase = "AlliesBase";

    [SerializeField, Tooltip("敵の原点位置タグ名")]
    private string _TagNameEnemyBase = "EnemyBase";
    #endregion

    #region プロパティ
    /// <summary> メインカメラタグ名 </summary>
    public string TagNameMainCamera { get => _TagNameMainCamera; }
    /// <summary> 先頭プレイヤータグ名 </summary>
    public string TagNamePlayer { get => _TagNamePlayer; }
    /// <summary> 後尾プレイヤータグ名 </summary>
    public string TagNameAllies { get => _TagNameAllies; }
    /// <summary> 敵タグ名 </summary>
    public string TagNameEnemy { get => _TagNameEnemy; }
    /// <summary> 先頭プレイヤーの原点位置タグ名 </summary>
    public string TagNamePlayerBase { get => _TagNamePlayerBase; }
    /// <summary> 後尾プレイヤーの原点位置タグ名 </summary>
    public string TagNameAlliesBase { get => _TagNameAlliesBase; }
    /// <summary> 敵プレイヤーの原点位置タグ名 </summary>
    public string TagNameEnemyBase { get => _TagNameEnemyBase; }
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