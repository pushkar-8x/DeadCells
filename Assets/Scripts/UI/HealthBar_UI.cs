using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Character _character;
    private Slider healthBarSlider;
    private CharacterStats _characterStats;


    private void Awake()
    {
        healthBarSlider = GetComponentInChildren<Slider>();
        _character = GetComponentInParent<Character>();
        _characterStats = GetComponentInParent<CharacterStats>();
    }

    private void OnEnable()
    {
        _character.OnFlip += FlipUI;
        _characterStats.OnHealthChanged += UpdateHealthBarUI;
    }
    void Start()
    {              
        UpdateHealthBarUI();
    }

    private void FlipUI() => transform.Rotate(0, 180, 0);

    private void UpdateHealthBarUI()
    {
        healthBarSlider.maxValue = _characterStats.GetMaxHealthWithModifiers();
        healthBarSlider.value = _characterStats.currentHealth;
    }

    private void OnDisable()
    {
        _character.OnFlip -= FlipUI;
        _characterStats.OnHealthChanged -= UpdateHealthBarUI;
    }
}
