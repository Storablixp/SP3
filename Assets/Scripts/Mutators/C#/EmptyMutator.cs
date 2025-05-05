using UnityEngine;
using System.Collections;

//[CreateAssetMenu(fileName = "Mutator", menuName = "Scriptable Objects/World Mutator/")]
public class EmptyMutator : WorldMutatorSO
{
    public override IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        for (int arrayX = 0; arrayX < worldSize.x; arrayX++)
        {
            for (int arrayY = startY; arrayY >= endY; arrayY--)
            {
               
            }
        }

        yield return null;
    }
}
