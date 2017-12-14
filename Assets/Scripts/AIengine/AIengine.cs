﻿using HRAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIengine : MonoBehaviour
{
    private M_Candidate candidate;
    private P_Interview interview;
    private IHMInterview ihm;
    private CameraManager cameraManager;
    // ChatManager chatm;

    // TODO has to get arguments from previous view
    // private string candidateName;
    // private string targetJob; 

    // Use this for initialization
    void Start () {
        // 1. We initialise model, view and Presenter with name of candidate and his job
        ihm = GetComponent<IHMInterview>();
        cameraManager = GetComponent<CameraManager>();
       // chatm = GetComponent<ChatManager>();
        candidate = new M_Candidate("steve", "chef de projet"); // TODO has to get arguments from previous view
        interview = new P_Interview(candidate, ihm, cameraManager);

        ihm.SetPresenter(interview);
    }

    // Update is called once per frame
    void Update()
    {
        if (!interview.IsOver)
        {
            interview.Launch();
        }
        else
        {
            // TODO : afficher le résultat dans la vue
        }

        /*
        // TEST
        // THIS HAS TO BE MOVED TO VIEW CLASS
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Choix enregistré");
            interview.SetChosenAnswer(1);
        }*/

    }
    
    // TEST
    // THIS HAS TO BE MOVED 
    public static void Affichage(string text) // ne dois pas être statique dans la fonction finale
    {
        Debug.Log(text);
    }
    
    /*
    public static void AffichageRéponses(List<string> questionsToDisplay) // ne dois pas être statique dans la fonction finale
    {
        int i = 0;
        foreach (string questionToDisplay in questionsToDisplay)
        {
            i++;
            Debug.Log(i+". "+questionToDisplay);
        }
    }
    // END TEST
    */

}
