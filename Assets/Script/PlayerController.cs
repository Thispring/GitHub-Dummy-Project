using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player의 행동을 제어하는 클래스 입니다.
public class PlayerController : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Move()
    {
        float h = 0f, v = 0f;
        if (Input.GetKey(KeyCode.A)) h -= 1f;
        if (Input.GetKey(KeyCode.D)) h += 1f;
        if (Input.GetKey(KeyCode.W)) v += 1f;
        if (Input.GetKey(KeyCode.S)) v -= 1f;

        Vector3 dir = new Vector3(h, 0f, v);
        if (dir.sqrMagnitude < 0.0001f) return;

        dir = dir.normalized;
        float speed = playerStats.Speed;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
