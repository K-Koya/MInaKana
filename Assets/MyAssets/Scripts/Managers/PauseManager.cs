using UnityEngine;

/// <summary>
/// �|�[�Y���������{
/// </summary>
public class PauseManager : Singleton<PauseManager>
{
    [SerializeField, Tooltip("�|�[�Y���j���[�{�^����")]
    private string _pauseButtonName = "Cancel";

    /// <summary>
    /// IPauseAndResume��OnPause���`����
    /// </summary>
    public delegate void OnPause();
    /// <summary>
    /// IPauseAndResume��OnResume���`����
    /// </summary>
    public delegate void OnResume();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/// <summary>
/// �|�[�Y�����Ώۂɂ���R���|�[�l���g�Ɍp��������
/// </summary>
public interface IPauseAndResume
{
    /// <summary>
    /// �|�[�Y�ɂȂ����u�ԂɎ��{���鏈��
    /// </summary>
    public void OnPause();

    /// <summary>
    /// �|�[�Y�����Ɠ����Ɏ��{���鏈��
    /// </summary>
    public void OnResume();
}
