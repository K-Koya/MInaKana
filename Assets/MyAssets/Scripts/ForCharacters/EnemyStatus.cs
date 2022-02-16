using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�L�����N�^�[�̔\�͒l
/// </summary>
public class EnemyStatus : CharacterStatus
{
    [SerializeField, Tooltip("�|���ꂽ���ɔ�������p�[�e�B�N��")]
    GameObject _DefeatEffect = default;

    void GetParameters()
    {
        //�f�[�^�e�[�u�����X�e�[�^�X�ꎮ���擾
        List<string> data = DataTableCharacter.I.GetDataUsingName(_Name);

        //ID�ԍ��擾
        _CharacterNumber = byte.Parse(data[1]);

        //�i�[
        _HPInitial = short.Parse(data[2]);
        _HPCurrent = _HPInitial;
        _Attack = short.Parse(data[4]);
        _Defense = short.Parse(data[5]);
        _Rapid = short.Parse(data[6]);
        _Technique = short.Parse(data[7]);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GetParameters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary> �G�L�����N�^�[���|����鎞�̓��� </summary>
    protected override void DefeatProcess()
    {
        base.DefeatProcess();

        GameObject go = Instantiate(_DefeatEffect);
        go.transform.position = this.transform.position;
        Destroy(go, 2f);

        Array.ForEach(GetComponentsInChildren<Renderer>(), r => r.enabled = false);
    }
}
