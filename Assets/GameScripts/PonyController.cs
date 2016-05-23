using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.MainPerson
{
    public class PonyController : MonoBehaviour
    {
        //для внутриигрового меню
        public bool menuIsActive = false;

        public Camera ScrCam;
        public GameObject player;
        public Image screenPointCenter;
        public Text m_TextCursor;

        public Vector2 rotationRange = new Vector3(70, 70);
        public float rotationSpeed = 2;
        public float dampingTime = 0.2f;
        public float playerSpeed = 4;

        //Скроллинг  
        public float scrollSpeed = 1;//Скорость скроллинга    
        public float scrollMinDistance = 1;//Минимальная дистанция скроллинга    
        public float scrollMaxDistance = 10;//Максимальная дистанция скроллинга
        public float distance = 60;//Дистанция

        //Debug public
        public float acceleration = 0.1f;
        private float accelerationRun = 0;

        private Vector3 m_TargetAngles;
        private Vector3 m_FollowAngles;
        private Vector3 m_FollowVelocity;
        private Quaternion m_OriginalRotation;
        private Quaternion m_Angle;
        private Rigidbody m_Rigidbody;
        private Transform m_RayTransform;
        private float v;
        private float h;
        private float r;
        Quaternion oldRotation = Quaternion.Euler(0,0,0);

        private Animator anim;
        IniFile lastPosition = new IniFile("Saves/LastPosotion.dat");

        private void Awake()
        {
            Screen.lockCursor = true;
            
        }

        private void Start()
        {
            m_OriginalRotation = transform.localRotation;
            m_Angle = transform.localRotation;
            anim = player.GetComponent<Animator>();      
        }

        private void Update()
        {
            if (!menuIsActive)
            {

                transform.position = player.transform.position + new Vector3(0, 0.5f, 0);
                ControlCursor();
                ScrCam.fieldOfView = Mathf.Lerp(ScrCam.fieldOfView, distance, Time.deltaTime * 2);
                // we make initial calculations from the original local rotation
                transform.localRotation = m_OriginalRotation;

                // read input from mouse or mobile controls
                float inputH;
                float inputV;
                inputH = Input.GetAxis("Mouse X");
                inputV = Input.GetAxis("Mouse Y");

                // wrap values to avoid springing quickly the wrong way from positive to negative
                if (m_TargetAngles.y > 180)
                {
                    m_TargetAngles.y -= 360;
                    m_FollowAngles.y -= 360;
                }
                if (m_TargetAngles.x > 180)
                {
                    m_TargetAngles.x -= 360;
                    m_FollowAngles.x -= 360;
                }
                if (m_TargetAngles.y < -180)
                {
                    m_TargetAngles.y += 360;
                    m_FollowAngles.y += 360;
                }
                if (m_TargetAngles.x < -180)
                {
                    m_TargetAngles.x += 360;
                    m_FollowAngles.x += 360;
                }

                // with mouse input, we have direct control with no springback required.
                m_TargetAngles.y += inputH * rotationSpeed;
                m_TargetAngles.x += inputV * rotationSpeed;

                // clamp values to allowed range
                m_TargetAngles.y = Mathf.Clamp(m_TargetAngles.y, -rotationRange.y * 0.5f, rotationRange.y * 0.5f);
                m_TargetAngles.x = Mathf.Clamp(m_TargetAngles.x, -rotationRange.x * 0.5f, rotationRange.x * 0.5f);


                // smoothly interpolate current values to target angles
                m_FollowAngles = Vector3.SmoothDamp(m_FollowAngles, m_TargetAngles, ref m_FollowVelocity, dampingTime);
                // update the actual gameobject's rotation               

                transform.localRotation = m_OriginalRotation * Quaternion.Euler(-m_FollowAngles.x, m_FollowAngles.y, 0);
                m_Angle = transform.localRotation;
            }
        }

        private void FixedUpdate()
        {
            if (!menuIsActive)
            {
                scroll();
                KeyControll();
            }
        }

        //Какая-то магия, которая работает О_о

        private void KeyControll()
        {
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");
            if (Input.GetKey(KeyCode.LeftShift) && v > 0)
            {

                if (accelerationRun < 1)
                    accelerationRun += acceleration;
                else
                    accelerationRun = 1;
            }
            else
            {
                if (accelerationRun > 0)
                    accelerationRun -= acceleration;
                else
                    accelerationRun = 0;

            }

            anim.SetFloat("speed", v + accelerationRun);

            Quaternion toRotation = oldRotation;

            Transform pl_GameObject = transform;

            Transform leftBall = GameObject.Find("left").transform;
            Transform rightBall = GameObject.Find("right").transform;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {                

                if (Input.GetKey(KeyCode.A))
                {
                    toRotation = Quaternion.RotateTowards(oldRotation, Quaternion.Euler(0, 0, transform.rotation.z + 3), 0.5f);
                    oldRotation = toRotation;
                    pl_GameObject.LookAt(Input.GetKey(KeyCode.S) ? rightBall : leftBall);
                    
                }
                else
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        toRotation = Quaternion.RotateTowards(oldRotation, Quaternion.Euler(0, 0, transform.rotation.z - 3), 0.5f);
                        oldRotation = toRotation;
                        pl_GameObject.LookAt(Input.GetKey(KeyCode.S) ? leftBall : rightBall);
                        
                    }
                    else
                    {
                        toRotation = Quaternion.RotateTowards(oldRotation, Quaternion.Euler(0, 0, 0), 1f);
                        oldRotation = toRotation;
                    }
                }

                Vector3 motionVector = pl_GameObject.forward;

                Vector3 newDir = Vector3.RotateTowards(player.transform.forward, new Vector3(motionVector.x, 0, motionVector.z), rotationSpeed * Time.deltaTime * 0.5f, 0.0F);
                player.transform.rotation = Quaternion.LookRotation(newDir) * toRotation;
                player.transform.position += new Vector3(newDir.x * Time.deltaTime * playerSpeed * (v + accelerationRun * 2f), 0, newDir.z * Time.deltaTime * playerSpeed * (v + accelerationRun * 2f));


            }
            else
            {             

                if (Input.GetKey(KeyCode.A))
                {
                    toRotation = Quaternion.RotateTowards(oldRotation, Quaternion.Euler(0, 0, transform.rotation.z + 3), 0.5f);
                    oldRotation = toRotation;
                    pl_GameObject.LookAt(leftBall);
                    anim.SetFloat("speed", 1);
                    Vector3 motionVector = pl_GameObject.forward;

                    Vector3 newDir = Vector3.RotateTowards(player.transform.forward, new Vector3(motionVector.x, 0, motionVector.z), rotationSpeed * Time.deltaTime * 0.5f, 0.0F);
                    player.transform.rotation = Quaternion.LookRotation(newDir) * toRotation;
                    player.transform.position += new Vector3(newDir.x * Time.deltaTime * playerSpeed, 0, newDir.z * Time.deltaTime * playerSpeed);
                }
                else
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        toRotation = Quaternion.RotateTowards(oldRotation, Quaternion.Euler(0, 0, transform.rotation.z - 3), 0.5f);
                        oldRotation = toRotation;
                        pl_GameObject.LookAt(rightBall);
                        anim.SetFloat("speed", 1);
                        Vector3 motionVector = pl_GameObject.forward;

                        Vector3 newDir = Vector3.RotateTowards(player.transform.forward, new Vector3(motionVector.x, 0, motionVector.z), rotationSpeed * Time.deltaTime * 0.5f, 0.0F);
                        player.transform.rotation = Quaternion.LookRotation(newDir) * toRotation;
                        player.transform.position += new Vector3(newDir.x * Time.deltaTime * playerSpeed, 0, newDir.z * Time.deltaTime * playerSpeed);
                    }
                    else
                    {
                        toRotation = Quaternion.RotateTowards(oldRotation, Quaternion.Euler(0, 0, 0), 1f);
                        oldRotation = toRotation;
                        anim.SetFloat("speed", v + h);
                    }
                }             
                
            }

        }

        //Конец магии О_о

        private void ControlCursor()
        {

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Screen.lockCursor = false;
            }
            else
            {
                Screen.lockCursor = true;
            }

            //О мой чай!!!!! Оно работает! Оно живет!!!!! Не трогать! Понял?!

            m_RayTransform = getTransformFromCamRay();

            if (m_RayTransform != null)
            {

                if (Vector3.Distance(player.transform.position, m_RayTransform.position) <= 3)
                {
                    if (m_RayTransform.tag == "UselesItems")
                    {
                        m_TextCursor.text = m_RayTransform.name.Split('|')[0];
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            search();
                        }
                    }
                    if (m_RayTransform.tag == "Door")
                    {
                        m_TextCursor.text = m_RayTransform.name.Split('|')[1];
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            openDoor();
                        }
                    }
                }
                else
                {
                    m_TextCursor.text = "";
                }
            }
        }

        private void scroll()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float d = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
                if (distance - d >= scrollMinDistance && distance - d <= scrollMaxDistance)
                {
                    distance -= d;
                }
            }
        }

        public void search()
        {
            UselesObject thisObject = m_RayTransform.GetComponent<UselesObject>();
            thisObject.isUse();
        }

        public void openDoor()
        {
            if (m_RayTransform.name.Split('|')[0] != "MainScene")
            {
                lastPosition.IniWriteValue("MainScene", "position", player.transform.position.x + "|" + player.transform.position.y + "|" + player.transform.position.z);
                lastPosition.IniWriteValue("MainScene", "rotation", player.transform.rotation.x + "|" + player.transform.rotation.y + "|" + player.transform.rotation.z + "|" + player.transform.rotation.w);
                lastPosition.IniWriteValue("MainScene", "camRotation", transform.rotation.x + "|" + transform.rotation.y + "|" + transform.rotation.z + "|" + transform.rotation.w);
            }
            Application.LoadLevel(m_RayTransform.name.Split('|')[0]);
        }

        Transform getTransformFromCamRay()
        {
            Ray ray = ScrCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform;
            }
            else
            {
                return null;
            }
        }
    }
}


