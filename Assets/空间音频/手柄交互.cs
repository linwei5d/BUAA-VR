using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
[RequireComponent(typeof(扬声器管理))]
public class 手柄交互 : MonoBehaviour
{
    public 扬声器管理 SpeakerManager;
    public XRRayInteractor xrRayInteractor; // XRRayInteractor组件的引用
    public InputActionReference correctTrigger, wrongTrigger;
    void Start() {
        correctTrigger.action.performed += SpeakerCorrectState;
        wrongTrigger.action.performed += SpeakerWrongState;
    }
    void SpeakerCorrectState(InputAction.CallbackContext _) {
        // 尝试获取射线击中的信息
        RaycastHit hitInfo;
        if (xrRayInteractor.TryGetCurrent3DRaycastHit(out hitInfo))
        {
            // 如果射线击中了物体，hitInfo会包含相关信息
            GameObject hitObject = hitInfo.collider.gameObject; // 获取射线击中的GameObject
            SpeakerManager.ChangSpeakerState(hitObject, 扬声器管理.CORRECT);
            Debug.Log("Ray interactor hit object: " + hitObject.name);
        }
    }
    void SpeakerWrongState(InputAction.CallbackContext _) {
        // 尝试获取射线击中的信息
        RaycastHit hitInfo;
        if (xrRayInteractor.TryGetCurrent3DRaycastHit(out hitInfo))
        {
            // 如果射线击中了物体，hitInfo会包含相关信息
            GameObject hitObject = hitInfo.collider.gameObject; // 获取射线击中的GameObject
            SpeakerManager.ChangSpeakerState(hitObject, 扬声器管理.WRONG);
            Debug.Log("Ray interactor hit object: " + hitObject.name);
        }
    }
    void Update()
    {
        // 尝试获取射线击中的信息
        RaycastHit hitInfo;
        if (xrRayInteractor.TryGetCurrent3DRaycastHit(out hitInfo))
        {
            // 如果射线击中了物体，hitInfo会包含相关信息
            GameObject hitObject = hitInfo.collider.gameObject; // 获取射线击中的GameObject
            SpeakerManager.ChangSpeakerState(hitObject, 扬声器管理.HILIGHTED);
            Debug.Log("Ray interactor hit object: " + hitObject.name);
        }
    }
}
