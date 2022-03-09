using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// GUI���HP��SP�\�������ۂ̃X�e�[�^�X�l�ʂ�ɔ��f����
/// </summary>
public class GUIManagerForHPSP : MonoBehaviour
{
    [SerializeField, Tooltip("_Name�Ɏw�肵���L�����N�^�[�̃X�e�[�^�X")]
    PlayerStatus _Status = default;

    [SerializeField, Tooltip("HP�����o�I�Ɋ����\�����邽�߂̉摜")]
    Image _HPGauge = default; 

    [SerializeField, Tooltip("HP�̐��l�������\������e�L�X�g")]
    Text _HPValue = default;

    [SerializeField, Tooltip("SP�����o�I�Ɋ����\�����邽�߂̉摜")]
    Image _SPGauge = default;

    [SerializeField, Tooltip("SP�̐��l�������\������e�L�X�g")]
    Text _SPValue = default;

    

    /// <summary> �L�����N�^�[�̍ő�HP </summary>
    short _HPInitial => _Status.HPInitial;

    /// <summary> �L�����N�^�[�̍���HP </summary>
    short _HPCurrent => _Status.HPCurrent;
    /// <summary> �O�t���[���̃L�����N�^�[��HP </summary>
    short _BeforeHPCurrent = 0;

    /// <summary> �L�����N�^�[�̍ő�SP </summary>
    short _SPInitial => _Status.SPInitial;

    /// <summary> �L�����N�^�[�̍���SP </summary>
    short _SPCurrent => _Status.SPCurrent;
    /// <summary> �O�t���[���̃L�����N�^�[��SP </summary>
    short _BeforeSPCurrent = 0;

    // Start is called before the first frame update
    void Start()
    {
        //HP�\��������
        _HPGauge.fillAmount = _HPCurrent / (float)_HPInitial;
        _HPValue.text = _HPCurrent + " / " + _HPInitial;
        _BeforeHPCurrent = _HPCurrent;
        //SP�\��������
        _SPGauge.fillAmount = _SPCurrent / (float)_SPInitial;
        _SPValue.text = _SPCurrent + " / " + _SPInitial;
        _BeforeSPCurrent = _SPCurrent;
    }

    // Update is called once per frame
    void Update()
    {
        if (_Status == null) return;

        //���l�ɕω��������GUI���X�V
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
