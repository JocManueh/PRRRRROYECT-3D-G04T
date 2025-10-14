using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;              // El personaje a seguir

    [Header("Posición de la Cámara")]
    public Vector3 offset = new Vector3(0, 5, -7);
    public float suavizado = 5f;

    [Header("Rotación con Mouse")]
    public bool rotarConMouse = true;
    public float sensibilidadX = 2f;
    public float sensibilidadY = 2f;
    public float limiteVerticalMin = -30f;
    public float limiteVerticalMax = 60f;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    void Start()
    {
        if (objetivo == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                objetivo = player.transform;
        }

        // Calcular rotación inicial
        Vector3 angulos = transform.eulerAngles;
        rotacionX = angulos.y;
        rotacionY = angulos.x;

        // Ocultar cursor si se rota con mouse
        if (rotarConMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Rotación con mouse
        if (rotarConMouse)
        {
            rotacionX += Input.GetAxis("Mouse X") * sensibilidadX;
            rotacionY -= Input.GetAxis("Mouse Y") * sensibilidadY;
            rotacionY = Mathf.Clamp(rotacionY, limiteVerticalMin, limiteVerticalMax);

            // Desbloquear cursor con ESC
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            // Volver a bloquear con click
            if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // Calcular posición deseada
        Quaternion rotacion = Quaternion.Euler(rotacionY, rotacionX, 0);
        Vector3 posicionDeseada = objetivo.position + rotacion * offset;

        // Suavizar movimiento
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);

        // Mirar al objetivo
        transform.LookAt(objetivo.position + Vector3.up * 1.5f);
    }
}