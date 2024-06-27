using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Treasure.Common
{
    public class SpriteColorModifier : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private Color[] variations;

        public void ChangeColor(int index)
        {
            spriteRenderer.color = variations[index];
        }
    }
}