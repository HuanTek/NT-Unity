using UnityEngine;
using System.Collections;

public class OutLineSelect : MonoBehaviour {

    public int Iterations = 3;
    public float Spread = 0.7f;
    public Color outterColor = new Color(0.133f, 1, 0, 1);

    public GameObject[] targetsGroup1;
    public GameObject[] targetsGroup2;

    // Use this for initialization
    void Start () {
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        foreach (GameObject target in targetsGroup1)
        {
            target.AddComponent<OutLineObject>().outterColor = outterColor;
        }
        //foreach (GameObject target in targetsGroup2)
        //{
        //    target.AddComponent<ShowSelectedBump>().outterColor = outterColor;
        //}
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
