using UnityEngine;

public class MoverCuboPingPong : MonoBehaviour
{
    public Vector3 puntoInicial;   // Posición de inicio
    public Vector3 puntoFinal;     // Posición a donde debe llegar
    public float velocidad = 2f;   // Velocidad del movimiento

    void Start()
    {
        // Guardar el punto inicial (posición actual del cubo)
        puntoInicial = transform.position;
    }

    void Update()
    {
        // Factor de movimiento que oscila entre 0 y 1
        float t = Mathf.PingPong(Time.time * velocidad, 1f);

        // Mover entre puntoInicial y puntoFinal
        transform.position = Vector3.Lerp(puntoInicial, puntoFinal, t);
    }
}
