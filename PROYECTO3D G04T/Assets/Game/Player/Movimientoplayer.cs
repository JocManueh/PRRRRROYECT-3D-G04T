using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Salto")]
    public float fuerzaSalto = 5f;
    public LayerMask capaSuelo;
    public Transform checkSuelo;
    public float radioCheckSuelo = 0.2f;

    private Rigidbody rb;
    private bool enSuelo;

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
        // Solo movimiento hacia ADELANTE con W
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
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
        if (checkSuelo != null)
        {
            enSuelo = Physics.CheckSphere(checkSuelo.position, radioCheckSuelo, capaSuelo);
        }
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