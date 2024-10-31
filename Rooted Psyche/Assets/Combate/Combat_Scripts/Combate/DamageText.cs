using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI damageText;

    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
    }

    public void ShowAndDestroy()
    {
        // Mover y desaparecer (aquí puedes agregar efectos como animación o partículas)
        // Destruir después de 1 segundo
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime);
        Color color = damageText.color;
        color.a -= Time.deltaTime; // Se desvanece gradualmente
        damageText.color = color;
    }
}
