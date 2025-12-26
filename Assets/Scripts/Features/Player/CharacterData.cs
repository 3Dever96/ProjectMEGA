using UnityEngine;

public class CharacterData : MonoBehaviour
{
    PlayerController player;

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
