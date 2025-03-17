using UnityEngine;
using GD.MinMaxSlider;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    [SerializeField] AudioClip[] feetSounds;
    [SerializeField][MinMaxSlider(0.1f,2f)] Vector2 pitch = Vector2.one;
    AudioSource source => GetComponent<AudioSource>();
    CharacterController character => GetComponentInParent<CharacterController>();
    Vector3 lastStepPosition, lastPosition;
    float distance;
    bool grounded = false;
    [SerializeField][Range(0.1f,5f)]float stepSize = 0.5f;
    bool moving = false;
    float currentStepSize;
    float normalVolume = 1f;
    
    void Start()
    {
        if (character==null) this.enabled = false;
        if (feetSounds.Length<1) this.enabled = false;
        currentStepSize = stepSize;
        normalVolume = source.volume;
    }

    void Update()
    {
        if (grounded!=character.isGrounded)
        {
            if (character.isGrounded) Landing(); else Air();
        }

        if (grounded && distance>currentStepSize) PlayFootstep(normalVolume);   
    }

    void LateUpdate()
    {
        if (grounded==false) return;
        float travel = (transform.position-lastPosition).magnitude;
        distance += travel;
        if (moving && travel==0) StopMoving();
        if (moving==false && travel>0) StartMoving();
        lastPosition = transform.position;
    }

    void PlayFootstep(float volume)
    {
        distance = 0;
        lastStepPosition = transform.position;
        if (currentStepSize<stepSize) currentStepSize = stepSize;
        source.pitch = Random.Range(pitch.x, pitch.y);
        source.PlayOneShot(feetSounds[Random.Range(0,feetSounds.Length)], volume);
    }

    void Air()
    {
        grounded = false;
    }

    void Landing()
    {
        grounded = true;
        PlayFootstep(normalVolume);
    }

    void StartMoving()
    {
        currentStepSize = stepSize * 0.2f;
        moving = true;
    }

    void StopMoving()
    {
        if (distance>stepSize*0.2f) PlayFootstep(normalVolume*0.8f);
        distance = 0;
        moving = false;
    }


} // SCRIPT END
