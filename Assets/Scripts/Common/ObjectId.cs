namespace Treasure.Common
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "ObjectId", menuName = "ObjectId", order = 0)]
    public class ObjectId : ScriptableObject 
    {
        [SerializeField] private string _objectId;
        public string Value => _objectId;
    }
}