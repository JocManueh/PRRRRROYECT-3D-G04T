using UnityEngine;

public class MoverCubo : MonoBehaviour
{
    public float velocidad = 2f; // Velocidad de movimiento

    void Update()
    {
        // Mueve el cubo constantemente en el eje X
        transform.Translate(Vector3.right * velocidad * Time.deltaTime);
    }
}