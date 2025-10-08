using UnityEngine;

public class CubeScaleAnimation : MonoBehaviour
{
    [Header("Configuración de Escala")]
    [Tooltip("Escala mínima del cubo")]
    public float escalaMinima = 0.5f;

    [Tooltip("Escala máxima del cubo")]
    public float escalaMaxima = 2.0f;

    [Tooltip("Velocidad de la animación (menor = más lento)")]
    public float velocidad = 1.0f;

    private Vector3 escalaInicial;
    private float tiempo = 0f;

    void Start()
    {
        // Guardamos la escala inicial del objeto
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        // Incrementamos el tiempo
        tiempo += Time.deltaTime * velocidad;

        // Usamos una función seno para crear el movimiento de ida y vuelta
        // El resultado oscila entre -1 y 1
        float factor = Mathf.Sin(tiempo);

        // Mapeamos el valor de -1,1 al rango de escalaMinima a escalaMaxima
        float escalaActual = Mathf.Lerp(escalaMinima, escalaMaxima, (factor + 1f) / 2f);

        // Aplicamos la nueva escala manteniendo las proporciones
        transform.localScale = escalaInicial * escalaActual;
    }
}