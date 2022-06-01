using System;
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

    public DirectionModule directionModule;


    [Serializable]
    public struct Range
    {
        public float min;
        public float max;
    }

    [Serializable]
    public struct DirectionModule
    {

        public float rotationSpeed; //bu abimiz rotasyon hýzý ne kadar hýzlý saða sola dönecek
        public Range rotationLimit; //rotasyon limitleri ne kadar saða sola döncek
        public Quaternion CurrentRotation { get; private set; } //bu abimiz sadece bizim o anki current rotationu tutuyo o anki rotation neyse bu da o
        public bool IsLeft { get; set; } // solda mý saðda mý kontrol amaçlý isleft true gelirse minimumu false gelirse maxý kullanýyo

        public void SmoothRotation()
        {
            var dt = Time.deltaTime;

            var rotation = Quaternion.Euler(0, IsLeft ? rotationLimit.min : rotationLimit.max, 0);
            CurrentRotation = Quaternion.Slerp(CurrentRotation, rotation,rotationSpeed * dt);
        }
    }
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

        // bak þimdi gördüm bizim xposla iþimiz yok sanýrým
        directionModule.SmoothRotation();
        rb.MoveRotation(directionModule.CurrentRotation);

    }
    private Vector3  MovePosition()
    {
        
        Vector3 moveVector = transform.forward * forwardSpeed * Time.deltaTime;
            //new Vector3(xPos * Time.deltaTime, 0, forwardSpeed * Time.deltaTime);
        return moveVector;
    }
    //ilk commit
    public void ChangeDirections()
    {
        // burda islefti setliyoruz xpos eðer 0dan küçükse true geliyo büyükse false
        xPos *= -1;
        directionModule.IsLeft = xPos < 0;
        //playerTransform.rotation = Quaternion.Euler(0, (xPos <= -1) ? -125 : 125, 0);
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



