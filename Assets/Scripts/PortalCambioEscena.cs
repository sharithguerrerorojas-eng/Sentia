using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCambioEscena : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string escenaDestino = "InteriorChoza 2";
    [SerializeField] private Vector2 posicionSpawn = new Vector2(0, 0);
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Guardar posición de spawn
            PlayerPrefs.SetFloat("SpawnX", posicionSpawn.x);
            PlayerPrefs.SetFloat("SpawnY", posicionSpawn.y);
            PlayerPrefs.Save();
            
            // Cambiar de escena
            SceneManager.LoadScene(escenaDestino);
        }
    }
}