using System.Collections;
using UnityEngine;

public class DamageableWall : MonoBehaviour
{
    [Header("벽 체력")]
    [SerializeField] private float hp = 10;

    // 초기화용 체력    
    private float initialHp;

    private bool isDestroy = false;
    public bool IsDestroyed => isDestroy;

    // 히트 시 깜빡임
    private Color flashColor = Color.red;
    private float flashDuration = 0.08f;
    private int flashCount = 2;
    private Renderer rend;
    private Color[] originalColors;
    private Coroutine flashCoroutine;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        HandleHit(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        HandleHit(collision.gameObject);
    }

    private void HandleHit(GameObject other)
    {
        int projLayer = LayerMask.NameToLayer("Projectile");
        if (projLayer < 0) return; // 레이어가 설정되어 있지 않으면 무시

        if (other.layer == projLayer)
        {
            Projectile proj = other.GetComponent<Projectile>();
            if (proj != null)
            {
                hp -= proj.Damage;
                StartFlash();
                Destroy();
            }
        }
    }

    private void Awake()
    {
        initialHp = hp;

        rend = GetComponent<Renderer>() ?? GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            Material[] mats = rend.materials;
            originalColors = new Color[mats.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].HasProperty("_Color")) originalColors[i] = mats[i].color;
                else originalColors[i] = Color.white;
            }
        }
    }

    private void StartFlash()
    {
        if (rend == null) return;
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        Material[] mats = rend.materials;
        for (int f = 0; f < flashCount; f++)
        {
            // set to flash color
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].HasProperty("_Color")) mats[i].color = flashColor;
            }

            yield return new WaitForSeconds(flashDuration);

            // restore
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].HasProperty("_Color")) mats[i].color = originalColors[i];
            }

            yield return new WaitForSeconds(flashDuration);
        }

        flashCoroutine = null;
    }

    private void Destroy()
    {
        if (hp <= 0)
        {
            isDestroy = true;
            gameObject.SetActive(false);
        }
    }

    // 외부에서 벽을 복구할 때 사용합니다.
    public void Restore()
    {
        hp = initialHp;
        isDestroy = false;
        rend.material.color = Color.white;
        gameObject.SetActive(true);
    }
}
