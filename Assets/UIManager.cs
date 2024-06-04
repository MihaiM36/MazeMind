using TMPro.Examples; // Importarea spatiului de nume pentru TextMeshPro (daca este utilizat)
using UnityEngine; // Importarea spatiului de nume Unity
using UnityEngine.UI; // Importarea spatiului de nume pentru UI in Unity

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Instanta Singleton a UIManager

    public GameObject questionPanel; // Referinta la panoul de intrebari
    public Text questionText; // Referinta la textul intrebarii
    public Button[] answerButtons; // Referinte la butoanele de raspuns

    private ObstacleInteraction currentObstacle; // Referinta la obstacolul curent

    void Awake()
    {
        // Pattern Singleton pentru a asigura existenta unei singure instante a UIManager
        if (Instance == null)
        {
            Instance = this; // Setarea instantei la aceasta componenta
        }
        else
        {
            Destroy(gameObject); // Distruge acest gameObject daca deja exista o instanta
        }
    }

    void Start()
    {
        HideQuestionPanel(); // Ascunderea panoului de intrebari la start
    }

    private void FindAndDisablePlayerMovement()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Gaseste jucatorul folosind tag-ul "Player"
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>(); // Obtine componenta PlayerMovement
            if (playerMovement != null)
            {
                playerMovement.EnableMovement(false); // Dezactiveaza miscarea jucatorului
            }
            else
            {
                Debug.LogError("PlayerMovement script not found on the player."); // Eroare daca componenta nu este gasita
            }

            MouseLook mouseLook = player.GetComponentInChildren<MouseLook>(); // Obtine componenta MouseLook
            if (mouseLook != null)
            {
                mouseLook.EnableRotation(false); // Dezactiveaza rotatia camerei
            }
            else
            {
                Debug.LogError("MouseLook script not found on the player."); // Eroare daca componenta nu este gasita
            }
        }
        else
        {
            Debug.LogError("Player with tag 'Player' not found."); // Eroare daca jucatorul nu este gasit
        }
    }

    private void FindAndEnablePlayerMovement()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Gaseste jucatorul folosind tag-ul "Player"
        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>(); // Obtine componenta PlayerMovement
            if (playerMovement != null)
            {
                playerMovement.EnableMovement(true); // Activeaza miscarea jucatorului
            }

            MouseLook mouseLook = player.GetComponentInChildren<MouseLook>(); // Obtine componenta MouseLook
            if (mouseLook != null)
            {
                mouseLook.EnableRotation(true); // Activeaza rotatia camerei
            }
        }
    }

    public void ShowQuestionPanel(string question, string[] answers, ObstacleInteraction obstacle)
    {
        questionText.text = question; // Seteaza textul intrebarii
        currentObstacle = obstacle; // Seteaza obstacolul curent

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < answers.Length)
            {
                answerButtons[i].gameObject.SetActive(true); // Activeaza butonul de raspuns
                answerButtons[i].GetComponentInChildren<Text>().text = answers[i]; // Seteaza textul raspunsului
                int answerIndex = i;
                answerButtons[i].onClick.RemoveAllListeners(); // Elimina toti ascultatorii de evenimente de click
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answerIndex)); // Adauga un ascultator pentru evenimentul de click
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false); // Dezactiveaza butonul daca nu exista raspuns
            }
        }

        questionPanel.SetActive(true); // Afiseaza panoul de intrebari
        Cursor.lockState = CursorLockMode.None; // Deblocheaza cursorul
        Cursor.visible = true; // Face cursorul vizibil

        // Gaseste si dezactiveaza miscarea jucatorului
        FindAndDisablePlayerMovement();
    }

    public void HideQuestionPanel()
    {
        questionPanel.SetActive(false); // Ascunde panoul de intrebari
        Cursor.lockState = CursorLockMode.Locked; // Blocheaza cursorul
        Cursor.visible = false; // Ascunde cursorul

        // Gaseste si activeaza miscarea jucatorului
        FindAndEnablePlayerMovement();
    }

    private void OnAnswerSelected(int index)
    {
        currentObstacle.CheckAnswer(index); // Verifica raspunsul selectat
        HideQuestionPanel(); // Ascunde panoul de intrebari
    }
}