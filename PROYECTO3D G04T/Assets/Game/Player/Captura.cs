using UnityEngine;
using TMPro;

public class RayCapture : MonoBehaviour
{
    [Header("Configuraci�n del Rayo")]
    public LineRenderer lineRenderer;
    public Transform puntoOrigen;           // Desde donde sale el rayo (mano del personaje)
    public float distanciaMaxima = 15f;     // Distancia m�xima del rayo
    public float distanciaCaptura = 5f;     // Distancia para poder capturar
    public LayerMask capasCapturables;      // Qu� objetos se pueden capturar

    [Header("Detecci�n")]
    public Color colorRayoNormal = Color.cyan;
    public Color colorRayoDetectando = Color.yellow;
    public Color colorRayoCapturando = Color.red;
    public Color colorRayoMuyLejos = Color.gray;

    [Header("Efectos")]
    public GameObject efectoCaptura;        // Part�culas al capturar
    public AudioClip sonidoCaptura;
    public AudioClip sonidoMuyLejos;        // Sonido cuando est� muy lejos

    [Header("UI")]
    public TextMeshProUGUI textoDistancia;  // Muestra la distancia al objeto
    public GameObject indicadorCaptura;     // Icono sobre objeto capturable

    private GameObject objetoApuntado;
    private AudioSource audioSource;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Configurar LineRenderer
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.enabled = true;  // Siempre encendido
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

        // Mostrar UI desde el inicio
        if (textoDistancia != null)
            textoDistancia.gameObject.SetActive(true);

        if (indicadorCaptura != null)
            indicadorCaptura.SetActive(false);
    }

    void Update()
    {
        // Actualizar rayo constantemente
        DetectarObjetoConRaycast();

        // Capturar con clic izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            IntentarCapturar();
        }
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
            // Golpe� un objeto capturable
            puntoFinal = hit.point;
            objetoApuntado = hit.collider.gameObject;

            float distancia = Vector3.Distance(puntoOrigen.position, hit.point);

            // Actualizar UI de distancia
            if (textoDistancia != null)
            {
                textoDistancia.text = $"Objetivo: {objetoApuntado.name}\nDistancia: {distancia:F1}m";

                if (distancia <= distanciaCaptura)
                {
                    textoDistancia.text += "\n[Click para Capturar]";
                    textoDistancia.color = Color.green;
                }
                else
                {
                    textoDistancia.text += "\n[Ac�rcate m�s]";
                    textoDistancia.color = Color.red;
                }
            }

            // Cambiar color seg�n distancia
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
            // No golpe� nada
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
        if (objetoApuntado == null)
        {
            Debug.Log("No hay objetivo apuntado");
            return;
        }

        float distancia = Vector3.Distance(puntoOrigen.position, objetoApuntado.transform.position);

        if (distancia <= distanciaCaptura)
        {
            CapturarObjeto(objetoApuntado);
        }
        else
        {
            // Muy lejos para capturar
            Debug.Log($"Objeto muy lejos: {distancia:F1}m (m�ximo: {distanciaCaptura}m)");

            if (sonidoMuyLejos != null && audioSource != null)
                audioSource.PlayOneShot(sonidoMuyLejos);
        }
    }

    void CapturarObjeto(GameObject objeto)
    {
        Debug.Log("�Capturando: " + objeto.name + "!");

        // Cambiar color del rayo a captura brevemente
        lineRenderer.startColor = colorRayoCapturando;
        lineRenderer.endColor = colorRayoCapturando;

        // Volver al color normal despu�s de un breve momento
        Invoke("RestaurarColorRayo", 0.2f);

        // Reproducir sonido
        if (sonidoCaptura != null && audioSource != null)
            audioSource.PlayOneShot(sonidoCaptura);

        // Crear efecto de part�culas
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

    void RestaurarColorRayo()
    {
        lineRenderer.startColor = colorRayoNormal;
        lineRenderer.endColor = colorRayoNormal;
    }

    // Visualizar rangos en el editor
    void OnDrawGizmosSelected()
    {
        if (puntoOrigen != null)
        {
            // Rango de captura (verde)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(puntoOrigen.position, distanciaCaptura);

            // Distancia m�xima del rayo (cyan)
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(puntoOrigen.position, puntoOrigen.forward * distanciaMaxima);
        }
    }
}