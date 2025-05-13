using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.CinemachineBlendDefinition.Style;

[RequireComponent(typeof(SoundEmitter))]
public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] public UnityEvent OnInteract;
    protected SoundEmitter emitter;

    [SerializeField] GameObject button;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] float duration = 1f;
    Material material;
    CinemachineBrain cinemachineBrain;

    public void Interact(PlayerController player)
    {
        if (material.color == Color.red)
        {
            material.color = Color.green;
        }else
        {
            material.color = Color.red;
        }
        OnInteract.Invoke();
    }

    protected void Awake()
    {
        emitter = GetComponent<SoundEmitter>();
        material = button.GetComponent<Renderer>().material;
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void FocusOnActivation() { StartCoroutine(FocusOnActivationCoroutine());  }
    private IEnumerator FocusOnActivationCoroutine()
    {
        GameManager.Instance.canPause = false;
        virtualCamera.gameObject.SetActive(true);
        Time.timeScale = 0f;

        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(Cut, 0);  
        yield return new WaitForSecondsRealtime(duration);
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(EaseInOut, 0.5f);


        Time.timeScale = 1f;
        virtualCamera.gameObject.SetActive(false);
        GameManager.Instance.canPause = true;
    }
}
