using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaAnimada : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject interactionPrompt; // El indicador "E"
    [SerializeField] private string escenaSiguiente = "InteriorChoza";
    
    [Header("Efecto de transición")]
    [SerializeField] private float duracionFade = 1.5f; // Segundos del efecto
    
    private bool isPlayerInRange = false;
    private bool puertaAbierta = false;
    private Jugador player;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        player = FindAnyObjectByType<Jugador>();
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        
        // Obtener el SpriteRenderer de la choza para el efecto fade
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !puertaAbierta)
        {
            AbrirPuerta();
        }
    }

    private void AbrirPuerta()
    {
        puertaAbierta = true;
        
        // Ocultar la "E"
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        
        // Bloquear movimiento de Nova
        if (player != null)
            player.canMove = false;
        
        // Iniciar efecto fade y cambiar escena
        StartCoroutine(FadeYCambiarEscena());
    }

    private IEnumerator FadeYCambiarEscena()
    {
        if (spriteRenderer != null)
        {
            Color colorOriginal = spriteRenderer.color;
            float tiempo = 0f;
            
            // Desvanecer la choza gradualmente
            while (tiempo < duracionFade)
            {
                tiempo += Time.deltaTime;
                float t = tiempo / duracionFade;
                
                Color color = colorOriginal;
                color.a = Mathf.Lerp(1f, 0f, t); // De opaco a transparente
                spriteRenderer.color = color;
                
                yield return null;
            }
        }
        
        // Pequeña pausa
        yield return new WaitForSecondsRealtime(0.3f);
        
        // Guardar spawn para la siguiente escena
        PlayerPrefs.SetFloat("SpawnX", 0f);
        PlayerPrefs.SetFloat("SpawnY", 0f);
        PlayerPrefs.Save();
        
        // Cambiar de escena
        SceneManager.LoadScene(escenaSiguiente);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionPrompt != null && !puertaAbierta)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }
}