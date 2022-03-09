using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// GUI上のHPとSP表示を実際のステータス値通りに反映する
/// </summary>
public class GUIManagerForHPSP : MonoBehaviour
{
    [SerializeField, Tooltip("_Nameに指定したキャラクターのステータス")]
    PlayerStatus _Status = default;

    [SerializeField, Tooltip("HPを視覚的に割合表示するための画像")]
    Image _HPGauge = default; 

    [SerializeField, Tooltip("HPの数値を実数表示するテキスト")]
    Text _HPValue = default;

    [SerializeField, Tooltip("SPを視覚的に割合表示するための画像")]
    Image _SPGauge = default;

    [SerializeField, Tooltip("SPの数値を実数表示するテキスト")]
    Text _SPValue = default;

    

    /// <summary> キャラクターの最大HP </summary>
    short _HPInitial => _Status.HPInitial;

    /// <summary> キャラクターの今のHP </summary>
    short _HPCurrent => _Status.HPCurrent;
    /// <summary> 前フレームのキャラクターのHP </summary>
    short _BeforeHPCurrent = 0;

    /// <summary> キャラクターの最大SP </summary>
    short _SPInitial => _Status.SPInitial;

    /// <summary> キャラクターの今のSP </summary>
    short _SPCurrent => _Status.SPCurrent;
    /// <summary> 前フレームのキャラクターのSP </summary>
    short _BeforeSPCurrent = 0;

    // Start is called before the first frame update
    void Start()
    {
        //HP表示初期化
        _HPGauge.fillAmount = _HPCurrent / (float)_HPInitial;
        _HPValue.text = _HPCurrent + " / " + _HPInitial;
        _BeforeHPCurrent = _HPCurrent;
        //SP表示初期化
        _SPGauge.fillAmount = _SPCurrent / (float)_SPInitial;
        _SPValue.text = _SPCurrent + " / " + _SPInitial;
        _BeforeSPCurrent = _SPCurrent;
    }

    // Update is called once per frame
    void Update()
    {
        if (_Status == null) return;

        //数値に変化があればGUIも更新
        if(_BeforeHPCurrent != _HPCurrent)
        {
            _HPGauge.DOFillAmount(_HPCurrent / (float)_HPInitial, 1f);
            _HPValue.DOText(_HPCurrent + " / " + _HPInitial, 1f);
            _BeforeHPCurrent = _HPCurrent;
        }
        if (_BeforeSPCurrent != _SPCurrent)
        {
            _SPGauge.DOFillAmount(_SPCurrent / (float)_SPInitial, 1f);
            _SPValue.DOText(_SPCurrent + " / " + _SPInitial, 1f);
            _BeforeSPCurrent = _SPCurrent;
        }
    }
}
