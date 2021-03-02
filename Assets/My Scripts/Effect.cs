using UnityEngine;

public class Effect : MonoBehaviour
{
    private ObjectPool _objectPool;

    [SerializeField] private float _destroyTime;

    private void OnEnable()
    {
        _objectPool = FindObjectOfType<ObjectPool>();
        Invoke(nameof(Destroy), _destroyTime);
    }
    private void Destroy()
    {
        _objectPool.ReturnGameObject(gameObject);
    }
}
