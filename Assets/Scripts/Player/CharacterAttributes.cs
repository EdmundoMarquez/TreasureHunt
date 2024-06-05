namespace Treasure.Player
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "CharacterAttributes", menuName = "Player/CharacterAttributes", order = 0)]
    public class CharacterAttributes : ScriptableObject 
    {
        [Range(10,30)]
        public int MaxHealth = 10;
        public int Health = 10;
        [Range(0,5)]
        public int Speed = 5;
    }
}