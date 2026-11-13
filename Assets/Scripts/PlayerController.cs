using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference look;

    [Header("Ajustes")]
    [SerializeField] Transform camera;
    [SerializeField] float speed = 4f;
    [SerializeField] float sensibility = 0.1f;

    CharacterController charController;
    float pitch, velY;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        move.action.Enable();
        look.action.Enable();
    }

    void OnDisable()
    {
        move.action.Disable();
        look.action.Disable();
    }

    void Update()
    {
        Vector2 lookMovement = look.action.ReadValue<Vector2>() * sensibility;
        transform.Rotate(0, lookMovement.x, 0);
        pitch = Mathf.Clamp(pitch - lookMovement.y, -85f, 85f);
        camera.localEulerAngles = new Vector3(pitch, 0, 0);

        Vector2 movement = move.action.ReadValue<Vector2>();
        Vector3 dir = transform.right * movement.x + transform.forward * movement.y;

        charController.Move((dir * speed) * Time.deltaTime);
    }

}
