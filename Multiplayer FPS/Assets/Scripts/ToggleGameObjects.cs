using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObjects : MonoBehaviour
{

    [SerializeField]
    private GameObject[] active;

    [SerializeField]
    private GameObject[] inactive;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in active)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in inactive)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggle()
    {
        foreach (GameObject go in active)
        {
            go.SetActive(!go.activeSelf);
        }
        foreach (GameObject go in inactive)
        {
            go.SetActive(!go.activeSelf);
        }
    }
}
