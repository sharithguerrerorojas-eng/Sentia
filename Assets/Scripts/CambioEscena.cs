using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    [Header("Nombre exacto de la siguiente escena")]
    [SerializeField] private string nombreEscenaSiguiente;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Solo cambia si toca el Player
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(nombreEscenaSiguiente);
        }
    }
}