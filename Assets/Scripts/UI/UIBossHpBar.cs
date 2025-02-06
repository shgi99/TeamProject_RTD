using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHpBar : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI HpText;
    public void UpdateHpBar(float maxHp, float currentHp, Color hpBarColor)
    {
        GetComponent<Slider>().value = (float)currentHp / maxHp;
        fillImage.color = hpBarColor;
        HpText.text = $"{currentHp} / {maxHp}";
    }
}
