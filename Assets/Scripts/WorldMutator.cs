using UnityEngine;
using System.Collections;

public class WorldMutator : ScriptableObject
{
    [Header("Mutator")]
    [SerializeField] private bool IncludeAllTiles = true;
    protected WorldGenerator worldGenerator;

    public virtual void SetUp(WorldGenerator worldGenerator)
    {
        this.worldGenerator = worldGenerator;

    }

    public virtual IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        return null;
    }
}
