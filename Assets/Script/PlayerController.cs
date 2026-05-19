using System.Collections;
using UnityEngine;

// Player의 행동을 제어하는 클래스 입니다.
public class PlayerController : MonoBehaviour
{ 
    private PlayerStats playerStats;
    private float jumpDuration = 0.5f; // 전체 점프 시간 (상승+하강)
    private bool isJumping = false;
    private float lastFacing = 1f; // 1 = right, -1 = left

    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //if (Input.GetKeyDown(KeyCode.Space)) Jump();
        ///if (Input.GetKeyDown(KeyCode.P)) SpawnProjectile();
    }

    private void Move()
    {
        float h = 0f, v = 0f;
        if (Input.GetKey(KeyCode.A)) h -= 1f;
        if (Input.GetKey(KeyCode.D)) h += 1f;

        if (Mathf.Abs(h) > 0.0001f)
        {
            lastFacing = Mathf.Sign(h);
            float yRot = (lastFacing >= 0f) ? 180f : 0f;
            Vector3 e = transform.eulerAngles;
            e.y = yRot;
            transform.eulerAngles = e;
        }

        Vector3 dir = new Vector3(h, 0f, v);
        if (dir.sqrMagnitude < 0.0001f) return;

        dir = dir.normalized;
        float speed = playerStats.Speed;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }

    private void Jump()
    {
        if (isJumping) return;
        StartCoroutine(JumpRoutine());
    }

    private Vector3 GetInputDirection()
    {
        float h = 0f, v = 0f;
        if (Input.GetKey(KeyCode.A)) h -= 1f;
        if (Input.GetKey(KeyCode.D)) h += 1f;
        if (Input.GetKey(KeyCode.W)) v += 1f;
        if (Input.GetKey(KeyCode.S)) v -= 1f;

        Vector3 dir = new Vector3(h, 0f, v);
        if (dir.sqrMagnitude < 0.0001f) return Vector3.zero;
        return dir.normalized;
    }

    private void SpawnProjectile()
    {
        if (playerStats.ProjectTile == null) return;

        Vector3 inputDir = GetInputDirection();
        Vector3 fireDir;
        if (inputDir == Vector3.zero)
        {
            fireDir = (lastFacing >= 0f) ? Vector3.right : Vector3.left;
        }
        else
        {
            fireDir = (inputDir.x >= 0f) ? Vector3.right : Vector3.left;
        }

        Vector3 spawnPos = transform.position + Vector3.up * 1f + fireDir * 0.6f;
        GameObject go = Instantiate(playerStats.ProjectTile, spawnPos, Quaternion.identity);
        Projectile proj = go.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Setup(fireDir);
        }
    }

    private IEnumerator JumpRoutine()
    {
        isJumping = true;

        float originalY = transform.position.y;
        float targetY = originalY + playerStats.JumpVelocity;

        float half = Mathf.Max(0.001f, jumpDuration * 0.5f);
        float elapsed = 0f;

        // 상승
        while (elapsed < half)
        {
            float t = elapsed / half;
            float eased = Mathf.SmoothStep(0f, 1f, t);
            float y = Mathf.Lerp(originalY, targetY, eased);
            Vector3 p = transform.position;
            p.y = y;
            transform.position = p;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 정확히 최상단 위치 보정
        Vector3 top = transform.position;
        top.y = targetY;
        transform.position = top;

        // 하강
        elapsed = 0f;
        while (elapsed < half)
        {
            float t = elapsed / half;
            float eased = Mathf.SmoothStep(0f, 1f, t);
            float y = Mathf.Lerp(targetY, originalY, eased);
            Vector3 p = transform.position;
            p.y = y;
            transform.position = p;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 원래 위치 보정
        Vector3 finalP = transform.position;
        finalP.y = originalY;
        transform.position = finalP;

        isJumping = false;
    }
}
