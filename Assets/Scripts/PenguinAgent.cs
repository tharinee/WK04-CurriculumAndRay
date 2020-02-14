﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PenguinAgent : Agent
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;
    public GameObject heartPrefab;
    public GameObject regurgitatedFishPrefab;

    private PenguinArea penguinArea;
    private PenguinAcademy penguinAcademy;
    new private Rigidbody rigidbody;
    private GameObject baby;

    private bool isFull; // If ture, penguin has a full stosach
    private float feedRadius = 0f;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        penguinArea = GetComponentInParent<PenguinArea>();
        penguinAcademy = FindObjectOfType<PenguinAcademy>();
        baby = penguinArea.penguinBaby;
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void AgentAction(float[] vectorAction)
    {
        float forwardAmount = vectorAction[0];
        float turnAmount = 0f;
        if (vectorAction[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            turnAmount = 1f;
        }

        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        // Apply a tiny negative reward every step to encourage action
        AddReward(-1f / agentParameters.maxStep);
    }

    public override float[] Heuristic()
    {
        float forwardAction = 0f;
        float turnAction = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            forwardAction = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // turn left
            turnAction = 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // turn right
            turnAction = 2f;
        }

        // Put the actions into an array and return
        return new float[] { forwardAction, turnAction };
    }
    public override void AgentReset()
    {
        isFull = false;
        penguinArea.ResetArea();
        feedRadius = penguinAcademy.FeedRadius;
    }


    public override void CollectObservations()
    {
        // Whether the penguin has eaten a fish (1 float = 1 value)
        AddVectorObs(isFull);

        // Distance to the baby (1 float = 1 value)
        AddVectorObs(Vector3.Distance(baby.transform.position, transform.position));

        // Direction to baby (1 Vector3 = 3 values)
        AddVectorObs((baby.transform.position - transform.position).normalized);

        // Direction penguin is facing (1 Vector3 = 3 values)
        AddVectorObs(transform.forward);

        // 1 + 1 + 3 + 3 = 8 total values
    }

    private void FixedUpdate()
    {
        // Test if the agent is close enough to to feed the baby
        if (Vector3.Distance(transform.position, baby.transform.position) < feedRadius)
        {
            // Close enough, try to feed the baby
            RegurgitateFish();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("fish"))
        {
            // Try to eat the fish
            EatFish(collision.gameObject);
        }
        else if (collision.transform.CompareTag("baby"))
        {
            // Try to feed the baby
            RegurgitateFish();
        }
    }


    private void EatFish(GameObject fishObject)
    {
        if (isFull) return; // Can't eat another fish while full
        isFull = true;

        penguinArea.RemoveSpecificFish(fishObject);

        AddReward(1f);
    }

    private void RegurgitateFish()
    {
        if (!isFull) return; // Nothing to regurgitate
        isFull = false;

        // Spawn regurgitated fish
        GameObject regurgitatedFish = Instantiate<GameObject>(regurgitatedFishPrefab);
        regurgitatedFish.transform.parent = transform.parent;
        regurgitatedFish.transform.position = baby.transform.position;
        Destroy(regurgitatedFish, 4f);

        // Spawn heart
        GameObject heart = Instantiate<GameObject>(heartPrefab);
        heart.transform.parent = transform.parent;
        heart.transform.position = baby.transform.position + Vector3.up;
        Destroy(heart, 4f);

        AddReward(1f);

        if (penguinArea.FishRemaining <= 0)
        {
            Done();
        }
}


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
