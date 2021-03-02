using System;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float _speed;

    private ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = FindObjectOfType<ObjectPool>();
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destructible"))
        {
            IDestructible destructible = other.GetComponent<IDestructible>();
            GameEvents.CollidedWithDestructibles();
            destructible?.Destruct(other.transform.position);
        }

        if (other.CompareTag("Bomb"))
        {
            AudioSource audioSource = GetComponentInParent<AudioSource>();
            audioSource.Play();

            IBomb bomb = other.GetComponent<IBomb>();
            bomb?.Bomb();
        }
    }

    private void OnBecameInvisible()
    {
        _objectPool.ReturnGameObject(gameObject);
    }

    private void Move()
    {
        transform.Translate(Vector3.up * (_speed * Time.deltaTime));
    }
}
