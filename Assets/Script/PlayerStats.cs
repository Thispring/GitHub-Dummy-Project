using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player의 상태를 나타내는 클래스 입니다.
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float speed;
    public float Speed => speed;
    [SerializeField] private float jumpVelocity;
    public float JumpVelocity => jumpVelocity;


    void Awake()
    {
        speed = 10f;
        jumpVelocity = 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
