using UnityEngine;
public class WallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _rightWall, _leftWall;

    private void Awake()
    {
        GameObject obj1 = Instantiate(_leftWall);
        obj1.transform.position = new Vector3(-ScreenArea.FitHrz - (obj1.transform.localScale.x / 2), 0,0);
        GameObject obj2 = Instantiate(_rightWall);
        obj2.transform.position = new Vector3(ScreenArea.FitHrz + (obj2.transform.localScale.x / 2), 0,0);
    }
}
