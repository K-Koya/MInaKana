using UnityEngine;

/// <summary>
/// �V���O���g����������R���|�[�l���g�̊��N���X
/// </summary>
/// <typeparam name="T">MonoBehaviour���p������iInspector��ɏo�������j�R���|�[�l���g</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// ture : DontDestroyOnLoad�̑Ώۂɂ���
    /// </summary>
    private bool _isDontDestroyOnLoad = false;

    /// <summary>
    /// Inspector��ɏo�Ă���V���O���g���̃R���|�[�l���g�C���X�^���X
    /// </summary>
    private static T _I = default;


    /* �v���p�e�B */
    /// <summary>
    /// Inspector��ɏo�Ă���V���O���g���̃R���|�[�l���g�C���X�^���X
    /// </summary>
    public static T I
    {
        get
        {
            //�Ώۂ̃V���O���g���R���|�[�l���g���o�^����ĂȂ���΁A���݂̃V�[������E���Ă���
            if (!_I)
            {
                _I = FindObjectOfType<T>();
                if (!_I) Debug.LogError("�V���O���g���R���|�[�l���g�� " + typeof(T) + " ���A���݂̃V�[���ɑ��݂��܂���I");
            }
            return _I;
        }
    }

    /// <summary>
    /// ture : DontDestroyOnLoad�̑Ώۂɂ���
    /// </summary>
    public bool IsDontDestroyOnLoad { get => _isDontDestroyOnLoad; set => _isDontDestroyOnLoad = value; }


    protected virtual void Awake()
    {
        //DontDestroyOnLoad�ɓo�^���Ȃ��R���|�[�l���g�Ȃ痣�E
        if (!_isDontDestroyOnLoad) return;

        //�o�^����Ă���V���O���g���R���|�[�l���g�������̃C���X�^���X�Ɠ����Ȃ�ADontDestroyOnLoad�ɓo�^����
        //�قȂ�΁A������j������
        if (_I && this != _I)  Destroy(this.gameObject);
        else DontDestroyOnLoad(this.gameObject);
    }
}
