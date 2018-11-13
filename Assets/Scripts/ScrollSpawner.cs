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

        if (random == 0)
        {
            element1 = ElementType.Fire;
        }
        else if (random == 1)
        {
            element1 = ElementType.Air;
        }
        else if (random == 2)
        {
            element1 = ElementType.Earth;
        }
        else if (random == 3)
        {
            element1 = ElementType.Water;
        }

        if (this.GetComponent<EnemySpawning>().wavedowntimeui.activeSelf == true && spawned == false)
        {
            location1 = Random.Range(0, SpawnLocation.Length);
            location2 = Random.Range(0, SpawnLocation.Length);

            if (location1 != location2 && spawned == false)
            {
                spawned = true;
                GameObject scroll1 = Instantiate(scroll, SpawnLocation[location1]);
                GameObject scroll2 = Instantiate(scroll, SpawnLocation[location2]);
                
                scroll1.GetComponent<ScrollUI>().SetElement(element1);

                random = Random.Range(0, 3);
                if (random == 0)
                {
                    element2 = ElementType.Fire;
                }
                else if (random == 1)
                {
                    element2 = ElementType.Air;
                }
                else if (random == 2)
                {
                    element2 = ElementType.Earth;
                }
                else if (random == 3)
                {
                    element2 = ElementType.Water;
                }

                scroll2.GetComponent<ScrollUI>().SetElement(element2);
                
            }
        }

        if (this.GetComponent<EnemySpawning>().wavedowntimeui.activeSelf == false)
        {
            spawned = false;
        }
	}
}
