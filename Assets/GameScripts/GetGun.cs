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
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gunIsHide)
            {
                if (onHold != null)
                    StopCoroutine(onHold);
                onShoot = StartCoroutine(OnShoot());
                hideAfterTime = StartCoroutine(HideIfNotShooting());
            }
            else
            {
                if (onShoot != null)
                    StopCoroutine(onShoot);
                StopCoroutine(hideAfterTime);
                onHold = StartCoroutine(OnHold());
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (hideAfterTime != null && !gunIsHide)
            {
                gunShot.Play();
                m_Anim.SetBool("shot",true);
                StopCoroutine(hideAfterTime);
                hideAfterTime = StartCoroutine(HideIfNotShooting());
            }
        }

        if (Input.GetMouseButton(1) && !gunIsHide)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 20, 0.1f);
        }
    }

    IEnumerator OnHold()
    {
        gunIsHide = !gunIsHide;
        m_Anim.SetBool("gun", true);
        m_Anim.SetFloat("HideGun", 1);
        yield return new WaitForSeconds(0.5f);
        gun.transform.parent = vertebrae.transform;
        gun.transform.localPosition = new Vector3(-4f, -15.7f, 4.3f);
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
