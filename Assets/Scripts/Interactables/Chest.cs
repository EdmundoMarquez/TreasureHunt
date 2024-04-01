using System.Collections;
using System.Collections.Generic;
using Treasure.EventBus;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private ObjectId _requiredCharacter;
    [SerializeField] private ChestData _chestData;
    [SerializeField] private GameObject _lockedSprite;
    [SerializeField] private GameObject _unlockedSprite;
    private WordData _wordToSolve;
    private bool _isUnlocked;

    private void Start()
    {
        _wordToSolve = _chestData.RandomizeWord();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(_isUnlocked) return;
        if(col.tag == "Player")
        {
           if(col.GetComponent<IPlayableCharacter>().CharacterId.Value != _requiredCharacter.Value) return;
           EventBus<OnShowPuzzle>.Raise(new OnShowPuzzle
           {
                chest = gameObject,
                puzzleWord = _wordToSolve
           });
        }
    }

    public void ToggleLock(bool unlock)
    {
        _isUnlocked = unlock;
        _lockedSprite.SetActive(!unlock);
        _unlockedSprite.SetActive(unlock);
    }
}
