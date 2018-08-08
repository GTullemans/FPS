using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float _WalkSpeed, _SprintSpeed;
    private float _TargetSpeed;
    [SerializeField] private float _Speed;
    [SerializeField] private float _Accalation;
    private Vector3 _Direction;
    private Vector3 _Movement;
    private Vector3 _PrevDir;
    [SerializeField] private float _Jumphight;
    private Rigidbody _RigidBody;
    private bool _Isgrouded;

    [SerializeField] private float _MouseSensitivityX, _MouseSensitivityY;
    private Quaternion _CharacterRotation;
    private Quaternion _CameraRotation;
    private Camera _Camera;

    private bool _cursorIsLocked = true;
    // Use this for initialization
    void Start () {
        _Direction = Vector3.zero;
        _Camera = Camera.main;

        _CameraRotation = _Camera.transform.localRotation;
        _CharacterRotation = transform.localRotation;


        _RigidBody = GetComponent<Rigidbody>();

	}

    private void Update()
    {
        LookRotation();

        _Isgrouded = Physics.Raycast(transform.position, Vector3.down, 1.2f);
        Debug.DrawRay(transform.position, new Vector3(0, -1.2f, 0));     
        
        

        if (Input.GetKeyDown(KeyCode.Space) && _Isgrouded)
            {
                _RigidBody.AddForce(Vector3.up*_Jumphight, ForceMode.Impulse);
            }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _TargetSpeed = _SprintSpeed;
        }
        else { _TargetSpeed = _WalkSpeed; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Getinput();

        

        if (_Direction != Vector3.zero)
        {
            if (_Speed <= _TargetSpeed)
            {
                _Speed += _Accalation;
            }
            _Movement = _Direction * _Speed;


            _PrevDir = _Direction;
        }
        if(_Speed > _TargetSpeed)
        {
            _Speed -= _Accalation;
        }

        if(_Direction == Vector3.zero&& _Speed != 0)
        {
            _Movement = _PrevDir * _Speed;

            _Speed -= _Accalation;

            if(_Speed < 0) { _Speed = 0; }

        }


        transform.Translate(_Movement * Time.fixedDeltaTime);
    }


    private void Getinput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _Direction.x = 1;
        }
        
        else if (Input.GetKey(KeyCode.S))
        {
            _Direction.x = -1;
        }
        else { _Direction.x = 0; }

        if (Input.GetKey(KeyCode.D))
        {
            _Direction.z = -1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _Direction.z = 1;
        }
        else { _Direction.z = 0; }

        _Direction.Normalize();
    }


    private void LookRotation()
    {
        float rotationy = Input.GetAxis("Mouse X") * _MouseSensitivityX;
        float rotationx = Input.GetAxis("Mouse Y") * _MouseSensitivityY;

        _CharacterRotation *= Quaternion.Euler(0, rotationy, 0);
        _CameraRotation *= Quaternion.Euler(-rotationx, 0, 0);

        transform.localRotation = _CharacterRotation;
        _Camera.transform.localRotation = _CameraRotation;

        InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _cursorIsLocked = true;
        }

        if (_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
