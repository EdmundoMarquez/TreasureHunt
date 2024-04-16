using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwordInfoBox : MonoBehaviour
{
    [SerializeField] private Image _swordIcon = null;
    [SerializeField] private TMP_Text _swordTitle = null;
    [SerializeField] private TMP_Text[] _statsTexts = null;
    
    public void Fill(string title, Sprite icon, DataProperty[] damageProperties)
    {
        _swordTitle.SetText(title);
        _swordIcon.sprite = icon;

        for (int i = 0; i < damageProperties.Length; i++)
        {
            _statsTexts[i].SetText($"{damageProperties[i].amount} de {damageProperties[i].propertyId.Value}");
        }
    }
}
