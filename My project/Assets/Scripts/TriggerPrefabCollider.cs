using UnityEngine;

public class TriggerPrefabCollider : MonoBehaviour
{
    public GameObject prefabToSpawn; // Prefab to instantiate
    private GameObject spawnedPrefab; // Reference to the instantiated prefab
    public GameObject[] bossWallObjects; // Drag all BossWall objects into this array in the inspector

    public AudioSource bossMusic; // Música a ser tocada no evento do Trigger
    private PauseMenu pauseMenu; // Referência ao script PauseMenu

    private void Start()
    {
        // Obtém o script PauseMenu
        pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenu não encontrado na cena!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && spawnedPrefab == null)
        {
            PlayerManager.reachedBossArea = true;

            // Destroy enemies and spawners
            DestroyAllObjectsWithTag("Enemy");
            DestroyAllObjectsWithTag("Spawner");

            // Instantiate the prefab at the trigger's position
            spawnedPrefab = Instantiate(prefabToSpawn, transform.position, transform.rotation);

            // Delete inactive objects tagged as "Boss3TpInactive"
            DeleteBoss3TpInactiveObjects();

            // Activate all BossWall objects
            ActivateBossWallObjects();

            // Altera a música para a do boss
            if (pauseMenu != null && bossMusic != null)
            {
                pauseMenu.SetCurrentMusic(bossMusic);
            }
        }
    }

    private void DestroyAllObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

    private void DeleteBoss3TpInactiveObjects()
    {
        GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("Boss3TpInactive");
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }
    }

    private void ActivateBossWallObjects()
    {
        foreach (GameObject wall in bossWallObjects)
        {
            if (wall != null)
            {
                wall.SetActive(true);
            }
        }
    }
}
