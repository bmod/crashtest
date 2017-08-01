using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoActionOnHover : MonoBehaviour {


	public Text text;
	public Color hoverColor;
    public AudioSource onHoverSound;
    public AudioSource onMenuConfirm;

	Color ogColor;
	// Use this for initialization
	void Start () {
		ogColor = text.color;
	}


	public void MouseIn() {
		text.color = hoverColor;
		if(onHoverSound != null)
        onHoverSound.Play();
	}

	public void MouseOut() {
		text.color = ogColor;
	}

    public void ConfirmMenuItem()
    {
		if(onMenuConfirm != null)
        onMenuConfirm.Play();
    }
}
