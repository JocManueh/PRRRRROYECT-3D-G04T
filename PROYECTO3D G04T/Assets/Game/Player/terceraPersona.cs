using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;              // El personaje a seguir

    [Header("Posición de la Cámara")]
    public Vector3 offset = new Vector3(0, 5, -7);
    public float suavizado = 5f;
    public float alturaMinima = 2f;         // Altura mínima de la cámara

    [Header("Seguimiento")]
    public bool seguirRotacion = true;      // Seguir rotación del personaje
    public float suavizadoRotacion = 3f;

    void Start()
    {
        if (objetivo == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                objetivo = player.transform;
        }

        // Cursor siempre visible y desbloqueado
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Calcular posición deseada
        Quaternion rotacion = seguirRotacion ? objetivo.rotation : Quaternion.identity;
        Vector3 posicionDeseada = objetivo.position + rotacion * offset;

        // Asegurar altura mínima
        if (posicionDeseada.y < objetivo.position.y + alturaMinima)
        {
            posicionDeseada.y = objetivo.position.y + alturaMinima;
        }

        // Suavizar movimiento
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);

        // Mirar al objetivo suavemente
        Vector3 direccion = objetivo.position - transform.position;
        Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, suavizadoRotacion * Time.deltaTime);
    }
}