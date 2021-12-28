using System.Linq;
using UnityEngine;

/// <summary>
/// �g�p�����^�O���̕�������Ǘ�
/// </summary>
public class TagNameManager : Singleton<TagNameManager>
{
    [Header("Add Tag�c �Őݒ肵���^�O���̕�������ȉ��ɓo�^����")]

    #region �����o
    [SerializeField, Tooltip("���C���J�����^�O��")]
    private string _buttonNameMainCamera = "MainCamera";

    [SerializeField, Tooltip("�擪�v���C���[�^�O��")]
    private string _buttonNamePlayer = "Player";

    [SerializeField, Tooltip("����v���C���[�^�O��")]
    private string _buttonNameAllies = "Allies";

    [SerializeField, Tooltip("�G�^�O��")]
    private string _buttonNameEnemy = "Enemy";
    #endregion

    #region �v���p�e�B
    /// <summary> ���C���J�����^�O�� </summary>
    public string ButtonNameMainCamera { get => _buttonNameMainCamera; }
    /// <summary> �擪�v���C���[�^�O�� </summary>
    public string ButtonNamePlayer { get => _buttonNamePlayer; }
    /// <summary> ����v���C���[�^�O�� </summary>
    public string ButtonNameAllies { get => _buttonNameAllies; }
    /// <summary> �G�^�O�� </summary>
    public string ButtonNameEnemy { get => _buttonNameEnemy; }
    #endregion

    protected override void Awake()
    {
        IsDontDestroyOnLoad = true;
        base.Awake();
    }
}

/// <summary>
/// ������tag�Ƃ̈�v��������郁�\�b�h��ǉ�
/// </summary>
static partial class Extensions
{
    /// <summary>
    /// CompareTag�̕�����r�o�[�W����
    /// </summary>
    /// <param name="obj">�Ώۂ̃Q�[���I�u�W�F�N�g</param>
    /// <param name="tags">��r�Ώۃ^�O</param>
    /// <returns>true : �����̃^�O�ƈ�ł���v����</returns>
    public static bool CompareTags(this GameObject obj, string[] tags)
    {
        return tags.Contains(obj.tag);
    }
}