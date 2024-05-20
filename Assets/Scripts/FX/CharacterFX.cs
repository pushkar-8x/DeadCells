using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] Material flashMat;
    private Material originalMat;

    [SerializeField] float flashDuration = 0.2f;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void PlayFlash()
    {
        StartCoroutine(PlayFlashRoutine());
    }

    private IEnumerator PlayFlashRoutine()
    {
        sr.sharedMaterial = flashMat;
        yield return new WaitForSeconds(flashDuration);
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
        CancelInvoke("BlinkSprite");
        sr.color = Color.white;
    }

    

}
