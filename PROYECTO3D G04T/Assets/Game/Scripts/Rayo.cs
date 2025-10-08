using UnityEngine;

public class RaycastVisualizer : MonoBehaviour
{
    [Header("Configuraci�n del Raycast")]
    [Tooltip("Distancia m�xima del rayo")]
    public float distanciaRayo = 10f;

    [Tooltip("Capa de objetos a detectar (opcional)")]
    public LayerMask capasDeteccion;

    [Header("Visualizaci�n Debug (Solo en Editor)")]
    [Tooltip("Color cuando NO detecta nada")]
    public Color colorSinDeteccion = Color.green;

    [Tooltip("Color cuando S� detecta algo")]
    public Color colorConDeteccion = Color.red;

    [Header("Visualizaci�n en Juego (LineRenderer)")]
    [Tooltip("Usar LineRenderer para ver el rayo en el juego")]
    public bool usarLineRenderer = false;

    private LineRenderer lineRenderer;

    void Start()
    {
        // Configurar LineRenderer si est� activado
        if (usarLineRenderer)
        {
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }

            // Configuraci�n del LineRenderer
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = 2;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
    }

    void Update()
    {
        // Lanzar el rayo desde la posici�n del objeto hacia adelante
        LanzarRaycast();

        // Tambi�n puedes lanzar desde el mouse (descomenta para probar)
        // LanzarRaycastDesdeMouse();
    }

    void LanzarRaycast()
    {
        // Origen y direcci�n del rayo
        Vector3 origen = transform.position;
        Vector3 direccion = transform.forward;

        RaycastHit hit;

        // Lanzar el raycast
        bool detecto = Physics.Raycast(origen, direccion, out hit, distanciaRayo, capasDeteccion);

        if (detecto)
        {
            // El rayo golpe� algo
            Debug.Log($"Rayo golpe�: {hit.collider.gameObject.name} a {hit.distance} metros");

            // Dibujar rayo en el editor (hasta el punto de impacto)
            Debug.DrawLine(origen, hit.point, colorConDeteccion);

            // Dibujar un punto en el lugar del impacto
            Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.yellow);

            // Actualizar LineRenderer si est� activo
            if (usarLineRenderer && lineRenderer != null)
            {
                lineRenderer.SetPosition(0, origen);
                lineRenderer.SetPosition(1, hit.point);
                lineRenderer.startColor = colorConDeteccion;
                lineRenderer.endColor = colorConDeteccion;
            }
        }
        else
        {
            // El rayo no golpe� nada
            Vector3 puntoFinal = origen + direccion * distanciaRayo;

            // Dibujar rayo completo en el editor
            Debug.DrawRay(origen, direccion * distanciaRayo, colorSinDeteccion);

            // Actualizar LineRenderer si est� activo
            if (usarLineRenderer && lineRenderer != null)
            {
                lineRenderer.SetPosition(0, origen);
                lineRenderer.SetPosition(1, puntoFinal);
                lineRenderer.startColor = colorSinDeteccion;
                lineRenderer.endColor = colorSinDeteccion;
            }
        }
    }

    void LanzarRaycastDesdeMouse()
    {
        // Crear un rayo desde la c�mara hacia donde apunta el mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // El mouse est� apuntando a un objeto
            Debug.DrawLine(ray.origin, hit.point, Color.cyan);

            // Si se hace clic, mostrar informaci�n
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log($"Click en: {hit.collider.gameObject.name}");
                Debug.Log($"Posici�n del impacto: {hit.point}");
                Debug.Log($"Normal de la superficie: {hit.normal}");
            }
        }
        else
        {
            // El mouse no apunta a nada
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.white);
        }
    }

    // M�todo opcional: Dibujar el rayo en la ventana Scene siempre
    void OnDrawGizmos()
    {
        // Este m�todo dibuja el rayo incluso cuando el objeto no est� seleccionado
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * distanciaRayo);
    }
}