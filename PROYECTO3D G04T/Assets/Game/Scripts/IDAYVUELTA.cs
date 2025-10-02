using UnityEngine;

public class MoverCuboIdaVuelta : MonoBehaviour
{
    public Vector3 puntoInicial;
    public Vector3 puntoFinal;
    public float velocidad = 2f;
    private bool yendo = true; // True = yendo al final, False = volviendo

    void Start()
    {
        puntoInicial = transform.position;
    }

    void Update()
    {
        if (yendo)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoFinal, velocidad * Time.deltaTime);

            if (transform.position == puntoFinal)
                yendo = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoInicial, velocidad * Time.deltaTime);

            if (transform.position == puntoInicial)
                yendo = true;
        }
    }
}
