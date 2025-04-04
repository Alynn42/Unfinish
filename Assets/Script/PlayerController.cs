using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float jumpPower = 4f;
    public float gravity = 10f;
    private Vector3 playerPos;
    private bool posPlayerPuzzle1 = false;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    private Vector3 CurrentPos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Vector3 currentRotation
    {
        get { return transform.eulerAngles; }
        set { transform.eulerAngles = value; }
    }



    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0f;

    public bool canMove = true;

    private CharacterController characterController;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    // Audio
    public AudioSource audioSource; // AudioSource component
    public AudioClip walkClip;      // Walking sound effect
    public AudioClip runClip;       // Running sound effect
    public AudioClip jumpClip;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool wasRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        // Ensure audio source is attached
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //LShift = Run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Play walking or running sound based on movement
        if (curSpeedX != 0 || curSpeedY != 0) // If there is movement
        {
            if (characterController.isGrounded)
            {
                if (isRunning && !audioSource.isPlaying) // Play running sound if running
                {
                    audioSource.clip = runClip;
                    audioSource.Play();
                }
                else if (isRunning && !wasRunning) // Player is running and wasn't running before
                {
                    if (audioSource.clip != runClip) // Only change sound if it's not already running
                    {
                        audioSource.clip = runClip;
                        audioSource.Play();
                    }
                    wasRunning = true;
                }
                else if (!isRunning && wasRunning) // Player was running and now is walking
                {
                    if (audioSource.clip != walkClip) // Only change sound if it's not already walking
                    {
                        audioSource.clip = walkClip;
                        audioSource.Play();
                    }
                    wasRunning = false;
                }
                else if (!isRunning && !audioSource.isPlaying) // Play walking sound if walking
                {
                    audioSource.clip = walkClip;
                    audioSource.Play();
                }
            }
        }
        else if (audioSource.isPlaying) // Stop sound if not moving
        {
            audioSource.Stop();
        }

        #endregion

        #region Handles Jumping
        if (Input.GetKey(jumpKey) && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
            audioSource.clip = jumpClip;
            audioSource.Play();
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

        playerPos = transform.position;

        if (!posPlayerPuzzle1)
        {
            posPlayerPuzzle1 = playerPos.x > 78.8 && playerPos.x <= 81 && playerPos.z > 11.5 && playerPos.z <= 12.5;
            Debug.Log(posPlayerPuzzle1);
        }
        if (posPlayerPuzzle1)
        {
            if( playerPos.x <= 78.8 && playerPos.x > 81 && playerPos.z < 11.5 && playerPos.z > 12.5)
            {
                posPlayerPuzzle1 = false;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                CurrentPos = transform.position;
                currentRotation = transform.eulerAngles;
                SceneManager.LoadScene("Sokuban01", LoadSceneMode.Additive);

            }

        }
    }

}
