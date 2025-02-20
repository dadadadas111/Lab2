using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFade : MonoBehaviour
{
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 0.5f;

    private Image barImage;
    private Image damagedBarImage;
    private float damagedHealthFadeTimer;
    private HealthSystem healthSystem;
    private GameMenu gameMenu;

    private bool isGameOver = false;

    private void Awake()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
        damagedBarImage = transform.Find("DamagedBar").GetComponent<Image>();
    }

    private void Start()
    {
        healthSystem = new HealthSystem(100);
        SetHealth(healthSystem.GetHealthNormalized());
        damagedBarImage.fillAmount = barImage.fillAmount;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        gameMenu = GameObject.Find("GameManager").GetComponent<GameMenu>();
    }

    private void Update()
    {
        // some key press to test the health system
        if (Input.GetKeyDown(KeyCode.F1))
        {
            healthSystem.Damage(10);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            healthSystem.Heal(10);
        }

        damagedHealthFadeTimer -= Time.deltaTime;
        if (damagedHealthFadeTimer < 0)
        {
            if (barImage.fillAmount < damagedBarImage.fillAmount)
            {
                float shrinkSpeed = 1f;
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }

        if (healthSystem.GetHealthNormalized() == 0 && !isGameOver)
        {
            gameMenu.GameOver();
            isGameOver = true;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public void Heal(int healAmount)
    {
        healthSystem.Heal(healAmount);
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        SetHealth(healthSystem.GetHealthNormalized());
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        damagedHealthFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;
        SetHealth(healthSystem.GetHealthNormalized());
    }

    public void SetHealth(float healthNormalized)
    {
        barImage.fillAmount = healthNormalized;
    }
}
