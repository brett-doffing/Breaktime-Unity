using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPase.Began)
        {
            Ray = ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, ou hit))
            {
                if (hit.collider != null)
                {
                    // Rotate
                    hit.collider.GetComponent
                }
            }
        }
    }
}
