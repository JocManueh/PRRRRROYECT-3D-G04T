using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float velocidadRotacion = 10f;

    [Header("Salto")]
    public float fuerzaSalto = 5f;
    public LayerMask capaSuelo;
    public Transform checkSuelo;
    public float radioCheckSuelo = 0.2f;

    private Rigidbody rb;
    private bool enSuelo;
    private Vector3 movimiento;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Configurar Rigidbody
        rb.freezeRotation = true; // Evitar que se voltee
    }

    void Update()
    {
        // Input de movimiento
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movimiento = new Vector3(horizontal, 0f, vertical).normalized;

        // Rotar el personaje en dirección del movimiento
        if (movimiento.magnitude > 0.1f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(movimiento);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
        }

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Verificar si está en el suelo
        enSuelo = Physics.CheckSphere(checkSuelo.position, radioCheckSuelo, capaSuelo);

        // Aplicar movimiento
        Vector3 velocidadObjetivo = movimiento * velocidad;
        velocidadObjetivo.y = rb.linearVelocity.y; // Mantener velocidad vertical
        rb.linearVelocity = velocidadObjetivo;
    }

    void OnDrawGizmosSelected()
    {
        if (checkSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checkSuelo.position, radioCheckSuelo);
        }
    }
}