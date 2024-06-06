namespace Treasure.Rewards
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using Treasure.Common;

    public class RewardPlacer : MonoBehaviour
    {
        [SerializeField]
        private GameObject itemPrefab;
        [SerializeField]
        private Vector2 offset = new Vector2(-0.5f, -0.5f);

        
        public GameObject PlaceItem(ItemData item, Transform parent)
        {
            GameObject newItem = CreateObject(itemPrefab, parent);
            //GameObject newItem = Instantiate(itemPrefab, placementPosition, Quaternion.identity);
            newItem.GetComponent<RewardItem>().Initialize(item);
            return newItem;
        }

        public GameObject CreateObject(GameObject prefab, Transform itemParent)
        {
            if (prefab == null)
                return null;
            GameObject newItem;
            if (Application.isPlaying)
            {
                newItem = Instantiate(prefab, itemParent);
                newItem.transform.localPosition = offset;
            }
            else
            {
                newItem = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                newItem.transform.parent = itemParent;
                newItem.transform.localPosition = offset;
            }

            return newItem;
        }
    }
}