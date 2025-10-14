using UnityEngine;

public class ObjetoCapurable : MonoBehaviour
{
    [Header("Configuración")]
    public int puntos = 10;                 // Puntos al capturar
    public GameObject efectoDestruccion;    // Efecto visual al ser destruido

    [Header("Animación Visual")]
    public bool flotarEnAire = true;
    public float alturaFlotacion = 0.5f;
    public float velocidadFlotacion = 2f;

    [Header("Rotación")]
    public bool rotarConstantemente = true;
    public Vector3 velocidadRotacion = new Vector3(0, 50, 0);

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;

        // Asegurarse de tener Collider
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
            Debug.LogWarning($"{gameObject.name} no tenía Collider. Se agregó BoxCollider automáticamente.");
        }
    }

    void Update()
    {
        // Efecto de flotación
        if (flotarEnAire)
        {
            float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion;
            transform.position = new Vector3(transform.position.x, nuevaY, transform.position.z);
        }

        // Rotación constante
        if (rotarConstantemente)
        {
            transform.Rotate(velocidadRotacion * Time.deltaTime);
        }
    }

    // Método llamado al ser capturado
    void OnDestroy()
    {
        // Crear efecto de destrucción
        if (efectoDestruccion != null)
        {
            Instantiate(efectoDestruccion, transform.position, Quaternion.identity);
        }


    }
}