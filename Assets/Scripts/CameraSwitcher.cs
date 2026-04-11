using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The Animator that drives the Cinemachine State-Driven Camera.")]
    [SerializeField] private Animator cameraAnimator;

    [Header("Buttons")]
    [SerializeField] private Button buttonPreviousCam;
    [SerializeField] private Button buttonNextCam;

    [Header("Animator Parameter")]
    [Tooltip("Name of the integer parameter in the Animator.")]
    [SerializeField] private string parameterName = "CameraState";

    [Header("Camera States")]
    [Tooltip("Total number of camera states.")]
    [SerializeField] private int totalCameraStates = 4;

    private void OnEnable()
    {
        buttonPreviousCam.onClick.AddListener(() => PreviousCamera());
        buttonNextCam.onClick.AddListener(() => NextCamera());
    }

    private void OnDisable()
    {
        buttonPreviousCam.onClick.RemoveAllListeners();
        buttonNextCam.onClick.RemoveAllListeners();
    }

    public void NextCamera()
    {
        int currentState = cameraAnimator.GetInteger(parameterName);
        int nextState = (currentState + 1) % totalCameraStates; // Wrap around for total camera states
        Debug.Log($"[CameraSwitcher] Switching to next camera state: {nextState}");
        cameraAnimator.SetInteger(parameterName, nextState);
    }

    public void PreviousCamera()
    {
        int currentState = cameraAnimator.GetInteger(parameterName);
        int previousState = (currentState - 1 + totalCameraStates) % totalCameraStates; // Wrap around for total camera states
        Debug.Log($"[CameraSwitcher] Switching to previous camera state: {previousState}");
        cameraAnimator.SetInteger(parameterName, previousState);
    }

}