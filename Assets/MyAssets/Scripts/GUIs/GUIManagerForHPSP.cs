using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// GUI上のHPとSP表示を実際のステータス値通りに反映する
/// </summary>
public class GUIManagerForHPSP : MonoBehaviour
{
    [SerializeField, Tooltip("指定した方のプレイヤー情報を反映する")]
    string _Name = "";

    [SerializeField, Tooltip("HPを視覚的に割合表示するための画像")]
    Image _HPGauge = default; 

    [SerializeField, Tooltip("HPの数値を実数表示するテキスト")]
    Text _HPValue = default;

    [SerializeField, Tooltip("SPを視覚的に割合表示するための画像")]
    Image _SPGauge = default;

    [SerializeField, Tooltip("SPの数値を実数表示するテキスト")]
    Text _SPValue = default;

    /// <summary> _Nameに指定したキャラクターのステータス </summary>
    PlayerStatus _status = default;

    /// <summary> キャラクターの最大HP </summary>
    short _HPInitial => _status.HPInitial;

    /// <summary> キャラクターの今のHP </summary>
    short _HPCurrent => _status.HPCurrent;
    /// <summary> 前フレームのキャラクターのHP </summary>
    short _BeforeHPCurrent = 0;

    /// <summary> キャラクターの最大SP </summary>
    short _SPInitial => _status.SPInitial;

    /// <summary> キャラクターの今のSP </summary>
    short _SPCurrent => _status.SPCurrent;
    /// <summary> 前フレームのキャラクターのSP </summary>
    short _BeforeSPCurrent = 0;

    // Start is called before the first frame update
    void Start()
    {
        //名前が_Nameのキャラクターのステータスを取得する
        _status = PlayerStatus.Players.Where(p => p.Name == _Name).First();
    }

    // Update is called once per frame
    void Update()
    {
        //数値に変化があればGUIも更新
        if(_BeforeHPCurrent != _HPCurrent)
        {
            _HPGauge.DOFillAmount(_HPCurrent / (float)_HPInitial, 0.2f);
            _HPValue.DOText(_HPCurrent + " / " + _HPInitial, 0.2f);
            _BeforeHPCurrent = _HPCurrent;
        }
        if (_BeforeSPCurrent != _SPCurrent)
        {
            _SPGauge.DOFillAmount(_SPCurrent / (float)_SPInitial, 0.2f);
            _SPValue.DOText(_SPCurrent + " / " + _SPInitial, 0.2f);
            _BeforeSPCurrent = _SPCurrent;
        }
    }
}
