using UnityEngine;
public class Destructables : MonoBehaviour, IDestructible
{
    private float _value1 = 5, _value2 = 8;
    private float _maxSpeed;

    [SerializeField] private float _turnSpeed;
    [SerializeField] private GameObject _effect;
    
    private Rigidbody _rb;
    private ObjectPool _objectPool;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _objectPool = FindObjectOfType<ObjectPool>();
        _maxSpeed = Random.Range(_value1, _value2);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameOver)
        {
            Movement();
        }
    }
    
    private void Movement()
    {
        if (transform.position.y < -ScreenArea.FitVer - 5)
        {
            transform.position = new Vector3(Random.Range(-ScreenArea.FitHrz, ScreenArea.FitHrz),  Random.Range(ScreenArea.FitVer + 10, ScreenArea.FitVer + 20), 0);
        }

        if (_rb.velocity.magnitude >= _maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _maxSpeed;
        }
        
        transform.Rotate(Vector3.one, _turnSpeed * Time.deltaTime, Space.Self);
    }

    public void Destruct(Vector3 pos)
    {
        GameObject obj = _objectPool.GetObject(_effect);
        obj.transform.position = pos;
        gameObject.SetActive(false);
    }
}
