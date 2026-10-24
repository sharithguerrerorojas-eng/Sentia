using UnityEngine;

public class ActivarSombra : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject sombra; // La sombra que aparecerá
    [SerializeField] private GameObject dialoguePanel; // El panel de diálogo (opcional)
    
    [Header("Configuración")]
    [SerializeField] private bool iniciarDialogoAutomatico = true; // Si quieres que el diálogo inicie solo
    
    private bool sombraActivada = false;
    private Jugador player;

    private void Start()
    {
        player = FindAnyObjectByType<Jugador>();
        
        // Asegurar que la sombra inicie invisible
        if (sombra != null)
            sombra.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !sombraActivada)
        {
            sombraActivada = true;
            
            // Activar la sombra
            if (sombra != null)
            {
                sombra.SetActive(true);
            }
            
            // Si quieres que el diálogo inicie automáticamente
            if (iniciarDialogoAutomatico)
            {
                // Buscar el script Dialogue en la sombra y llamar StartDialogue
                Dialogue dialogueScript = sombra.GetComponent<Dialogue>();
                if (dialogueScript != null)
                {
                    // Llamar al método público para iniciar diálogo
                    dialogueScript.IniciarDialogoManual();
                }
            }
        }
    }
}