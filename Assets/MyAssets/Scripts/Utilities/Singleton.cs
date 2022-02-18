using UnityEngine;

/// <summary>
/// シングルトン化させるコンポーネントの基底クラス
/// </summary>
/// <typeparam name="T">MonoBehaviourを継承する（Inspector上に出したい）コンポーネント</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("シングルトンオブジェクトである")]
    [SerializeField, Tooltip("ture : DontDestroyOnLoadの対象にする")]
    bool _isDontDestroyOnLoad = false;

    [Space]

    /// <summary>
    /// Inspector上に出ているシングルトンのコンポーネントインスタンス
    /// </summary>
    static T _I;


    /* プロパティ */
    /// <summary>
    /// Inspector上に出ているシングルトンのコンポーネントインスタンス
    /// </summary>
    public static T I
    {
        get
        {
            //対象のシングルトンコンポーネントが登録されてなければ、現在のシーンから拾ってくる
            if (!_I)
            {
                _I = (T)FindObjectOfType(typeof(T));
                if (!_I) Debug.LogError("シングルトンコンポーネントの " + typeof(T) + " が、現在のシーンに存在しません！");
            }
            return _I;
        }
    }

    /// <summary>
    /// ture : DontDestroyOnLoadの対象にする
    /// </summary>
    public bool IsDontDestroyOnLoad { get => _isDontDestroyOnLoad; set => _isDontDestroyOnLoad = value; }


    protected virtual void Awake()
    {
        //DontDestroyOnLoadに登録しないコンポーネントなら離脱
        if (!_isDontDestroyOnLoad) return;

        //登録されているシングルトンコンポーネントが自分のインスタンスと同じなら、DontDestroyOnLoadに登録する
        //異なれば、自分を破棄する
        if (this != I) Destroy(this.gameObject);
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
