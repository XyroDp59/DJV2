using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(SoundEmitter))]
public class PlayerController : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;

    public UnityEvent OnInteractEvent;
    private SoundEmitter soundEmitter;
    private Rigidbody rb;

    [Header("Camera Control")]
    [SerializeField] CinemachineVirtualCamera topDownCamera;
    [SerializeField] float cameraSpeed = 0.5f;
    CinemachineTrackedDolly trackedDollyCam;
    private float currentCameraMovement = 0;
    float topDownCameraHeight;

    [Header("Interaction")]
    [SerializeField] float interactionRange = 1.5f;
    [SerializeField] ParticleSystem throwIndicator;
    [SerializeField] float throwVelocity = 3f;
    private GrabbableObject currentGrabbedItem = null;
    private IInteractable nearActivable = null;

    [Header("Movement")]
    [SerializeField] float crouchSpeed = 1f;
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runningSpeed = 5f;
    private float currentSpeed;
    
    [SerializeField] float maxStamina = 2f;
    private bool isRunning = false;
    private float stamina = 0f;

    private bool hasClicked = false;
    private Vector3 clickedPos;
    private Vector2 oldMousePos = Vector2.zero;
    [SerializeField] private float followClickedDuration = 1f;
    [SerializeField] float mouseSensitivity = 0.75f;
    WaitForSeconds followClickedDelay;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        soundEmitter = GetComponent<SoundEmitter>();
        rb = GetComponent<Rigidbody>();
        topDownCameraHeight = topDownCamera.transform.position.y;
        followClickedDelay = new WaitForSeconds(followClickedDuration);
    }

    #region Input System
    private void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Move.performed += ctx =>
        {
            hasClicked = false;
            moveInput = ctx.ReadValue<Vector2>();
        };
        controls.Gameplay.Move.canceled += ctx =>
        {
            moveInput = Vector2.zero;
        };

        controls.Gameplay.Interact.performed += ctx => OnInteract();

        controls.Gameplay.Crouch.performed += ctx => currentSpeed = crouchSpeed;
        controls.Gameplay.Crouch.canceled += ctx => currentSpeed = walkSpeed;

        controls.Gameplay.Sprint.performed += ctx => isRunning = true;
        controls.Gameplay.Sprint.canceled += ctx => { currentSpeed = walkSpeed; isRunning = false; };
        currentSpeed = walkSpeed;

        controls.Gameplay.SwitchCamera.performed += ctx =>
        {
            topDownCamera.gameObject.SetActive(!topDownCamera.isActiveAndEnabled);
            Cursor.visible = topDownCamera.isActiveAndEnabled;
        };

        
        controls.Gameplay.SwitchPause.performed += ctx =>
        {
            if (!GameManager.Instance.CanBePaused()) return;
            if(Time.timeScale == 0) GameManager.Instance.ResumePause();
            else GameManager.Instance.Pause();
        };

        controls.Gameplay.Click.performed += ctx => { if (topDownCamera.isActiveAndEnabled) hasClicked = true; };
        controls.Gameplay.Click.canceled += ctx => hasClicked = false;
    }
    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
    #endregion

    #region Interactions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            GameManager.Instance.GameOver();
        }
    }

    private void OnInteract()
    {
        if (currentGrabbedItem != null)
        {
            Throw();
            return;
        }
        if (nearActivable == null) return;

        nearActivable.Interact(this);
        OnInteractEvent.Invoke();
    }

    public void SetInteractableObject(IInteractable i)
    {
        nearActivable = i;
    }

    public void SetItemGrabbed(GrabbableObject grabbableObject)
    {
        throwIndicator.gameObject.SetActive(!(grabbableObject == null));
        currentGrabbedItem = grabbableObject;
    }

    public void Throw()
    {
        currentGrabbedItem.GetRigidbody().useGravity = true;
        currentGrabbedItem.GetRigidbody().isKinematic = false;
        currentGrabbedItem.transform.parent = null;
        currentGrabbedItem.isThrown = true;
        currentGrabbedItem.GetRigidbody().velocity = (transform.forward + Vector3.up).normalized * throwVelocity;

        SetItemGrabbed(null);
    }
    #endregion

    #region Movement
    private void Update()
    {
        //------------ Movement and camera ------------//
        Vector3 movedir;
            // compute first person movement  
        if (!topDownCamera.isActiveAndEnabled && !hasClicked)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, (mousePos.x - oldMousePos.x) * mouseSensitivity, 0)));  
            oldMousePos = mousePos;
            movedir = (moveInput.x * transform.right + moveInput.y * transform.forward).normalized;
        }
        else
        {
            // compute top down movement using mouse
            if (moveInput.magnitude > 0)  hasClicked = false;
            if(hasClicked)
            {
                Debug.Log("aaaaaaaa");
                MoveTowardsClick();
            }
            // compute top down movement using WASD
            else
            {
                transform.LookAt(transform.position + new Vector3(moveInput.x, 0, moveInput.y));
            }
            movedir = transform.forward;
        }
        // Apply movement 
        if (moveInput == Vector2.zero && !hasClicked) rb.velocity = Vector3.zero;
        else rb.velocity = movedir * currentSpeed;

        topDownCamera.transform.position = rb.position + Vector3.up * topDownCameraHeight;

        // ------------ Play walking sounds ------------//
        if (rb.velocity != Vector3.zero) soundEmitter.PlaySound(currentSpeed, 0.3f);

        //------------ Stamina ------------//
        if (isRunning)
        {
            if (stamina <= 0f) return;
            currentSpeed = runningSpeed;
            stamina -= Time.deltaTime;
            if (stamina <= 0) { stamina = -maxStamina; isRunning = false; currentSpeed = walkSpeed; }
        }
        else
            stamina = Mathf.Clamp(stamina + Time.deltaTime, -maxStamina, maxStamina);
    }

    private void MoveTowardsClick()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        var plane = new Plane(Vector3.up, Vector3.zero);
        if (plane.Raycast(ray, out var distanceOnRay))
        {
            clickedPos = ray.GetPoint(distanceOnRay);
            clickedPos.y = transform.position.y;
            transform.LookAt(clickedPos);
        }
    }
    public float GetStamina()
    {
        return stamina / maxStamina;
    }
#endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactionRange);
        Gizmos.DrawLine(transform.position, clickedPos);
    }
}
