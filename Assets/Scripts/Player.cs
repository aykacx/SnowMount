using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("UI's")]
    [SerializeField] GameObject menuUI;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject deathUI;
    [SerializeField] GameObject endUI;

    [Header("MoveVar's")]
    [SerializeField] int forwardSpeed;
    [SerializeField] int xPos;

    [SerializeField] Transform playerTransform;

    GameManager gameManager;
    LevelManager levelManager;
    Rigidbody rb;
    Animator animator;
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        levelManager = GameObject.FindWithTag("GameManager").GetComponent<LevelManager>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (gameManager.CurrentGameState != GameState.Start)
        {
            return;
        }
        Vector3 movePos = new Vector3
            ((transform.position + MovePosition()).x, 
            (transform.position + MovePosition()).y, 
            (transform.position + MovePosition()).z);

        rb.MovePosition(movePos);
        if (gameObject.transform.position.x >= 13.5f)
        {
            Debug.Log("died");
        }
        else if (gameObject.transform.position.x <= -13.5f)
        {
            Debug.Log("died");
        }
    }
    private Vector3  MovePosition()
    {
        Vector3 moveVector = new Vector3(xPos * Time.deltaTime, 0, forwardSpeed * Time.deltaTime);
        return moveVector;
    }

    public void ChangeDirections()
    {
        xPos *= -1;
        playerTransform.rotation = Quaternion.Euler(0, (xPos <= -1) ? -125 : 125, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            inGameUI.SetActive(false);
            endUI.SetActive(true);
            gameManager.EndGame();
            Invoke("OpenNextLevel", 1.5f);
        }
        if (other.gameObject.tag == "Tree")
        {
            animator.SetTrigger("Death");
            inGameUI.SetActive(false);
            deathUI.SetActive(true);
            gameManager.EndGame();
            Invoke("OpenCurrentLevel", 1.5f);
        }
    }
    void OpenCurrentLevel()
    {
        animator.SetTrigger("Return");
        levelManager.StartLevel();
        deathUI.SetActive(false);
        menuUI.SetActive(true);
    }
    void OpenNextLevel()
    {
        gameManager.StartNextGame();
        endUI.SetActive(false);
        menuUI.SetActive(true);
    }
}
