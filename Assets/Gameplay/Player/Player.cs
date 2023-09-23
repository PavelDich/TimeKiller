using System.Collections;
using Mirror;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.AI;

public class Player : NetworkBehaviour
{
    #region Parameters
    public Components components;
    [System.Serializable]
    public class Components
    {
        public Transform _transform;
        public Rigidbody _rigidbody;
        public CapsuleCollider _collider;
        public Animator _animatorPlayer;
        public Animator _animatorMenu;
    }

    public Controller controller;
    [System.Serializable]
    public class Controller
    {
        public bool isActive;
        public GameObject[] localObjects;
        public Parameters parameters;
        [System.Serializable]
        public class Parameters
        {
            public Heal health;
            [System.Serializable]
            public class Heal
            {
                public float Max = 100f;
                public float Health = 100f;
                public float Regen = 1f;
                public float RegenReload = 5f;
                public Image Bar;
                public Image BarDiff;
                [HideInInspector]
                public float RegenReloadTimeLeft;
                [HideInInspector]
                public float OldValue;
            }
            public Stamin stamina;
            [System.Serializable]
            public class Stamin
            {
                public float Max = 100f;
                public float Stamina = 100f;
                public float Regen = 1f;
                public float RegenReload = 5f;
                public Image Bar;
                public Image BarDiff;
                [HideInInspector]
                public float RegenReloadTimeLeft;
                [HideInInspector]
                public float OldValue;
            }
        }
        public MainCamera mainCamera;
        [System.Serializable]
        public class MainCamera
        {
            //public VideCamera videCamera;
            public Camera camera;
            public float SensativityX;
            public float SensativityY;
            [HideInInspector]
            public float eulerX;
            [HideInInspector]
            public float eulerY;
        }
        public Body body;
        [System.Serializable]
        public class Body
        {
            [Header("Move")]
            public bool isSprint;
            public float SprintSpeed = 10f;
            public float SprintStaminaNeed = 0.1f;

            public float WalkSpeed = 5f;
            public float WalkStaminaNeed = 0.01f;

            public float CrawlHeight = 1f;
            public float CrawlSize = 0.5f;
            public float CrawlSpeed = 1f;
            public float CrawlStaminaNeed = 0f;


            [Header("Jump")]
            public LayerMask Ground;
            public int GroundCount;
            public float rayDistance = 0.6f;
            public float JumpForce = 5f;
            public float JumpStaminaNeed = 20f;
        }
    }

    [SyncVar(hook = nameof(SetHealth))]
    public float _Health = 100f;
    public void ChangeHealth(float newValue)
    {
        if (isServer)
            SyncHealth(newValue);
        else
            CmdSyncHealth(newValue);

        [Server]
        void SyncHealth(float newValue)
        { _Health = newValue; }
        [Command]
        void CmdSyncHealth(float newValue)
        { _Health = newValue; }
    }
    public void SetHealth(float oldValue, float newValue) { controller.parameters.health.Health = newValue; }
    #endregion


    private void Start()
    {
        if (!isOwned)
            foreach (GameObject i in controller.localObjects)
                Destroy(i);
        else
            SettingsImport();
    }

