using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HumanBone 
{
    public HumanBodyBones bone;
    public float weight = 1f;
}



public class Weapon_IK : MonoBehaviour
{

    public Transform target_Transform;
    public Transform aim_Transform;


    public int iteration = 2;

    [Range(0, 1)]
    public float weight = 1f;

    public HumanBone[] humanBones;
    Transform[] boneTransforms;

    public float Angle_limit = 90f;
    

    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = anim.GetBoneTransform(humanBones[i].bone);

        }
    }

    Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = target_Transform.position - aim_Transform.position;
        Vector3 aimDirection = aim_Transform.forward;
        float blendout = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        Debug.Log(targetAngle);
        //if (targetAngle > Angle_limit)
        //{
           
        //    blendout += (targetAngle - Angle_limit) / 50f;
        //}

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendout);
        return aim_Transform.position + direction;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 target_Position = GetTargetPosition();
        for (int i = 0; i < iteration; i++)

        {
            for (int b = 0; b < boneTransforms.Length; b++)
            {

                Transform bone = boneTransforms[b];
                float boneWeight = humanBones[b].weight * weight;
            AimAtTarget(bone, target_Position, boneWeight);
            }
        }
      
        
    }

    void AimAtTarget(Transform bone, Vector3 target_Position, float weight)
    {
        Vector3 aimDirection = aim_Transform.forward;
        Vector3 target_Direction = target_Position - aim_Transform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, target_Direction);
        Quaternion blenededRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight); 
        bone.rotation = blenededRotation * bone.rotation;
    }
}
