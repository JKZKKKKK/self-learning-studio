using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] GameObject[] FloorPerfabs;
    public void SpawnFloor()
    {
        int r = Random.Range(0, FloorPerfabs.Length);
        GameObject floor = Instantiate(FloorPerfabs[r], transform);//Instantiate(創建東西,在子物件)
        floor.transform.position = new Vector3(Random.Range(-6f, 1.5f ), -5.5f, 0);
    }
}
