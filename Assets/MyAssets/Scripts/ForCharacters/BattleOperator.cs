using UnityEngine;

/// <summary>
/// ���I�u�W�F�N�g�̃L�����N�^�[�̐퓬���̑�����󂯕t����
/// </summary>
[RequireComponent(typeof(CharacterStatus))]
public abstract class BattleOperator : MonoBehaviour
{
    /// <summary> �퓬���̏����ʒu </summary>
    protected Vector3 _BasePosition = default;

    /// <summary> �L�����N�^�[�̔\�͒l </summary>
    protected CharacterStatus _Status = default;

    /// <summary> ���s���̃R�}���h�̗��� </summary>
    protected Coroutine _RunningCommand = default;

    /// <summary> �퓬���̏����ʒu </summary>
    public Vector3 BasePosition { get => _BasePosition; set => _BasePosition = value; }

    protected virtual void Awake()
    {
        _Status = GetComponent<CharacterStatus>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //�����ʒu�����_��
        _BasePosition = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_Status.IsMyTurn)
        {
            OperateCommand();
        }
    }

    /// <summary>
    /// �s�������{
    /// </summary>
    /// <returns> true : �s���I�� </returns>
    protected abstract void OperateCommand();
}
