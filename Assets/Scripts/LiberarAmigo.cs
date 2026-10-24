using UnityEngine;

public class LiberarAmigo : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject jaula;
    [SerializeField] private GameObject amigo;
    
    // ✅ NUEVO: Referencia al diálogo
    [SerializeField] private Dialogue dialogoAmigo;
    
    private bool isPlayerInRange = false;
    private bool amigoLiberado = false;
    private Jugador player;

    private void Start()
    {
        player = FindAnyObjectByType<Jugador>();
        
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        
        if (amigo != null)
            amigo.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !amigoLiberado)
        {
            Liberar();
        }
    }

    private void Liberar()
    {
        amigoLiberado = true;
        
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
        
        if (jaula != null)
            jaula.SetActive(false);
        
        if (amigo != null)
            amigo.SetActive(true);
        
        // ✅ NUEVO: Iniciar diálogo después de liberar
        if (dialogoAmigo != null)
        {
            dialogoAmigo.IniciarDialogoManual();
        }
        
        Debug.Log("¡Amigo liberado!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !amigoLiberado)
        {
            isPlayerInRange = true;
            if (interactionPrompt != null)
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