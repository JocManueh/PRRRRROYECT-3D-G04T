using UnityEngine;

public class CubeRotationButtons : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    [Tooltip("Velocidad de rotación en grados por segundo")]
    public float velocidadRotacion = 90f;

    // Estados de rotación para cada eje
    private bool rotandoX = false;
    private bool rotandoY = false;
    private bool rotandoZ = false;

    void Update()
    {
        // Rotación continua en X si está activa
        if (rotandoX)
        {
            Quaternion rotacionX = Quaternion.AngleAxis(velocidadRotacion * Time.deltaTime, Vector3.right);
            transform.rotation = rotacionX * transform.rotation;
        }

        // Rotación continua en Y si está activa
        if (rotandoY)
        {
            Quaternion rotacionY = Quaternion.AngleAxis(velocidadRotacion * Time.deltaTime, Vector3.up);
            transform.rotation = rotacionY * transform.rotation;
        }

        // Rotación continua en Z si está activa
        if (rotandoZ)
        {
            Quaternion rotacionZ = Quaternion.AngleAxis(velocidadRotacion * Time.deltaTime, Vector3.forward);
            transform.rotation = rotacionZ * transform.rotation;
        }
    }

    // Métodos públicos para ser llamados desde los botones UI

    public void ToggleRotacionX()
    {
        rotandoX = !rotandoX;
        Debug.Log($"Rotación en X: {(rotandoX ? "ACTIVADA" : "DESACTIVADA")}");
    }

    public void ToggleRotacionY()
    {
        rotandoY = !rotandoY;
        Debug.Log($"Rotación en Y: {(rotandoY ? "ACTIVADA" : "DESACTIVADA")}");
    }

    public void ToggleRotacionZ()
    {
        rotandoZ = !rotandoZ;
        Debug.Log($"Rotación en Z: {(rotandoZ ? "ACTIVADA" : "DESACTIVADA")}");
    }

    // Métodos alternativos si quieres activar/desactivar directamente

    public void ActivarRotacionX(bool activar)
    {
        rotandoX = activar;
    }

    public void ActivarRotacionY(bool activar)
    {
        rotandoY = activar;
    }

    public void ActivarRotacionZ(bool activar)
    {
        rotandoZ = activar;
    }

    // Método para detener todas las rotaciones
    public void DetenerTodasLasRotaciones()
    {
        rotandoX = false;
        rotandoY = false;
        rotandoZ = false;
        Debug.Log("Todas las rotaciones detenidas");
    }
}