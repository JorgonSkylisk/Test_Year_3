using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Dash : MonoBehaviour
{
    [Tooltip("The strength with which the Dash pushes the player up")]
    public float DashAcceleration = 7f;
    [Range(0f, 1f)]
    [Tooltip("This will affect how much using the Dash will cancel the gravity value, to start going up faster. 0 is not at all, 1 is instant")]
    public float DashDownwardVelocityCancelingFactor = 1f;

    [Header("Durations")]
    [Tooltip("Time it takes to consume all the Dash fuel")]
    public float consumeDuration = 0.5f;
    [Tooltip("Time it takes to completely refill the Dash while on the ground")]
    public float refillDurationGrounded = 2f;
    [Tooltip("Time it takes to completely refill the Dash while in the air")]
    public float refillDurationInTheAir = 5f;
    [Tooltip("Delay after last use before starting to refill")]
    public float refillDelay = 1f;


    bool m_CanUseDash;
    PlayerCharacterController m_PlayerCharacterController;
    PlayerInputHandler m_InputHandler;
    float m_LastTimeOfUse;
    float dashTime = 0;

    // stored ratio for Dash resource (1 is full, 0 is empty)
    public float currentFillRatio { get; private set; }
    //public bool isDashUnlocked { get; private set; }

    //public bool isPlayergrounded() => m_PlayerCharacterController.isGrounded;

    //public UnityAction<bool> onUnlockDash;

    void Start()
    {

        m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, Dash>(m_PlayerCharacterController, this, gameObject);

        m_InputHandler = GetComponent<PlayerInputHandler>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, Dash>(m_InputHandler, this, gameObject);

        currentFillRatio = 1f;


    }

    void Update()
    {
        // Dash can only be used if not grounded and jump has been pressed again once in-air
        /*if (isPlayergrounded())
        {
            m_CanUseDash = true;
        }
        else if (!m_PlayerCharacterController.hasJumpedThisFrame && m_InputHandler.GetJumpInputDown())
        {
            m_CanUseDash = true;
        }
        */
        m_CanUseDash = true;
        // Dash usage
        bool DashIsInUse = m_CanUseDash && currentFillRatio > 0f && m_InputHandler.GetSprintInputHeld();
        if (DashIsInUse)
        {
            // store the last time of use for refill delay
            m_LastTimeOfUse = Time.time;

            float totalAcceleration = DashAcceleration;

            // cancel out gravity
            totalAcceleration += m_PlayerCharacterController.gravityDownForce;

            //if (m_PlayerCharacterController.characterVelocity.y < 0f)
            
                // handle making the Dash compensate for character's downward velocity with bonus acceleration
                //totalAcceleration += ((-m_PlayerCharacterController.characterVelocity.y / Time.deltaTime) * DashDownwardVelocityCancelingFactor);
            

            // apply the acceleration to character's velocity
            m_PlayerCharacterController.characterVelocity += new Vector3(m_PlayerCharacterController.characterVelocity.x, 0, m_PlayerCharacterController.characterVelocity.z) * totalAcceleration * Time.deltaTime;

            // consume fuel
            currentFillRatio = currentFillRatio - (Time.deltaTime / consumeDuration);
            dashTime += Time.deltaTime;
        }
        else
        {
            // refill the meter over time
            if (Time.time - m_LastTimeOfUse >= refillDelay)
            {
                float refillRate = 1 / (m_PlayerCharacterController.isGrounded ? refillDurationGrounded : refillDurationInTheAir);
                currentFillRatio = currentFillRatio + Time.deltaTime * refillRate;
            }
            //m_PlayerCharacterController.t = new Vector3(0, 0, 0);

            // keeps the ratio between 0 and 1
            currentFillRatio = Mathf.Clamp01(currentFillRatio);

        }

        if(dashTime == 0.25f)
        {
            m_PlayerCharacterController.characterVelocity = new Vector3(0, 0, 0);
            DashIsInUse = false;
        }

    }

}
