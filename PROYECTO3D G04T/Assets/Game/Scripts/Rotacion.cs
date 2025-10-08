using UnityEngine;

public class CubeRotationButtons : MonoBehaviour
{
    [Header("Configuraci�n de Rotaci�n")]
    [Tooltip("Velocidad de rotaci�n en grados por segundo")]
    public float velocidadRotacion = 90f;

    // Estados de rotaci�n para cada eje
    private bool rotandoX = false;
    private bool rotandoY = false;
    private bool rotandoZ = false;

    void Update()
    {
        // Rotaci�n continua en X si est� activa
        if (rotandoX)
        {
            Quaternion rotacionX = Quaternion.AngleAxis(velocidadRotacion * Time.deltaTime, Vector3.right);
            transform.rotation = rotacionX * transform.rotation;
        }

        // Rotaci�n continua en Y si est� activa
        if (rotandoY)
        {
            Quaternion rotacionY = Quaternion.AngleAxis(velocidadRotacion * Time.deltaTime, Vector3.up);
            transform.rotation = rotacionY * transform.rotation;
        }

        // Rotaci�n continua en Z si est� activa
        if (rotandoZ)
        {
            Quaternion rotacionZ = Quaternion.AngleAxis(velocidadRotacion * Time.deltaTime, Vector3.forward);
            transform.rotation = rotacionZ * transform.rotation;
        }
    }

    // M�todos p�blicos para ser llamados desde los botones UI

    public void ToggleRotacionX()
    {
        rotandoX = !rotandoX;
        Debug.Log($"Rotaci�n en X: {(rotandoX ? "ACTIVADA" : "DESACTIVADA")}");
    }

    public void ToggleRotacionY()
    {
        rotandoY = !rotandoY;
        Debug.Log($"Rotaci�n en Y: {(rotandoY ? "ACTIVADA" : "DESACTIVADA")}");
    }

    public void ToggleRotacionZ()
    {
        rotandoZ = !rotandoZ;
        Debug.Log($"Rotaci�n en Z: {(rotandoZ ? "ACTIVADA" : "DESACTIVADA")}");
    }

    // M�todos alternativos si quieres activar/desactivar directamente

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

    // M�todo para detener todas las rotaciones
    public void DetenerTodasLasRotaciones()
    {
        rotandoX = false;
        rotandoY = false;
        rotandoZ = false;
        Debug.Log("Todas las rotaciones detenidas");
    }
}