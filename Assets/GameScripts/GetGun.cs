using UnityEngine;

public class GetGun : MonoBehaviour {

    private bool gunIsHide = true;
    private Animator m_Anim;
    private Animator m_Anim2;
    public GameObject gun;
    // Use this for initialization
    void Start () {
        m_Anim = GetComponent<Animator>();
        //m_Anim2 = GameObject.FindGameObjectWithTag("Gun").GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gunIsHide)
            {
                m_Anim.SetBool("gun", true);
                m_Anim.SetFloat("HideGun", 0);
               // m_Anim2.SetFloat("HideGun", 0);
                gunIsHide = !gunIsHide;
            }
            else
            {
                m_Anim.SetBool("gun", true);
                m_Anim.SetFloat("HideGun", 1);
                //m_Anim2.SetFloat("HideGun", 1);
                gunIsHide = !gunIsHide;
            }
        }

        //Debug.Log(m_Anim.GetCurrentAnimatorStateInfo(0).length);
    }
}
