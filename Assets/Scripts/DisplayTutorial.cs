using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class DisplayTutorial : MonoBehaviour
{
    public GameObject uiControler;
    public GameObject detailControler;
    public GameObject detailModeGame;
    public GameObject menuMain;
    public GameObject displayTutorial;

    public string nameState;

    public static DisplayTutorial Instance { get; private set; }

    private GameObject[] players;


    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        uiControler.SetActive(true);
        detailControler.SetActive(false);
        detailModeGame.SetActive(false);
    }


    public void Controler()
    {
        detailControler.SetActive(true);
        uiControler.SetActive(false);
    }

    public void gameMode()
    {
        detailModeGame.SetActive(true);
        uiControler.SetActive(false);
    }

    public void OnExitClickedInTutorial()
    {
        detailControler.SetActive(false);
        detailModeGame.SetActive(false);
        uiControler.SetActive(false);
        displayTutorial.SetActive(false);
        menuMain.SetActive(true);
    }
}