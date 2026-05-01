using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("투사체 데미지")]
    [SerializeField] private float damage = 0f;
    public float Damage => damage;

    [Header("투사체 이동속도")]
    [SerializeField] private float speed = 5f;
    private float lifetime = 2f;

    // 호출 시 발사 방향을 받아 해당 방향으로 일정 시간 이동 후 파괴합니다.
    public void Setup(Vector3 direction)
    {
        StartCoroutine(MoveAndDestroy(direction.normalized));
    }

    private IEnumerator MoveAndDestroy(Vector3 dir)
    {
        float elapsed = 0f;
        while (elapsed < lifetime)
        {
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDestroyOnWall(other.gameObject);
    }

    private void TryDestroyOnWall(GameObject other)
    {
        if (other == null) return;

        // 우선 DamageableWall 컴포넌트가 있으면 충돌로 간주
        if (other.GetComponent<DamageableWall>() != null)
        {
            Destroy(gameObject);
            return;
        }

        // 'Wall' 레이어로 설정되어 있으면 파괴
        int wallLayer = LayerMask.NameToLayer("Wall");
        if (wallLayer >= 0 && other.layer == wallLayer)
        {
            Destroy(gameObject);
            return;
        }

        // 태그 기반으로도 처리
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }
    }
}
