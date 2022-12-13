using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewModel : MonoBehaviour
{
	public Avatar avatar;
    // public CharacterModel characterModel;
    public Animator animator;
    public ParticleSystem particleSystem;
    public AudioSource audioSource;
    public AudioClip jumpClip;

    // Start is called before the first frame update
    void Start()
    {
        // characterModel.JumpEvent += OnCharacterModelOnJumpEvent;
        // characterModel.OnGroundEvent += CharacterModelOnOnGroundEvent;
        // characterModel.LandedEvent += CharacterModelOnLandedEvent;

        // characterModel.GetInVehicleEvent += EnableView;
        // characterModel.CryingEvent += Cry;
    }

    /// <summary>
    /// Enable/disable whole view. Mainly for when we get in vehicles
    /// </summary>
    /// <param name="activate"></param>
    void EnableView(bool activate)
    {
        // TODO: Make more intelligent, eg dust shouldn't disappear
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            // Make sure it's not ME, only the children
            if (t != transform)
            {
                t.gameObject.SetActive(!activate);
            }
        }
    }

    void CharacterModelOnLandedEvent()
    {
        audioSource.clip = jumpClip;
        audioSource.Play();
        particleSystem.Emit(10);
    }

    void CharacterModelOnOnGroundEvent(bool onGround)
    {
        animator.SetBool("OnGround", onGround);
    }

    void OnCharacterModelOnJumpEvent()
    {
        animator.SetTrigger("Jump");
        //animator.SetBool("OnGround", false); // CHECK
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("LookDirectionActive", avatar.moveInput.magnitude);

            int velocityMagnitude = (int) (avatar.GetComponent<Rigidbody>().velocity.magnitude * 10f * Time.deltaTime);
            particleSystem.Emit(velocityMagnitude);
    }
}