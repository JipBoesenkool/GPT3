using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    private Vector3 mousePosition;
    public float moveSpeed = 0.1f;
    public List<GameObject> school;

    // Use this for initialization
    void Start()
    {
        school = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        if(this.transform.rotation.eulerAngles.z >= 90 && this.transform.rotation.eulerAngles.z <= 270)
        {
            this.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }


        if (Input.GetMouseButton(0))
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
        }

        // Get Angle in Radians
        float AngleRad = Mathf.Atan2(mousePosition.y - this.transform.position.y, mousePosition.x - this.transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            if(school.Count <= 0)
            {
                other.gameObject.transform.SetParent(GameObject.Find("School").transform);
                other.gameObject.GetComponent<TargetMovementScript>().inSchool = true;
                school.Add(other.gameObject);
                other.GetComponent<Collider2D>().enabled = false;
                other.GetComponent<TargetMovementScript>().leader = this.gameObject;
            }else if(school.Count > 0)
            {
                foreach (GameObject fish in school){
                    if (fish.GetComponent<TargetBehaviorScript>().color.Equals(other.GetComponent<TargetBehaviorScript>().color))
                    {
                        other.gameObject.transform.SetParent(GameObject.Find("School").transform);
                        other.gameObject.GetComponent<TargetMovementScript>().inSchool = true;
                        other.GetComponent<Collider2D>().enabled = false;
                        int index = school.Count - 1;
                        other.GetComponent<TargetMovementScript>().leader = school[index];
                        school.Add(other.gameObject);
                        break;
                    }else if (fish.GetComponent<TargetBehaviorScript>().shape.Equals(other.GetComponent<TargetBehaviorScript>().shape))
                    {
                        other.gameObject.transform.SetParent(GameObject.Find("School").transform);
                        other.gameObject.GetComponent<TargetMovementScript>().inSchool = true;
                        other.GetComponent<Collider2D>().enabled = false;
                        int index = school.Count - 1;
                        other.GetComponent<TargetMovementScript>().leader = school[index];
                        school.Add(other.gameObject);
                        break;
                    }
                }
            }
        }
    }
}

