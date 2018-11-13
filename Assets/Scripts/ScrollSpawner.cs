using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSpawner : MonoBehaviour {
    [SerializeField] private Transform[] SpawnLocation;
    [SerializeField] private GameObject scroll;
    private int location1;
    private int location2;
    private bool spawned = false;
    private ElementType element1;
    private ElementType element2;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        int random = Random.Range(0, 3);

		if(this.GetComponent<EnemySpawning>().wavedowntimeui.activeSelf == true && spawned == false)
        {
            location1 = Random.Range(0, SpawnLocation.Length);
            location2 = Random.Range(0, SpawnLocation.Length);

            if (location1 != location2)
            {
                GameObject scroll1 = Instantiate(scroll, SpawnLocation[location1]);
                GameObject scroll2 = Instantiate(scroll, SpawnLocation[location2]);
                element1 = (ElementType)random;
                scroll1.GetComponent<ScrollUI>().SetElement(element1);

                random = Random.Range(0, 3);
                element2 = (ElementType)random;
                scroll2.GetComponent<ScrollUI>().SetElement(element2);
                spawned = true;
            }
        }

        if (this.GetComponent<EnemySpawning>().wavedowntimeui.activeSelf == false)
        {
            spawned = false;
        }
	}
}
