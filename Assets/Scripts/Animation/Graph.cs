﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HRAP;

public class Graph : MonoBehaviour
{
    [Header("Graph variables")]
    public Transform pointPrefabAxis; // object used to represent axis points
    public Transform pointPrefabCandidate; // object used to represent candidate points
    public float size; // size of axis points
    private float radius; // radius of axis points
    public Color[] cols = new Color[9]; // differents colors for axis

    // Axis Values
    private int nbQualities;// Number of qualities
    Dictionary<int, List<Transform>> axis = new Dictionary<int, List<Transform>>(); // Dictionnary of each axis with points
    private float[] radianList;
    private double[] qualities = new double[9];
    private Transform[] qualitiesPoints = new Transform[9];
    private int lenght = 60; // better to be a multiple of 10

    // Material used for the connecting lines
    public Material lineMat;
    
    // Fill in this with the default Unity Cylinder mesh
    // We will account for the cylinder pivot/origin being in the middle.
    public Mesh cylinderMesh;
    
    GameObject[] ringGameObjects;
    
    //SINGLETON
    public static Graph graphInstance;
    void Awake()
    {
        if (graphInstance != null)
        {
            Debug.LogError("More than one graph in scene");
            return;
        }
        else
        {
            graphInstance = this;
        }

        // list of different radian angles of each axis
        radianList = new float[] { 0f, 0.689f, 1.396f, 2.094f, 2.792f, 3.490f, 4.188f, 4.886f, 5.585f };
        nbQualities = radianList.Length;
        this.radius = pointPrefabAxis.GetComponent<SphereCollider>().radius;
        Vector3 position;
        position.z = 0f;
        Transform point;
        List<Transform> points = new List<Transform>();

        // Create a line of points for each quality axis
        for (int i = 0; i < nbQualities; i++)
        {
            points = new List<Transform>();
            for (int j = 0; j < lenght; j++) // The line is composed of "length" points
            {
                point = Instantiate(pointPrefabAxis);
                Material mymat = point.GetComponent<Renderer>().material;
                position.x = j * radius * Mathf.Cos(radianList[i]);
                position.y = j * radius * Mathf.Sin(radianList[i]);
                mymat.SetColor("_EmissionColor", cols[i]);
                point.localPosition = position;
                points.Add(point);
            }
            axis.Add(i, points);
        }
        
        // initializing candidates points 
        if(null!=AIengine.aiEngine.result)
            GetResult(AIengine.aiEngine.result);

        // Placing candidate points, candidate points are rated /3
        for (int i = 0; i < nbQualities; i++)
        {
            if (axis.TryGetValue(i, out points)) // If the data exist in the dictionary with the given key
            {
                Transform candidatePoint = Instantiate(pointPrefabCandidate);
                int index = (int)(qualities[i] * lenght / 3);
                position.x = points[index].localPosition.x; 
                position.y = points[index].localPosition.y;
                candidatePoint.localPosition = position;
                qualitiesPoints[i] = candidatePoint;
            }
            else
            {
                Debug.Log("Error, empty key value in dictionnary");
            }
        }

    }

    public void GetResult(List<V_Competence> list)
    {
        // initializing candidates points 

        for (int i = 0; i < qualitiesPoints.Length; i++)
        {
            qualities[i] = list[i].Points;
        }
    }

    // Use this for initialization
    // Link all the points together with lines
    void Start()
    {
        this.ringGameObjects = new GameObject[qualitiesPoints.Length];
        //this.connectingRings = new ProceduralRing[points.Length];
        for (int i = 0; i < qualitiesPoints.Length; i++)
        {
            // Make a gameobject that we will put the ring on
            // And then put it as a child on the gameobject that has this Command and Control script
            this.ringGameObjects[i] = new GameObject();
            this.ringGameObjects[i].name = "Connecting ring #" + i;
            this.ringGameObjects[i].transform.parent = this.gameObject.transform;

            // We make a offset gameobject to counteract the default cylindermesh pivot/origin being in the middle
            GameObject ringOffsetCylinderMeshObject = new GameObject();
            ringOffsetCylinderMeshObject.transform.parent = this.ringGameObjects[i].transform;

            // Offset the cylinder so that the pivot/origin is at the bottom in relation to the outer ring gameobject.
            ringOffsetCylinderMeshObject.transform.localPosition = new Vector3(0f, 1f, 0f);
            // Set the radius
            ringOffsetCylinderMeshObject.transform.localScale = new Vector3(radius, 1f, radius);

            // Create the the Mesh and renderer to show the connecting ring
            MeshFilter ringMesh = ringOffsetCylinderMeshObject.AddComponent<MeshFilter>();
            ringMesh.mesh = this.cylinderMesh;

            MeshRenderer ringRenderer = ringOffsetCylinderMeshObject.AddComponent<MeshRenderer>();
            ringRenderer.material = lineMat;

        }
    }

    // Update is called once per frame
    void Update()
    {
        float cylinderDistance = 0;
        for (int i = 0; i < nbQualities - 1; i++)
        {
            // Move the ring to the point
            this.ringGameObjects[i].transform.position = this.qualitiesPoints[i].transform.position;

            // Match the scale to the distance
            cylinderDistance = 0.5f * Vector3.Distance(this.qualitiesPoints[i].transform.position, this.qualitiesPoints[i + 1].transform.position);
            this.ringGameObjects[i].transform.localScale = new Vector3(this.ringGameObjects[i].transform.localScale.x, cylinderDistance, this.ringGameObjects[i].transform.localScale.z);

            // Make the cylinder look at the main point.
            // Since the cylinder is pointing up(y) and the forward is z, we need to offset by 90 degrees.
            this.ringGameObjects[i].transform.LookAt(this.qualitiesPoints[i + 1].transform, Vector3.up);
            this.ringGameObjects[i].transform.rotation *= Quaternion.Euler(90, 0, 0);
        }

        // Draw last line

        // Move the ring to the point
        this.ringGameObjects[nbQualities - 1].transform.position = this.qualitiesPoints[nbQualities - 1].transform.position;

        // Match the scale to the distance
        cylinderDistance = 0.5f * Vector3.Distance(this.qualitiesPoints[nbQualities - 1].transform.position, this.qualitiesPoints[0].transform.position);
        this.ringGameObjects[nbQualities - 1].transform.localScale = new Vector3(this.ringGameObjects[nbQualities - 1].transform.localScale.x, cylinderDistance, this.ringGameObjects[0].transform.localScale.z);

        // Make the cylinder look at the main point.
        // Since the cylinder is pointing up(y) and the forward is z, we need to offset by 90 degrees.
        this.ringGameObjects[nbQualities - 1].transform.LookAt(this.qualitiesPoints[0].transform, Vector3.up);
        this.ringGameObjects[nbQualities - 1].transform.rotation *= Quaternion.Euler(90, 0, 0);
    }
}
