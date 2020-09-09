using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour {

    public GameObject cubeToCreate;

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Left Click");
            ClickToDestroy();
        }
        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Right Click");
            ClickToCreate();
        }
    }

    void ClickToDestroy()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(camRay, out hit))
        {
            Debug.Log(hit.collider.gameObject);
            Destroy(hit.collider.gameObject);
            return;
        }
    }

    void ClickToCreate()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(camRay, out hit))
        {
            Debug.Log(hit.collider.gameObject);
            GameObject cube = Instantiate(cubeToCreate, hit.collider.gameObject.transform.position + hit.normal, Quaternion.identity);
            cube.transform.parent = this.transform;
        }
    }
}
