using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private static ItemGenerator _instance;
    public static ItemGenerator Instance { get { return _instance; } }

    public GameObject itemPrefab;
    public Vector2 spawnArea;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public void GenerateItem()
    {
        Vector3 randomPos = new Vector3(Random.Range(-spawnArea.x / 2, spawnArea.x / 2), Random.Range(-spawnArea.y / 2, spawnArea.y / 2), 0);
        GameObject item = Instantiate(itemPrefab, transform.position + randomPos, Quaternion.identity);
    }

}