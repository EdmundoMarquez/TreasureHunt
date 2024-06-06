using System;

namespace Treasure.Common
{
    public interface IDamageable
    {
        void Damage(int amount, string instigator = "");
    }
}
