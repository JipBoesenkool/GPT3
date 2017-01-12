using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    private Color32 _original;
    private Color32 _red = new Color32(160, 22, 22, 128);

    public GameObject debugPoint;
    public Vector3 tankSize;

    public GameObject fishPrefab;
    public int numFish = 10;
    public List<GameObject> fishes;

    public Vector3 goalPos = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        //init fields
        fishes = new List<GameObject>();

        //get tanksize (bounding box)
        tankSize = this.GetComponent<BoxCollider>().size;
        tankSize /= 2;

        //spawn fishes
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = randomPos();
            GameObject fish = (GameObject)Instantiate(
                fishPrefab,
                pos,
                Quaternion.identity
            );
            fish.transform.parent = this.transform;
            //check the layer where the fish is spawned to determine it's size
            float posY = fish.transform.position.y;

            int maxScale = 1;
            if (posY > 399){maxScale = 1;}
            else if (posY > 299){maxScale = 2;}
            else if (posY > 199){maxScale = 3;}
            else if (posY > 99){maxScale = 4;}

            //---OM VIS THE SPAWNEN: fish.OnSpawn(float size, float requiredPoints);

            //fish.transform.localScale = Vector3.one * Random.Range(maxScale, maxScale);

            fish.GetComponent<FishScript>().OnSpawn(1, 0);
            fishes.Add(fish);
        }

        //goalPos
        goalPos = randomPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 10000) < 50)
        {
            goalPos = randomPos();

            if (debugPoint != null)
            {
                debugPoint.transform.position = goalPos;
            }
        }
    }

    private Vector3 randomPos()
    {
        return new Vector3(
            Random.Range(-tankSize.x, tankSize.x) + transform.position.x,
            Random.Range(-tankSize.y, tankSize.y) + transform.position.y,
            Random.Range(-tankSize.z, tankSize.z) + transform.position.z
        );
    }

    public void RemoveFish(GameObject fish)
    {
        fishes.Remove(fish);
    }
}
