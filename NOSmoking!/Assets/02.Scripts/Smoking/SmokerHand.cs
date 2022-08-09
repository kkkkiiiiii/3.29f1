using UnityEngine;

public class SmokerHand : MonoBehaviour
{
    public Transform cigarPivot;
    public JustCigar cigar;
    public Transform rightHand;
    private Animator smokerAnimator;

    public bool isArrive;
    void Start()
    {
        isArrive = false;
        smokerAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        cigar.gameObject.SetActive(true);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (!isArrive)
        {
            cigarPivot.position = smokerAnimator.GetIKPosition(AvatarIKGoal.RightHand);

            smokerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            smokerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

            smokerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
            smokerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
        }
        
    }
}
