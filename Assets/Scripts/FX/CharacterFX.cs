using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFX : MonoBehaviour
{
    private SpriteRenderer charSpriteRenderer;
    [SerializeField] Material flashMat;
    private Material originalMat;

    [SerializeField] float flashDuration = 0.2f;

    private void Awake()
    {
        charSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMat = charSpriteRenderer.material;
    }

    public void PlayFlash()
    {
        StartCoroutine(PlayFlashRoutine());
    }

    private IEnumerator PlayFlashRoutine()
    {
        charSpriteRenderer.sharedMaterial = flashMat;
        yield return new WaitForSeconds(flashDuration);
        charSpriteRenderer.sharedMaterial = originalMat;
    }

}
