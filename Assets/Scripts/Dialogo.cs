using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    [Header("Configuración del Diálogo")]
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(3, 10)] private string[] dialogueLines;
    
    [Header("Control del camino")]
    [SerializeField] private GameObject objetoABloquear;
    
    [Header("Modo de Inicio")]
    [SerializeField] private bool inicioAutomatico = false;

    private float typingSpeed = 0.05f;

    private bool isPlayerInRange;
    private bool dialogueStarted;
    private bool yaFinalizado = false;
    private int lineIndex;

    private Jugador player;

    private void Start()
    {
        player = FindAnyObjectByType<Jugador>();
        
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        
        if (dialogueMark != null)
            dialogueMark.SetActive(false);
            
        if (dialogueText == null || dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogError($"⚠️ FALTAN REFERENCIAS en: {gameObject.name}. Revisa el Inspector.", gameObject);
        }
    }

    void Update()
    {
        if (dialogueText == null || dialogueLines == null || dialogueLines.Length == 0)
        {
            return; 
        }

        // Solo permitir avanzar si el diálogo está activo
        if (dialogueStarted)
        {
            if (dialogueText.text == dialogueLines[lineIndex])
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
                {
                    NextDialogueLine();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
                {
                    StopAllCoroutines();
                    dialogueText.text = dialogueLines[lineIndex];
                }
            }
        }
    }

    public void IniciarDialogoManual()
    {
        if (!dialogueStarted)
        {
            StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            
            // ✅ SIEMPRE iniciar diálogo primero
            if (!dialogueStarted && !yaFinalizado)
            {
                StartDialogue();
            }
            else if (dialogueMark != null)
            {
                dialogueMark.SetActive(true);
            }
        }
    }

    private void StartDialogue()
    {
        Debug.Log($"🔍 Iniciando diálogo en: {gameObject.name}");
        Debug.Log($" dialoguePanel: {(dialoguePanel != null ? "ASIGNADO" : "NULL")}");
        Debug.Log($"📝 dialogueText: {(dialogueText != null ? "ASIGNADO" : "NULL")}");
        Debug.Log($"📄 dialogueLines: {dialogueLines?.Length ?? 0} líneas");
    
        if (dialogueText == null || dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogError($"¡ERROR en {gameObject.name}! No se puede iniciar el diálogo.", gameObject);
            return;
        }

        dialogueStarted = true;
        yaFinalizado = false;
        lineIndex = 0;
        
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);
        
        if (dialogueMark != null)
            dialogueMark.SetActive(false);
        
        if (player != null)
        {
            player.canMove = false;
        }
        
        StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        if (yaFinalizado) return;
        yaFinalizado = true;
        
        dialogueStarted = false;
        isPlayerInRange = false;
        
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        
        if (dialogueMark != null)
            dialogueMark.SetActive(false);
        
        if (player != null)
            player.canMove = true;

        StartCoroutine(CambiarAEscenaPelea());

        if (objetoABloquear != null)
        {
            objetoABloquear.SetActive(false);
        }

    }

    private IEnumerator CambiarAEscenaPelea()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        
        PlayerPrefs.SetInt("ViniendoDelDialogo", 1);
        PlayerPrefs.SetFloat("SpawnPeleaX", 0f);
        PlayerPrefs.SetFloat("SpawnPeleaY", 0f);
        PlayerPrefs.Save();
        
        Debug.Log("🌀 Teletransportando a escena Pelea...");
        SceneManager.LoadScene("Pelea");
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            if (dialogueStarted && !yaFinalizado)
            {
                EndDialogue();
            }
            
            if (dialogueMark != null)
            {
                dialogueMark.SetActive(false);
            }
        }
    }
}