﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HRAP;

[RequireComponent(typeof(NavMeshAgent))]
public class AIBehaviour : MonoBehaviour
{
    [Header("AI")]
    public Transform candidatePosition;
    public Transform chair;
    private Vector3 direction;
    private Animator animator;
    private NavMeshAgent agent;
    public bool animationTrigger; // boolean that can activate behavior

    [Header("Goal points variables")]
    public Transform wayPointsList;
    private Transform[] wayPoints;
    public int currentGP = 0;

    // Timing 
    private IEnumerator coroutine;

    //SINGLETON
    public static AIBehaviour aiBehaviourInstance;
    void Awake()
    {
        if (aiBehaviourInstance != null)
        {
            Debug.LogError("More than one AI Behaviour in scene");
            return;
        }
        else
        {
            aiBehaviourInstance = this;
        }
    }

    void Start()
    {
        // Find a reference to the Animator component in Awake since it exists in the scene.
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Get child elements from GoalPoints object
        wayPoints = new Transform[wayPointsList.childCount];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = wayPointsList.GetChild(i);
        }
        this.agent.autoTraverseOffMeshLink = false;
    }
    
    public void MoveTopoint(Vector3 point)
    {
        agent.SetDestination(point);
    }
    
    public void LookAt(Vector3 positionToLookAt)
    {
        this.direction = positionToLookAt - this.transform.position;
        this.direction.y = 0;

        if (this.direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction); // mathematical way to deal with rotation
            Vector3 rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            this.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f); // will just aim in the (x,z) plan
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            MoveTopoint(wayPoints[currentGP].position);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sitting"))
        {
            this.transform.position = chair.position;
        }
        if (CandidateController.candidateControllerInstance.canMove)
        {
            LookAt(candidatePosition.position);
        }
        // if the AI is close to reached target (1f), then she stop "walking" animation
        if (animationTrigger && Vector3.Distance(wayPoints[currentGP].position, this.transform.position) <= 1f)
        {
            currentGP++;
            if (currentGP==1) // if we reached 1st waypoint, play animation sit
            {
                LookAt(wayPoints[currentGP].position);
                animator.SetBool("Turning", true); // temporary Idle animation played for now
                animator.SetBool("Sitting", true);
            }
            CameraManager.cameraManagerinstance.Scale();
            animationTrigger = false;
        }
    }

    public void PlayAnimation(M_Animation animation, float waitingTime) // Display current camera
    {
        switch (animation)
        {
            case M_Animation.ANIM_MARCHE:
                coroutine = WaitAndPlay(waitingTime, "Walking");
                StartCoroutine(coroutine);
                break;
        }
    }
    
    private IEnumerator WaitAndPlay(float waitTime,string animName)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool(animName, true);
        animationTrigger = true;
    }
}

