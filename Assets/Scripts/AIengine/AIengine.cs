﻿using HRAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIengine : MonoBehaviour
{
    private M_Candidate candidate;
    private P_Interview interview;
    private IHMInterview ihm;
    public static string datapath;

    public static Dictionary<string, double> result;

    // Use this for initialization
    void Awake () {
        datapath = Application.dataPath;
        // We initialise model, view and Presenter with name of candidate and his job
        ihm = GetComponent<IHMInterview>();

        candidate = new M_Candidate(ihm.GetName(), ihm.GetPosition());
        interview = new P_Interview(candidate, ihm);

        ihm.SetPresenter(interview);
    }

    private void Start()
    {
        
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
            // TODO : afficher le résultat dans la vue ==> USE interview.getResult()
            ihm.Over();

            // Save candidate in db
            M_DataManager.Instance.AddCandidate(candidate);
            if (interview.candidateAnswers != null)
            {

                foreach (object o in interview.candidateAnswers)
                {
                    Debug.Log(o);
                }
            }
            result = interview.GetResult();
            if (interview.candidateAnswers != null)
            {
                Debug.Log(interview.candidateAnswers.ToString());
            }
        }
    }
}
