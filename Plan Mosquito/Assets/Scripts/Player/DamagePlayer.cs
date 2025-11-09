using UnityEngine;
using System.Collections;
public class DamagePlayer : MonoBehaviour
{
    //Blink
    public SpriteRenderer _spriteRenderer;
    public Color _flashColor = Color.gray;
    public float _flashDuration = 0.1f;
    public int _flashCount = 3;

    public void BlinkDamage()
    {
        StartCoroutine(FlashCoroutine());
    }
    private IEnumerator FlashCoroutine()
    {
        Color originalColor = _spriteRenderer.color;

        for (int i = 0; i < _flashCount; i++)
        {
            _spriteRenderer.color = _flashColor;
            yield return new WaitForSeconds(_flashDuration);
            _spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(_flashDuration);
        }
    }
}
