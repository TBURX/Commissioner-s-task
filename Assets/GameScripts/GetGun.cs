using System.Collections;
using UnityEngine;

public class GetGun : MonoBehaviour {

    private bool gunIsHide = true;
    private Animator m_Anim;
    public GameObject gun;
    public GameObject vertebrae;
    public GameObject head;


    Coroutine hideAfterTime = null;
    Coroutine onHold = null;
    Coroutine onShoot = null;

    // Use this for initialization
    void Start () {
        m_Anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator OnHold()
    {
        m_Anim.SetBool("gun", true);
        m_Anim.SetFloat("HideGun", 1);
        yield return new WaitForSeconds(0.5f);
        gun.transform.parent = vertebrae.transform;
        gun.transform.localPosition = new Vector3(-4f, -15.7f, 4.3f);
        gun.transform.localEulerAngles = new Vector3(4.4f, 9.27f, 88.78f);
        gunIsHide = !gunIsHide;
    }

    IEnumerator OnShoot()
    {
        m_Anim.SetBool("gun", true);
        m_Anim.SetFloat("HideGun", 0);
        yield return new WaitForSeconds(0.5f);
        gun.transform.parent = head.transform;
        gun.transform.localPosition = new Vector3(-49.18f, -0.46f, -13.14f);
        gun.transform.localEulerAngles = new Vector3(360.88f, -95.24f, -267.99f);
        gunIsHide = !gunIsHide;
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gunIsHide)
            {
                if(onHold != null)
                    StopCoroutine(onHold);
                onShoot = StartCoroutine(OnShoot());
                hideAfterTime = StartCoroutine(HideIfNotShooting());
            }
            else
            {
                if (onShoot != null)
                    StopCoroutine(onShoot);
                onHold = StartCoroutine(OnHold());
                StopCoroutine(hideAfterTime);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (hideAfterTime != null)
            {
                StopCoroutine(hideAfterTime);
                hideAfterTime = StartCoroutine(HideIfNotShooting());
            }
        }


        //Debug.Log(m_Anim.GetCurrentAnimatorStateInfo(0).length);
    }
}
