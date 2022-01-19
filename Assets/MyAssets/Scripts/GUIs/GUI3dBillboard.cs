using UnityEngine;

/// <summary>
/// 3次元空間中のGUIをカメラの方向へ向ける
/// </summary>
public class GUI3dBillboard : MonoBehaviour {

    /// <summary> カメラオブジェクト </summary>
    GameObject mainCameraObj = default;


    // Use this for initialization
    void Start () {

        mainCameraObj = GameObject.FindWithTag(TagNameManager.I.TagNameMainCamera);
    }
	
	// Update is called once per frame
	void Update () {

        transform.rotation = mainCameraObj.transform.rotation;
	}
}
