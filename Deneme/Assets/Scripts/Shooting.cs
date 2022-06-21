using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    InputManager inputManager;
    public GameObject FinalCamera;
    [Header("Images")]
    public Image PowerFillImage;
    public Image PowerFillBackgroundImage;
    [Space]
    Rigidbody rg;
    float torque = 15;
    public float mouseSensitivity = 200f;
    public Transform playerBody;
    public Transform cameraRotation;
    float xRotation = 0;
    float yRotation = 0;
    public Transform Bow;
    [HideInInspector] public bool Canshoot = true;
    CameraFollow cameraFollow;
    Controller_UI UI;
    int FinalTileTriggerTime = 0;
    public GameObject ParticleSystem;
    float powerBarSpeed = 100;
    float power = 0f;
    bool powerUp = true;
    private Camera mainCamera;

    void Awake()
    {
        inputManager = InputManager.Instance;
    }
    void Start()
    {
        rg = this.gameObject.GetComponent<Rigidbody>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        UI = FindObjectOfType<Controller_UI>();
        mainCamera = Camera.main;
    }


    void Update()
    {
        if(Canshoot && Input.GetMouseButton(0))
        {

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            Debug.Log(Input.GetAxis("Mouse X"));
            xRotation += mouseX;
            yRotation -= mouseY;
            // Debug.Log("y: "+yRotation);
            // Debug.Log("x: "+xRotation);
            cameraRotation.localRotation = Quaternion.Euler(yRotation,xRotation,0);
            

        }

        if(Input.GetMouseButtonUp(0) && Canshoot && cameraFollow.CanShoot)
        {
            Canshoot = false;
            this.transform.SetParent(null);
            Bow.SetParent(null);
            rg.isKinematic = false;
            rg.AddForce(transform.forward*((power/100)*2),ForceMode.Impulse);
            rg.AddTorque(-transform.up*torque);
            cameraRotation.SetParent(this.gameObject.transform);
        }
        PowerBar();
    }

    void PowerBar()
    {
        if(powerUp && Canshoot)
        {
            PowerFillBackgroundImage.gameObject.SetActive(true);
            power += Time.deltaTime * powerBarSpeed;
            if(power > 100)
                powerUp = false;

        }    
        else if (Canshoot)
        {
            power -= Time.deltaTime * powerBarSpeed;
            if(power < 0)
                powerUp = true;
        }
        PowerFillImage.fillAmount = power / 100;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11 || other.gameObject.layer == 12 || other.gameObject.layer == 13)
        {
            rg.isKinematic = true;
            if(other.gameObject.layer == 11)    // Outer
                UI.Won(2);
            else if(other.gameObject.layer == 12)   // Middle
                UI.Won(5);
            else if(other.gameObject.layer == 13)   // Center
                UI.Won(10);
            
            FinalCamera.SetActive(true);
            ParticleSystem.SetActive(true);
            PowerFillBackgroundImage.gameObject.SetActive(false);
        }
        else if(other.gameObject.layer == 15)
        {
            FinalTileTriggerTime++;
            if(FinalTileTriggerTime == 2)
            {
                PowerFillBackgroundImage.gameObject.SetActive(false);
                UI.Won(1);
                FinalCamera.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }
}
