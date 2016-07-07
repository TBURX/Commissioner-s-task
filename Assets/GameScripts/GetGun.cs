using Assets.GameScripts.MainPerson;
using System.Collections;
using UnityEngine;

public class GetGun : MonoBehaviour {

    private bool gunIsHide = true;
    private Animator m_Anim;

    public GameObject gun;
    private AudioSource gunShot;
    public GameObject vertebrae;
    public GameObject head;
    public Camera camera;

    public PonyController ponyController;


    Coroutine hideAfterTime = null;
    Coroutine onHold = null;
    Coroutine onShoot = null;

    // Use this for initialization
    void Start () {
        m_Anim = GetComponent<Animator>();
        gunShot = gun.transform.Find("Audio Source").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        //выстрел
        if (Input.GetMouseButtonDown(0) && hideAfterTime != null && !gunIsHide)
        {
            gunShot.Play();
            m_Anim.SetBool("shot", true);
            StopCoroutine(hideAfterTime);
            hideAfterTime = StartCoroutine(HideIfNotShooting());
        }

        //выхват
        if (Input.GetMouseButtonDown(0) && gunIsHide)
        {
            if (onHold != null)
                StopCoroutine(onHold);
            onShoot = StartCoroutine(OnShoot());
            hideAfterTime = StartCoroutine(HideIfNotShooting());
        }              

        //прицеливание
        if (Input.GetMouseButton(1) && !gunIsHide)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 40, 0.1f);
            ponyController.isShotZoom = true;
        }
        else
        {
            ponyController.isShotZoom = false;
        }

        
        //поворот головы
        if(!gunIsHide && ponyController.isShotZoom)
        {
            m_Anim.enabled = false;
            head.transform.eulerAngles = new Vector3(90 + (Input.GetKey(KeyCode.LeftShift)?Mathf.Sin(Time.realtimeSinceStartup * 3) * 5:0), 180, 90) + new Vector3(-camera.transform.eulerAngles.x, camera.transform.eulerAngles.y, camera.transform.eulerAngles.z);
            head.transform.localEulerAngles = new Vector3(head.transform.localEulerAngles.x, head.transform.localEulerAngles.y, Mathf.Clamp(head.transform.localEulerAngles.z, 90, 270));
        }
        else
        {
            m_Anim.enabled = true;
        }
    }

    IEnumerator OnHold()
    {
        gunIsHide = !gunIsHide;
        m_Anim.SetBool("gun", true);
        m_Anim.SetFloat("HideGun", 1);
        yield return new WaitForSeconds(0.5f);
        gun.transform.parent = vertebrae.transform;
        gun.transform.localPosition = new Vector3(-4f, -13.5f, 4.3f);
        gun.transform.localEulerAngles = new Vector3(4.4f, 9.27f, 88.78f);
    }

    IEnumerator OnShoot()
    {
        gunIsHide = !gunIsHide;
        m_Anim.SetBool("gun", true);
        m_Anim.SetFloat("HideGun", 0);
        yield return new WaitForSeconds(0.5f);
        gun.transform.parent = head.transform;
        gun.transform.localPosition = new Vector3(-49.18f, -0.46f, -13.14f);
        gun.transform.localEulerAngles = new Vector3(360.88f, -95.24f, -267.99f);
    }

    IEnumerator HideIfNotShooting()
    {
        yield return new WaitForSeconds(7);
        if (!gunIsHide)
        {
            StartCoroutine(OnHold());
        }
    }

    void FixedUpdate()
    {
    }
}
