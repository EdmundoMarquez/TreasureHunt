using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParameters", menuName = "SimpleRandomWalkSO", order = 0)]
public class SimpleRandomWalkSO : ScriptableObject 
{
    public int iterations = 10, walkLength = 10;
    public bool startRandomlyEachIteration = true;
}