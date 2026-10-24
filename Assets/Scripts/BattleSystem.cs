using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // ✅ Necesario para cambiar de escena

public class BattleSystem : MonoBehaviour
{
    [Header("Personajes")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    
    [Header("UI de Batalla")]
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider enemyHealthBar;
    [SerializeField] private TMP_Text battleText;
    
    [Header("Estadísticas")]
    [SerializeField] private float playerMaxHealth = 100f;
    [SerializeField] private float enemyMaxHealth = 80f;
    [SerializeField] private float playerAttackDamage = 25f;
    [SerializeField] private float enemyAttackDamage = 15f;
    
    private float playerCurrentHealth;
    private float enemyCurrentHealth;
    private bool isPlayerTurn = true;
    private bool battleActive = false;
    private Animator playerAnimator;
    private Animator enemyAnimator;

    private void Start()
    {
        playerAnimator = player != null ? player.GetComponent<Animator>() : null;
        enemyAnimator = enemy != null ? enemy.GetComponent<Animator>() : null;
        
        playerCurrentHealth = playerMaxHealth;
        enemyCurrentHealth = enemyMaxHealth;
        
        UpdateHealthBars();
        StartBattle();
    }

    public void StartBattle()
    {
        battleActive = true;
        isPlayerTurn = true;
        if (battleText != null)
            battleText.text = "¡Tu turno! J=Atacar, K=Defender";
    }

    private void Update()
    {
        if (!battleActive || !isPlayerTurn) return;
        
        if (Input.GetKeyDown(KeyCode.J)) PlayerAttack();
        else if (Input.GetKeyDown(KeyCode.K)) PlayerDefend();
    }

    private void PlayerAttack()
    {
        isPlayerTurn = false;
        if (battleText != null) battleText.text = "Nova ataca...";
        
        if (playerAnimator != null) playerAnimator.SetBool("IsAttacking", true);
        StartCoroutine(ApplyPlayerDamage());
    }

    private void PlayerDefend()
    {
        isPlayerTurn = false;
        if (battleText != null) battleText.text = "Nova se defiende...";
        StartCoroutine(EnemyTurn(0.5f)); // Recibe 50% menos daño
    }

    private IEnumerator ApplyPlayerDamage()
    {
        yield return new WaitForSeconds(0.5f);
        
        float damage = playerAttackDamage + Random.Range(-5f, 5f);
        enemyCurrentHealth -= damage;
        enemyCurrentHealth = Mathf.Max(0, enemyCurrentHealth);
        UpdateHealthBars();
        
        if (enemyAnimator != null) enemyAnimator.SetBool("IsHit", true);
        if (battleText != null) battleText.text = $"¡Causaste {damage:F0} de daño!";
        
        yield return new WaitForSeconds(1f);
        
        // Resetear animaciones
        if (playerAnimator != null) playerAnimator.SetBool("IsAttacking", false);
        if (enemyAnimator != null) enemyAnimator.SetBool("IsHit", false);
        
        if (enemyCurrentHealth <= 0)
        {
            if (battleText != null) battleText.text = "¡Victoria! Un portal ha aparecido...";
            if (playerAnimator != null) playerAnimator.SetBool("IsDead", true); // O animación de victoria
            
            battleActive = false;
            
            // ✅ 1. Desaparecer la sombra
            if (enemy != null) enemy.SetActive(false);
            
            // ✅ 2. Hacer aparecer el portal
            GameObject portal = GameObject.Find("PortalSalida");
            if (portal != null) portal.SetActive(true);
            
            // ✅ 3. Guardar progreso
            PlayerPrefs.SetInt("SombraDerrotada", 1);
            PlayerPrefs.Save();
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator EnemyTurn(float damageMultiplier = 1f)
    {
        if (battleText != null) battleText.text = "La sombra ataca...";
        yield return new WaitForSeconds(1f);
        
        if (enemyAnimator != null) enemyAnimator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.5f);
        
        float damage = (enemyAttackDamage + Random.Range(-3f, 3f)) * damageMultiplier;
        playerCurrentHealth -= damage;
        playerCurrentHealth = Mathf.Max(0, playerCurrentHealth);
        UpdateHealthBars();
        
        if (playerAnimator != null) playerAnimator.SetBool("IsHit", true);
        if (battleText != null) battleText.text = $"La sombra te causó {damage:F0} de daño";
        
        yield return new WaitForSeconds(1f);
        
        // Resetear animaciones
        if (playerAnimator != null) playerAnimator.SetBool("IsHit", false);
        if (enemyAnimator != null) enemyAnimator.SetBool("IsAttacking", false);
        
        if (playerCurrentHealth <= 0)
        {
            if (battleText != null) battleText.text = "Derrota...";
            battleActive = false;
        }
        else
        {
            isPlayerTurn = true;
            if (battleText != null) battleText.text = "Tu turno: J=Atacar, K=Defender";
        }
    }

    private void UpdateHealthBars()
    {
        if (playerHealthBar != null) playerHealthBar.value = playerCurrentHealth / playerMaxHealth;
        if (enemyHealthBar != null) enemyHealthBar.value = enemyCurrentHealth / enemyMaxHealth;
    }
}