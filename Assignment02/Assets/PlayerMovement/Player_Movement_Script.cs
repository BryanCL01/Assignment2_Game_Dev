using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMovement_Script : MonoBehaviour
{
    public float speed = 12f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float jumpHeight = 3f;
    public LayerMask groundMask;
    public CharacterController controller;

    public AudioSource footstepsSource;
    public AudioClip[] footstepClips;  // Array of footstep sound clips
    public float footstepInterval = 0.5f;

    public AudioSource musicSource;
    public AudioClip dayMusicClip; 
    public AudioClip nightMusicClip; 
    public AudioSource wallCollisionSource; // AudioSource for wall collision
    public AudioClip wallCollisionClip;    // AudioClip to play on collision

    public LightingManager dayNightCycle;
    public FogController fogController;

    Player_InputActions inputActions;
    InputAction movement;
    InputAction jump;
    InputAction reset;
    InputAction toggleCollision;
    InputAction toggleMusic;
    [SerializeField] private Transform player;       
    [SerializeField] private Transform enemy;         
    [SerializeField] private float maxDistance = 20f; // Maximum distance to affect volume
    [SerializeField] private float minVolume = 0.2f; 
    [SerializeField] private float maxVolume = 1.5f; 
    bool isGrounded;
    bool isCollisionActive = true;

    Vector3 originalSpawnPoint;
    Vector3 velocity;

    // Flags to manage collision sound
    private bool isCollidingWithWall = false;  // Flag to track wall collision
    private bool isNearWall = false;
    private bool isMusicPlaying = true;
    private bool lastIsDay = true;
    private bool lastIsFoggy = false;

    void Awake()
    {
        inputActions = new Player_InputActions();
        originalSpawnPoint = transform.position;
    }

    void OnEnable()
    {
        movement = inputActions.Player.Movement;
        jump = inputActions.Player.Jump;
        toggleCollision = inputActions.Player.ToggleCollision;
        reset = inputActions.Player.Reset;
        toggleMusic = inputActions.Player.ToggleMusic;

        movement.Enable();
        jump.Enable();
        toggleCollision.Enable();
        reset.Enable();
        toggleMusic.Enable();

        jump.performed += DoJump;
        toggleCollision.performed += ToggleCollision;
        reset.performed += ResetPosition;
        toggleMusic.performed += ToggleMusicAudio;
    }

    void OnDisable()
    {
        movement.Disable();
        jump.Disable();
        toggleCollision.Disable();
        reset.Disable();
        toggleMusic.Disable();
    }

    void FixedUpdate()
    {
        UpdateMusicBasedOnDayNightAndFog();
        updateMusicByDistance();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 v2 = movement.ReadValue<Vector2>();
        Vector3 move = transform.right * v2.x + transform.forward * v2.y;

        // Update flag based on player movement
        isNearWall = false; // Reset the wall proximity flag each frame
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f)) // Check if player is near a wall
        {
            if (hit.collider.CompareTag("Wall"))
            {
                isNearWall = true;  // Player is near the wall
            }
        }

        if (move.magnitude > 0 && isGrounded)
        {
            PlayFootsteps();
        }
        else
        {
            StopFootsteps();
        }

        if (isCollisionActive)
        {
            if (controller.enabled)
            {
                controller.Move(move * speed * Time.deltaTime);
                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime);
            }
        }
        else
        {
            Vector3 newPosition = transform.position + (move * speed * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            if (controller.enabled)
            {
                controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
            }
            transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        }

        // Stop wall collision sound if player is not near a wall anymore
        if (!isNearWall)
        {
            isCollidingWithWall = false; // Reset flag when player moves away from the wall
        }
    }

    void UpdateMusicBasedOnDayNightAndFog()
    {
        if (dayNightCycle == null) return;

        // Update music clip if isDay has changed
        if (dayNightCycle.isDay != lastIsDay)
        {
            lastIsDay = dayNightCycle.isDay;

            if (dayNightCycle.isDay)
            {
                musicSource.clip = dayMusicClip;
            }
            else
            {
                musicSource.clip = nightMusicClip;
            }

            if (isMusicPlaying)
            {
                musicSource.Play();
            }
        }

        // Adjust volume if isFoggy has changed
        if (fogController.isFoggy != lastIsFoggy)
        {
            lastIsFoggy = fogController.isFoggy;
            musicSource.volume = fogController.isFoggy ? 0.5f : 1f;
        }
    }

    void DoJump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ToggleCollision(InputAction.CallbackContext obj)
    {
        isCollisionActive = !isCollisionActive;
        controller.enabled = isCollisionActive;
    }

    void ResetPosition(InputAction.CallbackContext obj)
    {
        controller.enabled = false;
        transform.position = originalSpawnPoint;
        velocity = Vector3.zero;
        controller.enabled = true;
    }


    void PlayFootsteps()
    {
        if (!footstepsSource.isPlaying)
        {
            footstepsSource.clip = footstepClips[Random.Range(0, footstepClips.Length)];
            footstepsSource.loop = true; // Enable looping
            footstepsSource.Play();
        }
    }

    void StopFootsteps()
    {
        footstepsSource.Pause(); // Pause the looping playback
    }

    // Handle collision with objects (detect if player hits the wall)
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the collided object has the "Wall" tag
        if (hit.collider.CompareTag("Wall"))
        {
            // Play the collision sound only once
            if (!isCollidingWithWall && wallCollisionSource != null && wallCollisionClip != null)
            {
                wallCollisionSource.PlayOneShot(wallCollisionClip); // Play the collision sound
                isCollidingWithWall = true; // Set flag to prevent repeat play during the same collision
            }
        }
    }
    private void updateMusicByDistance()
    {
        float distance = Vector3.Distance(player.position, enemy.position);
        float volumeFactor = Mathf.Clamp01(1 - (distance / maxDistance));
        float targetVolume = Mathf.Lerp(minVolume, maxVolume, volumeFactor);

        musicSource.volume = Mathf.Lerp(musicSource.volume, targetVolume, Time.deltaTime * 5f);
    }
        // Toggle music on and off when "M" key is pressed
    void ToggleMusicAudio(InputAction.CallbackContext obj)
    {
        if (isMusicPlaying)
        {
            musicSource.Pause();  // Pause the music
        }
        else
        {
            musicSource.Play();   // Play the music
        }
        isMusicPlaying = !isMusicPlaying; // Toggle the state
    }
}
