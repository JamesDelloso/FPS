using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollowMouse : MonoBehaviour
{
    [SerializeField]
    private float zValue;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zValue;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));
    }
}
