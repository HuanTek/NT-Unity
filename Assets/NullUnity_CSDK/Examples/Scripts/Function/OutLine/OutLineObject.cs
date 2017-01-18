using UnityEngine;
using System.Collections;

public class OutLineObject : MonoBehaviour {

    public Shader selectedShader;
    public Color outterColor;


    private Color myColor;
    private Shader myShader;

    // Use this for initialization
    void Start () {
        myColor = GetComponent<Renderer>().material.color;
        myShader = GetComponent<Renderer>().material.shader;
        selectedShader = Shader.Find("Hidden/RimLightSpce");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowOutLine() {
        GetComponent<Renderer>().material.shader = selectedShader;
        GetComponent<Renderer>().material.SetColor("_RimColor", outterColor);
    }

    public void HideOutLine() {
        GetComponent<Renderer>().material.color = myColor;
        GetComponent<Renderer>().material.shader = myShader;
    }
}
