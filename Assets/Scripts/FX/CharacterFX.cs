using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AilmentType
{
    Chilled,
    Ignited,
    Electrified
}

[System.Serializable]
public struct AilmentData
{
    public AilmentType ailmentType;
    public Sprite image;
    public Color[] effectColors;
}

public class CharacterFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] Material flashMat;
    private Material originalMat;
    [SerializeField] List<AilmentData> ailmentData;
    [SerializeField] float flashDuration = 0.2f;
    [SerializeField] Image ailmentImage;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
        ailmentImage.enabled = false;
    }

    private void Start()
    {
        
    }

    public void PlayFlash()
    {
        StartCoroutine(PlayFlashRoutine());
    }

    private IEnumerator PlayFlashRoutine()
    {
        sr.sharedMaterial = flashMat;
        Color tempColor = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        sr.color = tempColor;
        sr.sharedMaterial = originalMat;
    }

    private void BlinkSprite()
    {
        if(sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelBlink()
    {
        CancelInvoke(nameof(BlinkSprite));
        sr.color = Color.white;
    }

    public void SetTransparent(bool _transparent)
    {
        sr.color = _transparent ? Color.clear : Color.white;
    }

    public void PlayAilmentEffects(AilmentType _ailmentType , float _duration)
    {
        AilmentData currentAilmentData = GetAilmentData(_ailmentType);
       StopCoroutine(nameof(BlinkSprite));
        StartCoroutine(BlinkSprite(currentAilmentData ,_duration));
    }

    private AilmentData GetAilmentData(AilmentType ailmentType)
    {
        foreach (AilmentData ailment in ailmentData)
        {
            if (ailment.ailmentType == ailmentType)
            {
                return ailment;
            }
        }
        return ailmentData[0];
    }

    private IEnumerator BlinkSprite(AilmentData ailmentData, float _duration)
    {
        float elapsed = 0f;
        int colorIndex = 0;
        ailmentImage.enabled = true;
        ailmentImage.sprite = ailmentData.image;
        while (elapsed < _duration)
        {
            sr.color = ailmentData.effectColors[colorIndex];
            colorIndex = (colorIndex + 1) % 2; // Toggle between 0 and 1
            yield return new WaitForSeconds(0.2f); // Change color every 0.5 seconds
            elapsed += 0.2f;
        }
        ailmentImage.enabled = false;
        // Reset the color to the original color or the first color in the array
        sr.color = Color.white;
    }

}
