using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scene에 있는 wall 오브젝트를 등록해주세요,\nPrefab 폴더 오브젝트 등록 X")]
    [SerializeField] private DamageableWall wall;

    // Update is called once per frame
    void Update()
    {
        if (wall != null && wall.IsDestroyed && Input.GetKeyDown(KeyCode.M))
        {
            wall.Restore();
        }
    }
}
