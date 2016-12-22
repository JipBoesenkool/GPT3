using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFishRecruitScript : MonoBehaviour
{
	public BoidController bc;

    // Use this for initialization
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {

    }

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Fish"))
		{
			//TODO: need logic here when fish should be added or not
			//if not call flock.Flee(player.gameobject)
			bc.AddFish (
				other.gameObject
			);
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
