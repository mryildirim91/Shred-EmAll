using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _initYPos;
    private Player _player;
    [SerializeField] private float _speed;
    
    private void Awake()
    {
        GameEvents.ONLevelContinue += ActivatePlayer;
        _player = GetComponentInChildren<Player>();
        transform.position = new Vector3(0, -ScreenArea.FitVer + 2, 0);
        _initYPos = transform.position.y;
    }

    private void Update()
    {
        if(!GameManager.Instance.IsGameStarted || GameManager.Instance.IsLevelComplete)
            return;
        
        Move();

        if (Input.GetKey(KeyCode.Space))
        {
            RotateAround();
        }
    }

    private void Move()
    {
        float hrzMove = Input.GetAxis("Horizontal");
       
        transform.Translate(Vector3.right * (hrzMove * _speed * Time.deltaTime));
        
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(_touch.position);
            touchPos.z = 0;
            Vector3 direction = touchPos - transform.position;

            switch (_touch.phase)
            {
                case TouchPhase.Began:
                    transform.Translate(new Vector3(direction.x, direction.y, 0) * (_speed * Time.deltaTime));
                    break;
                case TouchPhase.Moved:
                    transform.Translate(new Vector3(direction.x, direction.y, 0) * (_speed * Time.deltaTime));
                    break;
                case TouchPhase.Ended:
                    transform.Translate(Vector3.zero);
                    break;
                case TouchPhase.Stationary:
                    transform.Translate(new Vector3(direction.x, direction.y, 0) * (_speed * Time.deltaTime));
                    break;
            }
            
            RotateAround();
        }
        
        MoveLimit();
    }

    private void RotateAround()
    {
        _player.RotateAround();
    }
    
    private void MoveLimit()
    {
        float offset = transform.localScale.x / 2;
        float moveLimitX = Mathf.Clamp(transform.position.x, -ScreenArea.FitHrz + offset,
            ScreenArea.FitHrz - offset);
        float moveLimitY = Mathf.Clamp(transform.position.y, _initYPos, _initYPos);

        transform.position = new Vector3(moveLimitX, moveLimitY, 0);
    }

    private void ActivatePlayer()
    {
        Debug.Log("PlayerActivated");
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        GameEvents.ONLevelContinue -= ActivatePlayer;
    }
}
