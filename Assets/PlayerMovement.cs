using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Viteza de miscare a jucatorului
    // public float jumpHeight = 2f; // Inaltimea sariturii, dezactivata pentru ca saritura este dezactivata
    public Transform groundCheck; // Transform pentru a verifica coliziunea cu solul
    public float groundDistance = 0.4f; // Distanta de verificare pentru coliziunea cu solul
    public LayerMask groundMask; // Layer pentru sol

    private CharacterController controller; // Referinta la componenta CharacterController
    private Vector3 velocity; // Viteza jucatorului
    private bool isGrounded; // Flag pentru a verifica daca jucatorul este pe sol
    private bool canMove = true; // Variabila pentru a controla daca jucatorul se poate misca
    private Vector3 lastPosition; // Variabila pentru a stoca ultima pozitie valida

    void Start()
    {
        controller = GetComponent<CharacterController>(); // Obtinerea componentei CharacterController
    }

    void Update()
    {
        if (!canMove)
        {
            // Resetarea vitezei pentru a preveni fortele continue
            velocity = Vector3.zero;
            controller.Move(Vector3.zero); // Asigura ca jucatorul nu se misca
            return;
        }

        // Verificare coliziune cu solul
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Asigura ca jucatorul ramane pe sol
            lastPosition = transform.position; // Actualizeaza ultima pozitie valida
        }

        float moveHorizontal = Input.GetAxis("Horizontal"); // Obtinerea input-ului orizontal
        float moveVertical = Input.GetAxis("Vertical"); // Obtinerea input-ului vertical

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical; // Calcularea directiei de miscare

        controller.Move(move * speed * Time.deltaTime); // Miscarea jucatorului

        // Eliminare sau comentare a gestionarii input-ului pentru sarituri
        /*
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
        */

        velocity.y += Physics.gravity.y * Time.deltaTime; // Aplicarea gravitatiei

        controller.Move(velocity * Time.deltaTime); // Aplicarea miscarii pe axa Y (sus-jos)
    }

    public void EnableMovement(bool enable)
    {
        canMove = enable; // Activarea sau dezactivarea miscarii
        if (!enable)
        {
            velocity = Vector3.zero; // Resetarea vitezei cand miscarea este dezactivata
            transform.position = lastPosition; // Resetarea pozitiei la ultima pozitie valida
            controller.Move(Vector3.zero); // Asigura ca jucatorul nu se misca
        }
    }
}