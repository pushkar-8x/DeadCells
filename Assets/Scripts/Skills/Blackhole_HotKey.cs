using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private KeyCode _hotKey;
    private TextMeshProUGUI hotKeyText;
    private Blackhole_SkillController _blackHole;
    private Transform _owningEnemy;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        hotKeyText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetupHotKey(Blackhole_SkillController _blackHole , KeyCode hotKey, Transform _owningEnemy)
    {
        this._hotKey = hotKey;
        this._owningEnemy = _owningEnemy;
        this._blackHole = _blackHole;
        hotKeyText.text = hotKey.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(_hotKey))
        {
            //_blackHole.AddTargetToList(_owningEnemy);
            _spriteRenderer.color = Color.clear;
            hotKeyText.color = Color.clear;
        }
    }
}
