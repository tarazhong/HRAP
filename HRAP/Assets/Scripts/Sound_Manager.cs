﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour {
	Vector3 thumbPos;// position du curseur
	UILabel percent; // récuperation du label pourcentage
	UISlider volume; // récuperation du slider pour régler le volume 
	UICheckbox cb;
	// Use this for initialization
	void Start () {
		percent = GameObject.Find("percent").GetComponent<UILabel>();
		volume = GameObject.Find ("Slider").GetComponent<UISlider> ();
		cb = GameObject.Find ("Checkbox").GetComponent<UICheckbox> ();

	}
	
	// Update is called once per frame
	void Update () {
		
		thumbPos = GameObject.Find("Thumb").GetComponent<UISprite>().transform.position;
		percent.transform.position = thumbPos;
		percent.text = (volume.sliderValue * 100).ToString("0")+"%";
		if (cb.isChecked) {
			percent.enabled = true;
			volume.enabled = true;
			AudioListener.volume = volume.sliderValue; // reglage du volume général
		} else {
			percent.enabled = false;
			volume.enabled = false;
			AudioListener.volume = 0;
		}



	}
}
