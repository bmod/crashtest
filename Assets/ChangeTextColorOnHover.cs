using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextColorOnHover : MonoBehaviour {


	public Text text;
	public Color hoverColor;
	Color ogColor;
	// Use this for initialization
	void Start () {
		ogColor = text.color;
	}


	public void MouseIn() {
		text.color = hoverColor;
	}

	public void MouseOut() {
		text.color = ogColor;
	}
}
