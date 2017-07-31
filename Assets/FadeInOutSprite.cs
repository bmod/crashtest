using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutSprite : MonoBehaviour {


	public SpriteRenderer spriteRenderer;

	public float pingPongLen = .7f;
	float ogalpha;

	// Use this for initialization
	void Start () {
		ogalpha = spriteRenderer.color.a;
	}
	
	// Update is called once per frame
	void Update () {
		if (this == null || spriteRenderer == null)
			return;
		float alphaAmend = -Mathf.PingPong (Time.time, pingPongLen);
		spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.b, spriteRenderer.color.b, ogalpha + alphaAmend);
	}
}
