using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    public Player _player;
    public Player player
    {
        get { return _player; }
        set { _player = value; }
    }

    public object Player { get; internal set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Scene을 이동하더라도 파괴되지 않게 
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
