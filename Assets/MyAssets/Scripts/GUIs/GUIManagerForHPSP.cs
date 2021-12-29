using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GUI���HP��SP�\�������ۂ̃X�e�[�^�X�l�ʂ�ɔ��f����
/// </summary>
public class GUIManagerForHPSP : MonoBehaviour
{
    [SerializeField, Tooltip("�w�肵�����̃v���C���[���𔽉f����")]
    string _Name = "";

    [SerializeField, Tooltip("HP�����o�I�Ɋ����\�����邽�߂̉摜")]
    Image _HPGauge = default; 

    [SerializeField, Tooltip("HP�̐��l�������\������e�L�X�g")]
    Text _HPValue = default;

    [SerializeField, Tooltip("SP�����o�I�Ɋ����\�����邽�߂̉摜")]
    Image _SPGauge = default;

    [SerializeField, Tooltip("SP�̐��l�������\������e�L�X�g")]
    Text _SPValue = default;

    /// <summary> _Name�Ɏw�肵���L�����N�^�[�̃X�e�[�^�X </summary>
    PlayerStatus _status = default;

    /// <summary> �L�����N�^�[�̍ő�HP </summary>
    short _HPInitial => _status.HPInitial;

    /// <summary> �L�����N�^�[�̍���HP </summary>
    short _HPCurrent => _status.HPCurrent;
    /// <summary> �O�t���[���̃L�����N�^�[��HP </summary>
    short _BeforeHPCurrent = 0;

    /// <summary> �L�����N�^�[�̍ő�SP </summary>
    short _SPInitial => _status.SPInitial;

    /// <summary> �L�����N�^�[�̍���SP </summary>
    short _SPCurrent => _status.SPCurrent;
    /// <summary> �O�t���[���̃L�����N�^�[��SP </summary>
    short _BeforeSPCurrent = 0;

    // Start is called before the first frame update
    void Start()
    {
        //���O��_Name�̃L�����N�^�[�̃X�e�[�^�X���擾����
        _status = PlayerStatus.Players.Where(p => p.Name == _Name).First();
    }

    // Update is called once per frame
    void Update()
    {
        //���l�ɕω��������GUI���X�V
        if(_BeforeHPCurrent != _HPCurrent)
        {
            _HPGauge.fillAmount = _HPCurrent / (float)_HPInitial;
            _HPValue.text = _HPCurrent + " / " + _HPInitial;
            _BeforeHPCurrent = _HPCurrent;
        }
        if (_BeforeSPCurrent != _SPCurrent)
        {
            _SPGauge.fillAmount = _SPCurrent / (float)_SPInitial;
            _SPValue.text = _SPCurrent + " / " + _SPInitial;
            _BeforeSPCurrent = _SPCurrent;
        }
    }
}
