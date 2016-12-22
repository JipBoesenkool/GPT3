using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishRecruitScript : MonoBehaviour
{
	public BoidController bc;

    public float numberOfSizeNeededToCollectBigger;

    public float numberOfSizeCollected = 0;
    private float biggestFish;
    public float joinSize = 1; //Smallest size able to recruit to school. 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if required number of collected fish is reached, allow collection of one size bigger.
        if(numberOfSizeCollected >= numberOfSizeNeededToCollectBigger)
        {
            numberOfSizeCollected = 0; // reset number of collected.
            joinSize++; 
        }
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Fish"))
		{
            float fishSize = other.GetComponent<FishScript>().size; 

            if (other.GetComponent<Flock>() != null)//If the other fish is still wild(Flock gets removed as it joins the school
            {
                if (!other.GetComponent<Flock>().fleeing) //If the fish is fleeing, can not be collected
                {
                    if (fishSize <= joinSize)
                    {
                        bc.AddFish(other.gameObject);
                        //If current biggest collectable size, number of collected by 1
                        if (fishSize == joinSize)
                        {
                            numberOfSizeCollected++;
                        }
                    }
                    else
                    {
                        other.GetComponent<Flock>().Flee(this.gameObject); //Tell other fish to flee from this school
                    }
                }
            }
		}
	}

	/*
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {

            if (!other.GetComponent<FishScript>().inSchool)
            {
                if (!easyMode)
                {
                    if (school.Count <= 0)
                    {
                        other.gameObject.transform.SetParent(GameObject.Find("School").transform);
                        other.GetComponent<Collider>().enabled = false;
                        school.Add(other.gameObject);
                    }
                    else if (school.Count > 0)
                    {
                        foreach (GameObject fish in school)
                        {
                            //Only add fish if you already have fish of the same color.
                            if (fish.GetComponent<FishScript>().color.Equals(other.GetComponent<FishScript>().color))
                            {
                                other.gameObject.transform.SetParent(GameObject.Find("School").transform);
                                other.GetComponent<Collider>().enabled = false;
                                school.Add(other.gameObject);
                                break;
                            }
                            else if (fish.GetComponent<FishScript>().type.Equals(other.GetComponent<FishScript>().type))
                            {
                                other.gameObject.transform.SetParent(GameObject.Find("School").transform);
                                other.GetComponent<Collider>().enabled = false;
                                school.Add(other.gameObject);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    other.gameObject.transform.SetParent(GameObject.Find("School").transform);
                    school.Add(other.gameObject);
                    other.GetComponent<Collider>().enabled = false;
                }
            }

        }
    }
*/
}
