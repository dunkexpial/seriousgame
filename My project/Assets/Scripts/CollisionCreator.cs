using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ReplaceAllTiles : MonoBehaviour
{
    private Tilemap tilemap;               // Reference to the Tilemap component on this GameObject
    public TileBase replacementTile;       // Tile to replace all tiles with

    // This method is no longer called automatically
    public void ReplaceTiles()
    {
        tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found on this GameObject.");
            return;
        }

        BoundsInt bounds = tilemap.cellBounds;

        #if UNITY_EDITOR
        Undo.RecordObject(tilemap, "Replace All Tiles"); // Record the tilemap for undo
        #endif

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Replace only if a tile exists at this position
                if (tilemap.HasTile(tilePosition))
                {
                    tilemap.SetTile(tilePosition, replacementTile);
                }
            }
        }

        #if UNITY_EDITOR
        EditorUtility.SetDirty(tilemap); // Mark tilemap as dirty to save changes
        #endif

        Debug.Log("Tiles replaced successfully.");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ReplaceAllTiles))]
public class ReplaceAllTilesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ReplaceAllTiles script = (ReplaceAllTiles)target;

        if (GUILayout.Button("Replace Tiles"))
        {
            script.ReplaceTiles();
        }
    }
}
#endif
