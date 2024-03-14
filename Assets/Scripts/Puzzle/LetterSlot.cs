using UnityEngine;
using TMPro;

public class LetterSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text _letterText;

    public void Init(string letter)
    {
        _letterText.SetText(letter);
    }
}