using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationTiles : MonoBehaviour
{
    public Transform GenerationTerrain;
    public GameObject BlocInDestructible;
    public GameObject BlocDestructible;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 16; i++)
        {
            for (int j = 0; j <= 10; j++)
            {
                bool estZoneSpawn = EstZoneDeSpawn(i, j);
                bool estIndestructible = false;

                //Instantiate(Case, new Vector3(i, 0, j), Quaternion.identity).transform.parent = GenerationTerrain;
                if (i % 2 != 0 && j % 2 != 0 && !estZoneSpawn && j > 0 && j < 10)
                {
                    estIndestructible = true;
                   GameObject go =  Instantiate(BlocInDestructible, new Vector3(i, 0.6f, j), Quaternion.identity, GenerationTerrain);
                    go.name = $"bloc indestructible ({i}, {j})";
                }

                if (!estZoneSpawn && !estIndestructible && Random.value <= 0.9f)
                { 
                    GameObject go = Instantiate(BlocDestructible, new Vector3(i, 0.6f, j), Quaternion.identity, GenerationTerrain);
                    go.name = $"bloc destructible ({i}, {j})";

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool EstZoneDeSpawn(int i, int j)
    {
        if ((i == 0 && j == 0) || (i == 1 && j == 0) || (i == 0 && j == 1))
        {
            return true;
        }
        if ((i == 16 && j == 0) || (i == 15 && j == 0) || (i == 16 && j == 1))
        {
            return true;
        }
        if ((i == 0 && j == 10) || (i == 1 && j == 10) || (i == 0 && j == 9))
        {
            return true;
        }
        if ((i == 16 && j == 10) || (i == 15 && j == 10) || (i == 16 && j == 9))
        {
            return true;
        }


        return false;
    }
}
