using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    float speed = 7;
    Rigidbody rb;    
    GameObject Player;
    public GameObject Camera;
    GroundSpawner Spawn;
    Controller_UI UI;
    CameraFollow CameraFollow;
    private float elapsedTime;
    private float duration = 4f;
    int tilesPassed = 0;
    public GameObject Bow;
    float percentageComplete;
    bool oneTime = true;
    public GameObject PlayerModel;
    public Animator animator;
    [HideInInspector] public bool IsDead = false;
    int touchPart = Screen.width/3;
    
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        Player = this.gameObject;
        Spawn = FindObjectOfType<GroundSpawner>();
        UI = FindObjectOfType<Controller_UI>();
        CameraFollow = FindObjectOfType<CameraFollow>();
    }
    
    private void FixedUpdate()
    {
        if(CameraFollow.Final || IsDead) return;
        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);

        if(Input.GetMouseButton(0))
        {
            if(Input.mousePosition.x < touchPart) LeftMove();
            else if(Input.mousePosition.x > (touchPart + touchPart)) RightMove();
            return;
        }
        Straight();
    }

    void Update()
    {
        if(CameraFollow.Final && percentageComplete < .4)
        {
            Bow.SetActive(true);
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / duration;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-90,0,90),Mathf.SmoothStep(0,1,percentageComplete));
        }
        else if(CameraFollow.Final && percentageComplete > .4 && oneTime)
        {
            PlayerModel.SetActive(false);
            Bow.transform.SetParent(Camera.transform);
            oneTime = false;
        }
    }


    void LeftMove()
    {
        Player.transform.position =new Vector3(-2.6f,-1.35f,Player.transform.position.z);
        Player.transform.rotation = Quaternion.Euler(0,0,35);
    }
    void RightMove()
    {
        Player.transform.position = new Vector3(2.65f, -1.35f, Player.transform.position.z);
        Player.transform.rotation = Quaternion.Euler(0,0,145);
    }
    void Straight()
    {
        Player.transform.position = new Vector3(0, -1.35f, Player.transform.position.z);
        Player.transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            if(tilesPassed < 1)
            {
                Spawn.SpawnFromPool("Tile", GroundSpawner.nextSpawnPoint);
                tilesPassed++;
                return;
            }
            Spawn.FinalTileSpawn();
        }
        else if(other.gameObject.layer == 9)   // Arrow 
        {
            UI.ScoreTextSet();
            other.gameObject.SetActive(false);
        }
        else if(other.gameObject.layer == 10)   // Final Tile
        {
            //transform.position = new Vector3(0,-.7f,Player.transform.position.z);
            speed = 0;
            CameraFollow.Final = true;
            Straight();
        }
        else if(other.gameObject.layer == 14)
        {
            speed = 0;
            animator.SetTrigger("Death");
            UI.Died();
            IsDead = true;
        }
    }
}
