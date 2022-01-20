using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�L�����N�^�[�̔\�͒l
/// </summary>
public class EnemyStatus : CharacterStatus
{
    /// <summary> ���̃L�����N�^�[�������_�����O���郌���_���[ </summary>
    Renderer _Renderer = default;

    void GetParameters()
    {
        //�f�[�^�e�[�u�����X�e�[�^�X�ꎮ���擾
        List<string> data = DataTableCharacter.I.GetDataUsingName(_Name);

        //ID�ԍ��擾
        _CharacterNumber = byte.Parse(data[1]);

        //�i�[
        _HPInitial = short.Parse(data[2]);
        _HPCurrent = _HPInitial;
        _Attack = short.Parse(data[3]);
        _Defense = short.Parse(data[4]);
        _Rapid = short.Parse(data[5]);
        _Technique = short.Parse(data[6]);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GetParameters();
        _Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary> �G�L�����N�^�[���|����鎞�̓��� </summary>
    protected override void DefeatProcess()
    {
        base.DefeatProcess();

        _Renderer.enabled = false;
    }
}
