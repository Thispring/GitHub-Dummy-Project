using UnityEngine;

// Player의 상태를 나타내는 클래스 입니다.
public class PlayerStats : MonoBehaviour
{
    [Header("플레이어 이동속도")]
    [SerializeField] private float speed = 5f;
    public float Speed => speed;
    
    [Header("플레이어 점프력")]
    [SerializeField] private float jumpVelocity = 2f;
    public float JumpVelocity => jumpVelocity;

    [Header("플레이어 투사체")]
    [SerializeField] private GameObject projectTile = null;
    public GameObject ProjectTile => projectTile;

}
