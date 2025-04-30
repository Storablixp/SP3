using UnityEngine;
using System.Collections;

public class WorldMutator : ScriptableObject
{
    [Header("Mutator")]
    [SerializeField] private bool IncludeAllTiles = true;
    public bool CanBeVisualized = true;
    protected bool visuals;
    protected WorldGenerator worldGenerator;

    [Header("Location (Only if IncludeAllTiles is false)")]
    [SerializeField] private int StartingDepthLevel;
    [SerializeField] private int EndDepthLevel;
    protected int startY;
    protected int endY;

    public virtual void SetUp(WorldGenerator worldGenerator, Vector2Int worldSize)
    {
        this.worldGenerator = worldGenerator;

        if (IncludeAllTiles)
        {
            startY = worldSize.y - 1;
            endY = 0;
        }
        else
        {
            startY = (worldSize.y - StartingDepthLevel) - 1;
            endY = (startY - (EndDepthLevel - StartingDepthLevel)) + 1;
        }

    }

    public virtual IEnumerator ApplyMutator(Vector2Int worldSize)
    {
        return null;
    }
}