    private void FixedUpdate()
    {
        if (isOwned && controller.isActive)
        {
            Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetKey(KeyCode.LeftShift), Input.GetKey(KeyCode.LeftControl));
        }
    }

    private void Update()
    {
        if (isOwned)
        {
            if (controller.isActive)
            {
                MoveHead(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), controller.mainCamera.SensativityX, controller.mainCamera.SensativityY);
                if (Input.GetKeyDown(KeyCode.Space)) MoveJump(controller.body.JumpForce);
                Parameters();
                if (Input.GetKeyDown(KeyCode.H)) ChangeHealth(controller.parameters.health.Health - 10);
                if (Input.GetKeyDown(KeyCode.V)) Manager.gameManager.isGameStarted = !Manager.gameManager.isGameStarted;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                OpenMenu();
        }
    }







    public void OpenMenu()
    {
        controller.isActive = !controller.isActive;
        components._animatorMenu.SetBool("isActive", !controller.isActive);
        Cursor.visible = !controller.isActive;
        SettingsImport();
    }

    public void SettingsImport()
    {
        controller.mainCamera.SensativityX = float.Parse(PlayerPrefs.GetString("SettingsPlayerCameraSensitivityX"));
        controller.mainCamera.SensativityY = float.Parse(PlayerPrefs.GetString("SettingsPlayerCameraSensitivityY"));
    }

    public void Parameters()
    {
        if (controller.parameters.health.Bar != null && controller.parameters.health.BarDiff != null)
        {
            if (controller.parameters.health.Health < controller.parameters.health.Max)
                controller.parameters.health.Health += controller.parameters.health.Regen * Time.deltaTime;

            controller.parameters.health.Bar.fillAmount = controller.parameters.health.Health / controller.parameters.health.Max;
        }

        controller.parameters.stamina.RegenReloadTimeLeft -= Time.deltaTime;
        if (controller.parameters.stamina.Bar != null && controller.parameters.stamina.BarDiff != null)
        {
            if (controller.parameters.stamina.Stamina < controller.parameters.stamina.Max && controller.parameters.stamina.RegenReloadTimeLeft <= 0)
                controller.parameters.stamina.Stamina += controller.parameters.stamina.Regen * Time.deltaTime;

            controller.parameters.stamina.Bar.fillAmount = controller.parameters.stamina.Stamina / controller.parameters.stamina.Max;
            if (!controller.body.isSprint) controller.parameters.stamina.BarDiff.fillAmount =
            Mathf.Lerp(controller.parameters.stamina.Bar.fillAmount, controller.parameters.stamina.OldValue, controller.parameters.stamina.RegenReloadTimeLeft / controller.parameters.stamina.RegenReload);
        }
    }


    public void Move(float horizontal, float vertical, bool shift, bool control)
    {
        if (shift && !Physics.Raycast(components._transform.position, Vector3.up, controller.body.CrawlHeight, controller.body.Ground))
        {
            MoveBody(horizontal, vertical, controller.body.SprintSpeed, controller.body.SprintStaminaNeed);
            controller.body.isSprint = true;
            //            components._animator.SetBool("isCrawl", false);
            controller.parameters.stamina.RegenReloadTimeLeft = controller.parameters.stamina.RegenReload;
            controller.parameters.stamina.OldValue = controller.parameters.stamina.BarDiff.fillAmount;
            components._collider.height = 2f;
        }
        else if (control)
        {
            MoveBody(horizontal, vertical, controller.body.CrawlSpeed, controller.body.CrawlStaminaNeed);
            controller.body.isSprint = false;
            //            components._animator.SetBool("isCrawl", true);
            components._collider.height = controller.body.CrawlSize;
        }
        else if (!Physics.Raycast(components._transform.position, Vector3.up, controller.body.CrawlHeight, controller.body.Ground))
        {
            MoveBody(horizontal, vertical, controller.body.WalkSpeed, controller.body.WalkStaminaNeed);
            controller.body.isSprint = false;
            //            components._animator.SetBool("isCrawl", false);
            components._collider.height = 2f;
        }
        else
        {
            MoveBody(horizontal, vertical, controller.body.CrawlSpeed, controller.body.CrawlStaminaNeed);
            controller.body.isSprint = false;
            //            components._animator.SetBool("isCrawl", true);
            components._collider.height = controller.body.CrawlSize;
        }
    }
    private void MoveBody(float horizontal, float vertical, float speed, float staminaNeed)
    {
        if (controller.parameters.stamina.Stamina < (controller.parameters.stamina.Max * 0.25))
            speed *= controller.parameters.stamina.Stamina * 4 / controller.parameters.stamina.Max;
        if (horizontal != 0f || vertical != 0f)
        { if (staminaNeed >= 0) controller.parameters.stamina.Stamina = Mathf.Clamp(controller.parameters.stamina.Stamina - staminaNeed, 0f, controller.parameters.stamina.Max); }
        else if (controller.parameters.stamina.RegenReloadTimeLeft <= 0f)
            controller.parameters.stamina.Stamina = Mathf.Clamp(controller.parameters.stamina.Stamina - staminaNeed, 0f, controller.parameters.stamina.Max);

        Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontal, 0f, vertical));
        moveDirection.y = 0f;
        components._rigidbody.velocity = moveDirection * speed + new Vector3(0f, components._rigidbody.velocity.y, 0f);
    }

    public void MoveJump(float force)
    {
        if (controller.body.GroundCount > 0 && controller.parameters.stamina.Stamina > controller.body.JumpStaminaNeed)
        {
            if (controller.parameters.stamina.Stamina < (controller.parameters.stamina.Max * 0.25))
                force *= controller.parameters.stamina.Stamina * 4 / controller.parameters.stamina.Max;
            controller.parameters.stamina.Stamina -= controller.body.JumpStaminaNeed;
            controller.parameters.stamina.RegenReloadTimeLeft = controller.parameters.stamina.RegenReload;
            controller.parameters.stamina.OldValue = controller.parameters.stamina.BarDiff.fillAmount;

            components._rigidbody.velocity = new Vector3(0, force, 0);
        }
    }

    public void MoveHead(float mouseX, float mouseY, float sensitivityX, float sensitivityY)
    {
        controller.mainCamera.eulerX -= mouseY * sensitivityY * Time.deltaTime;
        controller.mainCamera.eulerX = Mathf.Clamp(controller.mainCamera.eulerX, -80f, 80f);
        controller.mainCamera.camera.transform.localRotation = Quaternion.Euler(controller.mainCamera.eulerX, 0, 0);

        controller.mainCamera.eulerY = (transform.rotation.eulerAngles.y + mouseX * sensitivityX * Time.deltaTime) % 360;
        transform.rotation = Quaternion.Euler(0, controller.mainCamera.eulerY, 0);
    }





    private void OnTriggerEnter(Collider col)
    {
        if ((~controller.body.Ground & (1 << col.gameObject.layer)) == 0)
        {
            controller.body.GroundCount++;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if ((~controller.body.Ground & (1 << col.gameObject.layer)) == 0)
        {
            controller.body.GroundCount--;
        }
    }
}
