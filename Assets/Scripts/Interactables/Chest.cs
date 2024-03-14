using System.Collections;
using System.Collections.Generic;
using Treasure.EventBus;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private ObjectId _requiredCharacter;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
           if(col.GetComponent<IPlayableCharacter>().CharacterId.Value != _requiredCharacter.Value) return;
           EventBus<OnShowPuzzle>.Raise(new OnShowPuzzle());
        }
    }
}
