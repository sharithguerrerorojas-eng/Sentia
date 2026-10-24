using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    // Referencias a los paneles
    [SerializeField] private GameObject capitulosPanel;
    [SerializeField] private GameObject opcionesPanel;

    // ============================================
    // BOTÓN: JUGAR
    // Carga la siguiente escena (GameScene)
    // ============================================
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // ============================================
    // BOTÓN: CAPÍTULOS
    // Muestra el panel de capítulos
    // ============================================
    public void AbrirCapitulos()
    {
        if (capitulosPanel != null)
        {
            capitulosPanel.SetActive(true);
            Debug.Log("Panel de Capítulos abierto");
        }
    }

    public void CerrarCapitulos()
    {
        if (capitulosPanel != null)
        {
            capitulosPanel.SetActive(false);
            Debug.Log("Panel de Capítulos cerrado");
        }
    }

    // ============================================
    // BOTÓN: OPCIONES
    // Muestra el panel de opciones
    // ============================================
    public void AbrirOpciones()
    {
        if (opcionesPanel != null)
        {
            opcionesPanel.SetActive(true);
            Debug.Log("Panel de Opciones abierto");
        }
    }

    public void CerrarOpciones()
    {
        if (opcionesPanel != null)
        {
            opcionesPanel.SetActive(false);
            Debug.Log("Panel de Opciones cerrado");
        }
    }

    // ============================================
    // BOTÓN: SALIR
    // Cierra la aplicación
    // ============================================
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}