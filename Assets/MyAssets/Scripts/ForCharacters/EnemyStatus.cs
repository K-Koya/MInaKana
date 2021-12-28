using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�L�����N�^�[�̔\�͒l
/// </summary>
public class EnemyStatus : CharacterStatus
{
    void GetParameters()
    {
        //�f�[�^�e�[�u�����X�e�[�^�X�ꎮ���擾
        List<string> data = DataTableCharacter.I.GetDataUsingName(_Name);

        //�i�[
        _HPInitial = short.Parse(data[1]);
        _HPCurrent = _HPInitial;
        _Attack = short.Parse(data[3]);
        _Defense = short.Parse(data[4]);
        _Rapid = short.Parse(data[5]);
        _Technique = short.Parse(data[6]);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetParameters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
