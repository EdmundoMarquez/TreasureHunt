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

        
        public GameObject PlaceItem(ItemData item, Vector2 placementPosition)
        {
            GameObject newItem = CreateObject(itemPrefab, placementPosition);
            //GameObject newItem = Instantiate(itemPrefab, placementPosition, Quaternion.identity);
            newItem.GetComponent<RewardItem>().Initialize(item);
            return newItem;
        }

        public GameObject CreateObject(GameObject prefab, Vector3 placementPosition)
        {
            if (prefab == null)
                return null;
            GameObject newItem;
            if (Application.isPlaying)
            {
                newItem = Instantiate(prefab, placementPosition, Quaternion.identity);
            }
            else
            {
                newItem = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                newItem.transform.position = placementPosition;
                newItem.transform.rotation = Quaternion.identity;
            }

            return newItem;
        }
    }
}