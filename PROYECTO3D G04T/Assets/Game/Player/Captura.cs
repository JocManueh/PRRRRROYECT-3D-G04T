using UnityEngine;
using TMPro;

public class RayCapture : MonoBehaviour
{
    [Header("Configuración del Rayo")]
    public LineRenderer lineRenderer;
    public Transform puntoOrigen;           // Desde donde sale el rayo (mano del personaje)
    public float distanciaMaxima = 15f;     // Distancia máxima del rayo
    public float distanciaCaptura = 5f;     // Distancia para poder capturar
    public LayerMask capasCapturables;      // Qué objetos se pueden capturar

    [Header("Detección")]
    public Color colorRayoNormal = Color.cyan;
    public Color colorRayoDetectando = Color.yellow;
    public Color colorRayoCapturando = Color.red;
    public Color colorRayoMuyLejos = Color.gray;

    [Header("Efectos")]
    public GameObject efectoCaptura;        // Partículas al capturar
    public AudioClip sonidoDisparo;
    public AudioClip sonidoCaptura;
    public AudioClip sonidoMuyLejos;        // Sonido cuando está muy lejos

    [Header("UI")]
    public TextMeshProUGUI textoDistancia;  // Muestra la distancia al objeto
    public GameObject indicadorCaptura;     // Icono sobre objeto capturable

    private GameObject objetoApuntado;
    private AudioSource audioSource;
    private bool rayoActivo = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Configurar LineRenderer
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = colorRayoNormal;
        lineRenderer.endColor = colorRayoNormal;
        lineRenderer.positionCount = 2;

        // Configurar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Ocultar UI
        if (textoDistancia != null)
            textoDistancia.gameObject.SetActive(false);

        if (indicadorCaptura != null)
            indicadorCaptura.SetActive(false);
    }

    void Update()
    {
        // Activar/desactivar rayo con botón derecho del mouse
        if (Input.GetMouseButtonDown(1))
        {
            ActivarRayo();
        }

        if (Input.GetMouseButtonUp(1))
        {
            DesactivarRayo();
        }

        // Si el rayo está activo
        if (rayoActivo)
        {
            DetectarObjetoConRaycast();

            // Capturar con clic izquierdo
            if (Input.GetMouseButtonDown(0))
            {
                IntentarCapturar();
            }
        }
    }

    void ActivarRayo()
    {
        rayoActivo = true;
        if (lineRenderer != null)
            lineRenderer.enabled = true;

        if (textoDistancia != null)
            textoDistancia.gameObject.SetActive(true);

        if (sonidoDisparo != null && audioSource != null)
            audioSource.PlayOneShot(sonidoDisparo);

        Debug.Log("Rayo activado");
    }

    void DesactivarRayo()
    {
        rayoActivo = false;
        if (lineRenderer != null)
            lineRenderer.enabled = false;

        if (textoDistancia != null)
            textoDistancia.gameObject.SetActive(false);

        if (indicadorCaptura != null)
            indicadorCaptura.SetActive(false);

        objetoApuntado = null;
        Debug.Log("Rayo desactivado");
    }

    void DetectarObjetoConRaycast()
    {
        if (puntoOrigen == null || mainCamera == null) return;

        // Crear raycast desde el punto de origen hacia adelante
        Ray ray = new Ray(puntoOrigen.position, puntoOrigen.forward);
        RaycastHit hit;

        // Dibujar el rayo
        Vector3 puntoFinal = puntoOrigen.position + puntoOrigen.forward * distanciaMaxima;

        if (Physics.Raycast(ray, out hit, distanciaMaxima, capasCapturables))
        {
            // Golpeó un objeto capturable
            puntoFinal = hit.point;
            objetoApuntado = hit.collider.gameObject;

            float distancia = Vector3.Distance(puntoOrigen.position, hit.point);

            // Actualizar UI de distancia
            if (textoDistancia != null)
            {
                textoDistancia.text = $"Distancia: {distancia:F1}m";

                if (distancia <= distanciaCaptura)
                {
                    textoDistancia.text += "\n[Click Izquierdo para Capturar]";
                    textoDistancia.color = Color.green;
                }
                else
                {
                    textoDistancia.text += "\n[Muy lejos]";
                    textoDistancia.color = Color.red;
                }
            }

            // Cambiar color según distancia
            if (distancia <= distanciaCaptura)
            {
                lineRenderer.startColor = colorRayoDetectando;
                lineRenderer.endColor = colorRayoDetectando;

                // Mostrar indicador sobre el objeto
                if (indicadorCaptura != null)
                {
                    indicadorCaptura.SetActive(true);
                    indicadorCaptura.transform.position = hit.point + Vector3.up * 2f;
                    indicadorCaptura.transform.LookAt(mainCamera.transform);
                }
            }
            else
            {
                lineRenderer.startColor = colorRayoMuyLejos;
                lineRenderer.endColor = colorRayoMuyLejos;

                if (indicadorCaptura != null)
                    indicadorCaptura.SetActive(false);
            }
        }
        else
        {
            // No golpeó nada
            objetoApuntado = null;
            lineRenderer.startColor = colorRayoNormal;
            lineRenderer.endColor = colorRayoNormal;

            if (textoDistancia != null)
            {
                textoDistancia.text = "Sin objetivo";
                textoDistancia.color = Color.white;
            }

            if (indicadorCaptura != null)
                indicadorCaptura.SetActive(false);
        }

        // Actualizar posiciones del LineRenderer
        lineRenderer.SetPosition(0, puntoOrigen.position);
        lineRenderer.SetPosition(1, puntoFinal);
    }

    void IntentarCapturar()
    {
        if (objetoApuntado == null) return;

        float distancia = Vector3.Distance(puntoOrigen.position, objetoApuntado.transform.position);

        if (distancia <= distanciaCaptura)
        {
            CapturarObjeto(objetoApuntado);
        }
        else
        {
            // Muy lejos para capturar
            Debug.Log($"Objeto muy lejos: {distancia:F1}m (máximo: {distanciaCaptura}m)");

            if (sonidoMuyLejos != null && audioSource != null)
                audioSource.PlayOneShot(sonidoMuyLejos);
        }
    }

    void CapturarObjeto(GameObject objeto)
    {
        Debug.Log("Capturando: " + objeto.name);

        // Cambiar color del rayo a captura
        if (lineRenderer != null)
        {
            lineRenderer.startColor = colorRayoCapturando;
            lineRenderer.endColor = colorRayoCapturando;
        }

        // Reproducir sonido
        if (sonidoCaptura != null && audioSource != null)
            audioSource.PlayOneShot(sonidoCaptura);

        // Crear efecto de partículas
        if (efectoCaptura != null)
        {
            Instantiate(efectoCaptura, objeto.transform.position, Quaternion.identity);
        }

        // Destruir el objeto
        Destroy(objeto);

        // Limpiar referencias
        objetoApuntado = null;
        if (indicadorCaptura != null)
            indicadorCaptura.SetActive(false);
    }

    // Visualizar rangos en el editor
    void OnDrawGizmosSelected()
    {
        if (puntoOrigen != null)
        {
            // Rango de captura (verde)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(puntoOrigen.position, distanciaCaptura);

            // Distancia máxima del rayo (cyan)
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(puntoOrigen.position, puntoOrigen.forward * distanciaMaxima);
        }
    }
}