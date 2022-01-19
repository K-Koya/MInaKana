using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// �퓬�𐧌䂷��R���|�[�l���g
/// </summary>
public class BattleManager : MonoBehaviour
{
    /// <summary> �e�L�����N�^�[��RapidAccumulation�����̒l�ɒB����ƍs���ł��� </summary>
    const short TURN_BORDER = 500;

    /// <summary> �e�퓬���̃L�����N�^�[�̃X�e�[�^�X�ւ̃A�N�Z�b�T </summary>
    CharacterStatus[] _BattleCharacters = default;

    /// <summary> ���݂̍s���҂̃X�e�[�^�X </summary>
    CharacterStatus _TurnOwner = default;

    // Start is called before the first frame update
    void Start()
    {
        //�퓬�ɎQ�����̃L�����N�^�[�S���̃X�e�[�^�X���擾
        _BattleCharacters = FindObjectsOfType<CharacterStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_TurnOwner || !_TurnOwner.IsMyTurn) TurnInstructer();
    }

    /// <summary>
    /// ���̍s���҂������Ďw������
    /// </summary>
    void TurnInstructer()
    {
        //���̃^�[���̍s���҂�ۊ�
        _TurnOwner = _BattleCharacters.Where(b => !(b.RapidAccumulation < TURN_BORDER)).FirstOrDefault();

        //�N����RapidAccumulation��TURN_BORDER�𒴂���܂ŁA�S���q���l�����Z
        while (!_TurnOwner)
        {
            Array.ForEach(_BattleCharacters, b => b.RapidAccumulation += b.Rapid);
            _TurnOwner = _BattleCharacters.OrderByDescending(b => b.RapidAccumulation).Where(b => !(b.RapidAccumulation < TURN_BORDER)).FirstOrDefault();
        }

        //���Ȃ��̃^�[���ł�
        _TurnOwner.IsMyTurn = true;
        _TurnOwner.RapidAccumulation -= TURN_BORDER;

        Debug.Log(_TurnOwner.Name + " �̃^�[���ł��B");
    }
}
