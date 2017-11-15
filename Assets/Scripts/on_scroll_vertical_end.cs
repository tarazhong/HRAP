﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class On_scroll_vertical_end : MonoBehaviour {
	

	UIButton button_continue;
	UIToggle cb;
	UIScrollBar vertical_scrollbar;
	// Use this for initialization
	void Start(){
		try{
			button_continue = GameObject.Find("Bouton_continuer").GetComponent<UIButton>();
			vertical_scrollbar = GameObject.Find ("Vertical_Scroll_Bar").GetComponent<UIScrollBar>();
			cb = GameObject.Find ("Checkbox").GetComponent<UIToggle> (); 
		}catch(UnityException e){
			Debug.LogException (e, this);
		
		}

		Set_Interactable_Button(false);
		//vertical_scrollbar.enabled = true;
		//Set_Interactable_Button (false); // rends indisponible le bouton
	}

	// Update is called once per frame
	void Update () {
		if (vertical_scrollbar.value >=0.99 && cb.value) { // tant que le scroll n'est pas en bas
			Set_Interactable_Button (true);// rendre le bouton clickable 
		} else
			Set_Interactable_Button (false);
	}

	void Set_Interactable_Button(bool b)
	{
		button_continue.isEnabled = b;
	}
}