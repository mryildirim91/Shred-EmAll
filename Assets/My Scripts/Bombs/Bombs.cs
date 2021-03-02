using UnityEngine;

public class Bombs : MonoBehaviour, IBomb
{
    [SerializeField] private GameObject _effect;
    
    private float _value1 = 3, _value2 = 6;
    private float _maxSpeed;
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
        if (GameManager.Instance.IsLevelComplete || GameManager.Instance.IsGameOver)
        {
            _objectPool.ReturnGameObject(gameObject);
            return;
        }
        Movement();
    }
    
    private void Movement()
    {
        if (transform.position.y < -ScreenArea.FitVer - 5)
        {
            transform.position = new Vector3(Random.Range(-ScreenArea.FitHrz, ScreenArea.FitHrz),  Random.Range(ScreenArea.FitVer + 10 , ScreenArea.FitVer + 20), 0);
        }

        if (_rb.velocity.magnitude >= _maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _maxSpeed;
        }
        
        transform.Rotate(Vector3.one, 200 * Time.deltaTime, Space.Self);
    }

    public void Bomb()
    {
        GameObject obj = _objectPool.GetObject(_effect);
        
        obj.transform.position = transform.position;
        gameObject.SetActive(false);
    }
}
