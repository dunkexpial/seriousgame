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

    void Start()
    {
        tilemap = GetComponent<Tilemap>(); // Get the Tilemap component on this GameObject
        if (tilemap != null)
        {
            ReplaceTiles();
        }
        else
        {
            Debug.LogError("Tilemap component not found on this GameObject.");
        }
    }

    void ReplaceTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        #if UNITY_EDITOR
        Undo.RecordObject(tilemap, "Replace All Tiles"); // Record the tilemap for undo
        #endif

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);

                // Check if there is a tile at this position
                if (tilemap.HasTile(tilePosition))
                {
                    // Replace with the replacement tile
                    tilemap.SetTile(tilePosition, replacementTile);
                }
            }
        }

        #if UNITY_EDITOR
        EditorUtility.SetDirty(tilemap); // Mark tilemap as dirty to save changes
        #endif
    }
}