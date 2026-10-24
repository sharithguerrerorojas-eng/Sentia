using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;
    
    [Header("Configuración de Suavizado")]
    public float velocidadCamara = 5f;
    public Vector3 desplazamiento;

    [Header("Zona Muerta (Dead Zone)")]
    public float zonaMuertaX = 1.5f;

    [Header("Límites del Escenario (Clamping)")]
    public bool usarLimites = true;
    public float limiteMinX = -10f;
    public float limiteMaxX = 10f;

    private void LateUpdate()
    {
        // Si no hay objetivo asignado, no hacer nada
        if (objetivo == null) return;

        // Posición base de la cámara
        Vector3 posicionDeseada = transform.position;

        // Calcular la diferencia en el eje X entre el personaje y la cámara
        float deltaX = objetivo.position.x - transform.position.x;

        // ZONA MUERTA: Solo mover la cámara si el personaje se aleja más de la zona muerta
        if (Mathf.Abs(deltaX) > zonaMuertaX)
        {
            posicionDeseada.x = objetivo.position.x - (Mathf.Sign(deltaX) * zonaMuertaX);
        }

        // Mantener la posición Y del objetivo + el desplazamiento vertical
        posicionDeseada.y = objetivo.position.y + desplazamiento.y;
        posicionDeseada.z = desplazamiento.z;

        // LÍMITES DEL ESCENARIO: Evitar que la cámara muestre áreas vacías
        if (usarLimites)
        {
            posicionDeseada.x = Mathf.Clamp(posicionDeseada.x, limiteMinX, limiteMaxX);
        }

        // Suavizado del movimiento (Lerp)
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara * Time.deltaTime);

        transform.position = posicionSuavizada;
    }

    // Esto dibuja guías visuales en el editor para que veas la zona muerta y los límites
    private void OnDrawGizmosSelected()
    {
        // Dibujar zona muerta (cian)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(zonaMuertaX * 2, 2f, 0f));

        // Dibujar límites del escenario (rojo)
        if (usarLimites)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(limiteMinX, transform.position.y - 5, 0), 
                           new Vector3(limiteMinX, transform.position.y + 5, 0));
            Gizmos.DrawLine(new Vector3(limiteMaxX, transform.position.y - 5, 0), 
                           new Vector3(limiteMaxX, transform.position.y + 5, 0));
        }
    }
}