using UnityEngine;

public class MoverCuboIdaVuelta : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Vector3 puntoInicial;
    public Vector3 puntoFinal;
    public float velocidad = 2f;

    [Header("Control")]
    [Tooltip("True = Movimiento automático ida y vuelta | False = Control manual")]
    public bool movimientoAutomatico = true;

    [Header("Teclas de Control Manual")]
    public KeyCode teclaAdelante = KeyCode.W;
    public KeyCode teclaAtras = KeyCode.S;
    public KeyCode teclaIzquierda = KeyCode.A;
    public KeyCode teclaDerecha = KeyCode.D;
    public KeyCode teclaArriba = KeyCode.Space;
    public KeyCode teclaAbajo = KeyCode.LeftControl;

    private bool yendo = true; // True = yendo al final, False = volviendo

    void Start()
    {
        puntoInicial = transform.position;
    }

    void Update()
    {
        if (movimientoAutomatico)
        {
            // Movimiento automático de ida y vuelta
            MovimientoAutomatico();
        }
        else
        {
            // Control manual del cubo
            ControlManual();
        }
    }

    void MovimientoAutomatico()
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

    void ControlManual()
    {
        Vector3 movimiento = Vector3.zero;

        // Movimiento en los ejes
        if (Input.GetKey(teclaAdelante))
            movimiento += Vector3.forward;
        if (Input.GetKey(teclaAtras))
            movimiento += Vector3.back;
        if (Input.GetKey(teclaIzquierda))
            movimiento += Vector3.left;
        if (Input.GetKey(teclaDerecha))
            movimiento += Vector3.right;
        if (Input.GetKey(teclaArriba))
            movimiento += Vector3.up;
        if (Input.GetKey(teclaAbajo))
            movimiento += Vector3.down;

        // Aplicar el movimiento
        transform.position += movimiento.normalized * velocidad * Time.deltaTime;
    }
}