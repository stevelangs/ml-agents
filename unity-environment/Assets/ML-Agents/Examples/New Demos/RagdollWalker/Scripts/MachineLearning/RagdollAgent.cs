using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollAgent : Agent
{

    public Ragdoll ragdollPrefab;
    public Ragdoll ragdoll;

    public Light spotLight;

    Vector3 pelvisStartPos;
    Dictionary<Transform, Vector3> posDict;
    Dictionary<Transform, Quaternion> rotDict;
    Vector3 leftLegMuscleChainPreviousVector;
    Vector3 rightLegMuscleChainPreviousVector;
    Vector3 leftArmMuscleChainPreviousVector;
    Vector3 rightArmMuscleChainPreviousVector;
    Vector3 upperChestPreviousRotation;
    Vector3 lowerChestPreviousRotation;
    Vector3 leftFootPreviousRotation;
    Vector3 rightFootPreviousRotation;

    float[] previousActions = new float[24];
    // List<float> previousActions = new List<float>(24);

    public override void InitializeAgent()
    {
        base.InitializeAgent();

        if (ragdoll == null)
            ragdoll = GameObject.Instantiate(ragdollPrefab, transform.position, Quaternion.identity);
        posDict = new Dictionary<Transform, Vector3>();
        rotDict = new Dictionary<Transform, Quaternion>();
        foreach (var t in ragdoll.GetComponentsInChildren<Transform>())
        {
            posDict.Add(t, t.localPosition);
            rotDict.Add(t, t.localRotation);
        }
    }

    internal void AddVectorObs(Vector3 observation, List<float> state)
    {
        state.Add(observation.x);
        state.Add(observation.y);
        state.Add(observation.z);
    }

    void AddVectorObs(float obs, List<float> state)
    {
        state.Add(obs);
    }


    public override void CollectObservations()
    {
        // AddVectorObs(ragdoll.pelvis.transform.localPosition);
        // AddVectorObs(ragdoll.head.transform.localPosition);
        // AddVectorObs(ragdoll.leftUpperLeg.transform.localPosition);
        // AddVectorObs(ragdoll.leftLowerLeg.transform.localPosition);
        // AddVectorObs(ragdoll.leftFoot.transform.localPosition);
        // AddVectorObs(ragdoll.rightUpperLeg.transform.localPosition);
        // AddVectorObs(ragdoll.rightLowerLeg.transform.localPosition);
        // AddVectorObs(ragdoll.rightFoot.transform.localPosition);
        // AddVectorObs(ragdoll.leftUpperArm.transform.localPosition);
        // AddVectorObs(ragdoll.leftLowerArm.transform.localPosition);
        // AddVectorObs(ragdoll.leftHand.transform.localPosition);
        // AddVectorObs(ragdoll.rightUpperArm.transform.localPosition);
        // AddVectorObs(ragdoll.rightLowerArm.transform.localPosition);
        // AddVectorObs(ragdoll.rightHand.transform.localPosition);
        // AddVectorObs(ragdoll.upperChest.transform.localPosition);
        // AddVectorObs(ragdoll.lowerChest.transform.localPosition);

        AddVectorObs(ragdoll.pelvis.transform.position);
        AddVectorObs(ragdoll.head.RelativePosFromPelvis);
        AddVectorObs(ragdoll.leftUpperLeg.RelativePosFromPelvis);
        AddVectorObs(ragdoll.leftLowerLeg.RelativePosFromPelvis);
        AddVectorObs(ragdoll.leftFoot.RelativePosFromPelvis);
        AddVectorObs(ragdoll.rightUpperLeg.RelativePosFromPelvis);
        AddVectorObs(ragdoll.rightLowerLeg.RelativePosFromPelvis);
        AddVectorObs(ragdoll.rightFoot.RelativePosFromPelvis);
        AddVectorObs(ragdoll.leftUpperArm.RelativePosFromPelvis);
        AddVectorObs(ragdoll.leftLowerArm.RelativePosFromPelvis);
        AddVectorObs(ragdoll.leftHand.RelativePosFromPelvis);
        AddVectorObs(ragdoll.rightUpperArm.RelativePosFromPelvis);
        AddVectorObs(ragdoll.rightLowerArm.RelativePosFromPelvis);
        AddVectorObs(ragdoll.rightHand.RelativePosFromPelvis);
        AddVectorObs(ragdoll.upperChest.RelativePosFromPelvis);
        AddVectorObs(ragdoll.lowerChest.transform.localPosition);

        AddVectorObs(ragdoll.pelvis.rigidbody.rotation);
        // AddVectorObs(ragdoll.pelvis.transform.localRotation);
        AddVectorObs(ragdoll.head.transform.localRotation);
        AddVectorObs(ragdoll.leftUpperLeg.transform.localRotation);
        AddVectorObs(ragdoll.leftLowerLeg.transform.localRotation);
        AddVectorObs(ragdoll.leftFoot.transform.localRotation);
        AddVectorObs(ragdoll.rightUpperLeg.transform.localRotation);
        AddVectorObs(ragdoll.rightLowerLeg.transform.localRotation);
        AddVectorObs(ragdoll.rightFoot.transform.localRotation);
        AddVectorObs(ragdoll.leftUpperArm.transform.localRotation);
        // AddVectorObs(ragdoll.leftLowerArm.transform.localRotation);
        // AddVectorObs(ragdoll.leftHand.transform.localRotation);
        AddVectorObs(ragdoll.rightUpperArm.transform.localRotation);
        // AddVectorObs(ragdoll.rightLowerArm.transform.localRotation);
        // AddVectorObs(ragdoll.rightHand.transform.localRotation);
        AddVectorObs(ragdoll.upperChest.transform.localRotation);
        AddVectorObs(ragdoll.lowerChest.transform.localRotation);

        // AddVectorObs(ragdoll.pelvis.rigidbody.rotation);
        // AddVectorObs(ragdoll.head.rigidbody.rotation);
        // AddVectorObs(ragdoll.leftUpperLeg.rigidbody.rotation);
        // AddVectorObs(ragdoll.leftLowerLeg.rigidbody.rotation);
        // AddVectorObs(ragdoll.leftFoot.rigidbody.rotation);
        // AddVectorObs(ragdoll.rightUpperLeg.rigidbody.rotation);
        // AddVectorObs(ragdoll.rightLowerLeg.rigidbody.rotation);
        // AddVectorObs(ragdoll.rightFoot.rigidbody.rotation);
        // AddVectorObs(ragdoll.leftUpperArm.rigidbody.rotation);
        // AddVectorObs(ragdoll.leftLowerArm.rigidbody.rotation);
        // AddVectorObs(ragdoll.leftHand.rigidbody.rotation);
        // AddVectorObs(ragdoll.rightUpperArm.rigidbody.rotation);
        // AddVectorObs(ragdoll.rightLowerArm.rigidbody.rotation);
        // AddVectorObs(ragdoll.rightHand.rigidbody.rotation);
        // AddVectorObs(ragdoll.upperChest.rigidbody.rotation);
        // AddVectorObs(ragdoll.lowerChest.rigidbody.rotation);

        AddVectorObs(ragdoll.pelvis.rigidbody.velocity);
        AddVectorObs(ragdoll.head.rigidbody.velocity);
        AddVectorObs(ragdoll.leftUpperLeg.rigidbody.velocity);
        AddVectorObs(ragdoll.leftLowerLeg.rigidbody.velocity);
        AddVectorObs(ragdoll.leftFoot.rigidbody.velocity);
        AddVectorObs(ragdoll.rightUpperLeg.rigidbody.velocity);
        AddVectorObs(ragdoll.rightLowerLeg.rigidbody.velocity);
        AddVectorObs(ragdoll.rightFoot.rigidbody.velocity);
        // AddVectorObs(ragdoll.leftUpperArm.rigidbody.velocity);
        // AddVectorObs(ragdoll.leftLowerArm.rigidbody.velocity);
        // AddVectorObs(ragdoll.leftHand.rigidbody.velocity);
        // AddVectorObs(ragdoll.rightUpperArm.rigidbody.velocity);
        // AddVectorObs(ragdoll.rightLowerArm.rigidbody.velocity);
        // AddVectorObs(ragdoll.rightHand.rigidbody.velocity);
        AddVectorObs(ragdoll.upperChest.rigidbody.velocity);
        AddVectorObs(ragdoll.lowerChest.rigidbody.velocity);

        AddVectorObs(ragdoll.pelvis.rigidbody.angularVelocity);
        AddVectorObs(ragdoll.head.rigidbody.angularVelocity);
        AddVectorObs(ragdoll.leftUpperLeg.rigidbody.angularVelocity);
        AddVectorObs(ragdoll.leftLowerLeg.rigidbody.angularVelocity);
        AddVectorObs(ragdoll.leftFoot.rigidbody.angularVelocity);
        AddVectorObs(ragdoll.rightUpperLeg.rigidbody.angularVelocity);
        AddVectorObs(ragdoll.rightLowerLeg.rigidbody.angularVelocity);
        AddVectorObs(ragdoll.rightFoot.rigidbody.angularVelocity);
        // AddVectorObs(ragdoll.leftUpperArm.rigidbody.angularVelocity);
        // AddVectorObs(ragdoll.leftLowerArm.rigidbody.angularVelocity);
        // AddVectorObs(ragdoll.leftHand.rigidbody.angularVelocity);
        // AddVectorObs(ragdoll.rightUpperArm.rigidbody.angularVelocity);
        // AddVectorObs(ragdoll.rightLowerArm.rigidbody.angularVelocity);
        // AddVectorObs(ragdoll.rightHand.rigidbody.angularVelocity);
        AddVectorObs(ragdoll.upperChest.rigidbody.angularVelocity);
        AddVectorObs(ragdoll.lowerChest.rigidbody.angularVelocity);

        // AddVectorObs(leftLegMuscleChainPreviousVector);
        // AddVectorObs(rightLegMuscleChainPreviousVector);
        // AddVectorObs(leftArmMuscleChainPreviousVector);
        // AddVectorObs(rightArmMuscleChainPreviousVector);
        // AddVectorObs(upperChestPreviousRotation);
        // AddVectorObs(lowerChestPreviousRotation);
        // AddVectorObs(leftFootPreviousRotation);
        // AddVectorObs(rightFootPreviousRotation);
        AddVectorObs(previousActions);

        // AddVectorObs(ragdoll.pelvis.transform.up);

        // AddVectorObs(ragdoll.pelvis.transform.forward);

        // AddVectorObs(ragdoll.pelvis.rigidbody.velocity);
        // AddVectorObs(ragdoll.pelvis.rigidbody.angularVelocity);

        // AddVectorObs(ragdoll.leftHand.LocalPosInPelvis);
        // AddVectorObs(ragdoll.rightHand.LocalPosInPelvis);

        // AddVectorObs(ragdoll.leftHand.Velocity);
        // AddVectorObs(ragdoll.rightHand.Velocity);

        // AddVectorObs(ragdoll.pelvis.transform.InverseTransformVector(ragdoll.leftHand.Velocity));
        // AddVectorObs(ragdoll.pelvis.transform.InverseTransformVector(ragdoll.rightHand.Velocity));


        // AddVectorObs(ragdoll.head.Height);
        // AddVectorObs(ragdoll.head.transform.up);
        // AddVectorObs(ragdoll.head.Velocity);
        // AddVectorObs(ragdoll.head.rigidbody.angularVelocity);

        // AddVectorObs(ragdoll.head.RelativePosFromPelvis);

        // AddVectorObs(ragdoll.leftFoot.LocalPosInPelvis);
        // AddVectorObs(ragdoll.rightFoot.LocalPosInPelvis);


        AddVectorObs(ragdoll.rightFoot.touchingGround ? 1f : 0f);
        AddVectorObs(ragdoll.leftFoot.touchingGround ? 1f : 0f);
    }
    // public override void CollectObservations()
    // {
    //     AddVectorObs(ragdoll.pelvis.Height);

    //     AddVectorObs(ragdoll.pelvis.transform.up);

    //     AddVectorObs(ragdoll.pelvis.transform.forward);

    //     AddVectorObs(ragdoll.pelvis.rigidbody.velocity);
    //     AddVectorObs(ragdoll.pelvis.rigidbody.angularVelocity);

    //     AddVectorObs(ragdoll.leftHand.LocalPosInPelvis);
    //     AddVectorObs(ragdoll.rightHand.LocalPosInPelvis);

    //     AddVectorObs(ragdoll.leftHand.Velocity);
    //     AddVectorObs(ragdoll.rightHand.Velocity);

    //     AddVectorObs(ragdoll.pelvis.transform.InverseTransformVector(ragdoll.leftHand.Velocity));
    //     AddVectorObs(ragdoll.pelvis.transform.InverseTransformVector(ragdoll.rightHand.Velocity));


    //     AddVectorObs(ragdoll.head.Height);
    //     AddVectorObs(ragdoll.head.transform.up);
    //     AddVectorObs(ragdoll.head.Velocity);
    //     AddVectorObs(ragdoll.head.rigidbody.angularVelocity);

    //     AddVectorObs(ragdoll.head.RelativePosFromPelvis);

    //     AddVectorObs(ragdoll.leftFoot.LocalPosInPelvis);
    //     AddVectorObs(ragdoll.rightFoot.LocalPosInPelvis);


    //     AddVectorObs(ragdoll.rightFoot.touchingGround ? 1f : 0f);
    //     AddVectorObs(ragdoll.leftFoot.touchingGround ? 1f : 0f);
    // }

    public bool useMuscleChain;

    public override void AgentAction(float[] act, string textAction)
    {

        if (useMuscleChain)
        {
            ragdoll.leftLegMuscleChain.SetTargetPos(new Vector3(act[0], act[1], act[2]));
            // leftLegMuscleChainPreviousVector = (new Vector3(act[0], act[1], act[2]));
            ragdoll.rightLefMuscleChain.SetTargetPos(new Vector3(act[3], act[4], act[5]));
            // rightLegMuscleChainPreviousVector = (new Vector3(act[3], act[4], act[5]));

            ragdoll.leftArmMuscle.SetTargetPos(new Vector3(act[6], act[7], act[8]));
            // leftArmMuscleChainPreviousVector = (new Vector3(act[6], act[7], act[8]));
            ragdoll.rightArmMuscle.SetTargetPos(new Vector3(act[9], act[10], act[11]));
            // rightArmMuscleChainPreviousVector = (new Vector3(act[6], act[7], act[8]));

            ragdoll.upperChest.SetNormalizedTargetRotation(act[12], act[13], act[14]);
            // upperChestPreviousRotation = 
            ragdoll.lowerChest.SetNormalizedTargetRotation(act[15], act[16], act[17]);

            ragdoll.leftFoot.SetNormalizedTargetRotation(act[18], act[19], act[20]);
            ragdoll.rightFoot.SetNormalizedTargetRotation(act[21], act[22], act[23]);
            // var newList = new List<float>(){act[0], act[1], act[2], act[3], act[4], act[5], act[6], act[7], act[8], act[9], [10], [11], [12], [13], [14], [15], [16], [17], [18], [19], [20], [21], [22], [23]};
            // List<float> previous = new List<float>(){act[0], act[1], act[2], act[3], act[4], act[5], act[6], act[7], act[8], act[9], act[10], act[11], act[12], act[13], act[14], act[15], act[16], act[17], act[18], act[19], act[20], act[21], act[22], act[23]};
            float[] acts = new float[]{act[0], act[1], act[2], act[3], act[4], act[5], act[6], act[7], act[8], act[9], act[10], act[11], act[12], act[13], act[14], act[15], act[16], act[17], act[18], act[19], act[20], act[21], act[22], act[23]};
            previousActions = acts;
            // previousActions = new List<float>(24);
            // previousActions.AddRange(acts);
            
            
            // AddVectorObs(previousActions);
        }
        else
        {
            ragdoll.leftUpperLeg.SetNormalizedTargetRotation(act[0], act[1], act[2]);
            ragdoll.leftLowerLeg.SetNormalizedTargetRotation(act[3], act[4], act[5]);

            ragdoll.rightUpperLeg.SetNormalizedTargetRotation(act[6], act[7], act[8]);
            ragdoll.rightLowerLeg.SetNormalizedTargetRotation(act[9], act[10], act[11]);

            ragdoll.leftUpperArm.SetNormalizedTargetRotation(act[12], act[13], act[14]);
            ragdoll.leftLowerArm.SetNormalizedTargetRotation(act[15], act[16], act[17]);

            ragdoll.rightUpperArm.SetNormalizedTargetRotation(act[18], act[19], act[20]);
            ragdoll.rightLowerArm.SetNormalizedTargetRotation(act[21], act[22], act[23]);

            ragdoll.upperChest.SetNormalizedTargetRotation(act[24], act[25], act[26]);
            ragdoll.lowerChest.SetNormalizedTargetRotation(act[27], act[28], act[29]);

            ragdoll.leftFoot.SetNormalizedTargetRotation(act[30], act[31], act[32]);
            ragdoll.rightFoot.SetNormalizedTargetRotation(act[33], act[34], act[35]);
        }

        //if (float.IsNaN(ragdoll.head.Height) || float.IsInfinity(ragdoll.head.Height) || ragdoll.head.Height > 5f || ragdoll.head.Height < 1f)
        //{
        //    reward = -1f;
        //    done = true;
        //}
        //else
        {





            AddReward(((ragdoll.head.Height - 1.2f) + ragdoll.head.transform.up.y * 0.1f)/100);

            if (ragdoll.upperChest.touchingGround || ragdoll.lowerChest.touchingGround || ragdoll.head.touchingGround || ragdoll.head.Height < 1.2f)
            {
                AddReward(-1f);
                if (Application.isEditor)
                    print(GetCumulativeReward());
                Done();
            }



        }




    }
    public override void AgentReset()
    {

        if (ragdoll != null)
        {
            foreach (var t in ragdoll.GetComponentsInChildren<Transform>())
            {
                t.localPosition = posDict[t];
                t.localRotation = rotDict[t];
                if (t.GetComponent<Rigidbody>() != null)
                {
                    t.GetComponent<Rigidbody>().velocity = t.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
            }
        }

        if (spotLight != null)
        {
            spotLight.transform.LookAt(transform.position + Random.insideUnitSphere * 2f, transform.forward);
            spotLight.intensity = Random.Range(5f, 45f);
            spotLight.color = new Color(Random.value, Random.value, Random.value);
            spotLight.spotAngle = Random.Range(20f, 60f);
        }

    }

    public override void AgentOnDone()
    {

    }


}
