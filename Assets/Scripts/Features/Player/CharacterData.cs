using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public float StepDistance
    {
        get { return stepDistance; }
    }

    public float StepTime
    {
        get { return stepTime; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    public float ActionSpeed
    {
        get { return actionSpeed; }
    }

    public float JumpSpeed
    {
        get { return jumpSpeed; }
    }

    public float Gravity
    {
        get { return gravity; }
    }

    public float StickForce
    {
        get { return stickForce; }
    }

    public float TerminalVelocity
    {
        get { return terminalVelocity; }
    }

    public float SlideSpeed
    {
        get { return slideSpeed; }
    }

    public bool HasDoubleJump
    {
        get { return hasDoubleJump; }
    }

    public bool HasAirDash
    {
        get { return hasAirDash; }
    }

    PlayerController player;

    [Header("Step")]
    [SerializeField] float stepDistance = 0.0625f;
    [SerializeField] float stepTime = 0.11f;

    [Header("Movement Speed")]
    [SerializeField] float moveSpeed = 5.15f;
    [SerializeField] float actionSpeed = 9.375f;
    [SerializeField] float jumpSpeed = 18.75f;
    [SerializeField] float gravity = -54f;
    [SerializeField] float stickForce = -5f;
    [SerializeField] float terminalVelocity = -26.25f;
    [SerializeField] float slideSpeed = -2f;

    [Header("Action Bools")]
    [SerializeField] bool hasDoubleJump;
    [SerializeField] bool hasAirDash;

    PlayerGroundState groundState;
    PlayerAirState airState;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();

        groundState = GetComponent<PlayerGroundState>();
        airState = GetComponent<PlayerAirState>();

        player.GetCharacterStates(groundState, airState);
    }
}
