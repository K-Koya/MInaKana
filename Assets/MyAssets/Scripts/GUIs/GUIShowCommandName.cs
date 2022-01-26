using UnityEngine;
using UnityEngine.UI;

public class GUIShowCommandName : MonoBehaviour
{
    [SerializeField, Tooltip("コマンド名")]
    Text _Name = default;

    [SerializeField, Tooltip("コマンド消費SPもしくはアイテム残数")]
    Text _Value = default;

    /// <summary> リスト一行に表示 </summary>
    /// <param name="cName"> コマンド名 </param>
    /// <param name="value"> コマンド消費SPもしくはアイテム残数 </param>
    public void Show(string cName, string value)
    {
        _Name.text = cName;
        _Value.text = value;
    }
}
