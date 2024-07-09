namespace Treasure.Damageables
{
    using UnityEngine;
    using Treasure.Common;

    public interface IEnemy
    {
        void Init();
        void OnDamage();
        void OnDead();
        void OnRevive();
        Transform Self{get;}
        Transform Player{get; set;} 
        bool IsDead{get; set;}
    }
}
