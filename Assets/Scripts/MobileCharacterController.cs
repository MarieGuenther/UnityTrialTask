using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileCharacterController : MonoBehaviour
{
    [SerializeField]
    private CharacterController _unityCharacterController = default;
    [SerializeField]
    private float _movementSpeed = 0.5f;
    private float gravity;

    [SerializeField]
    private Transform _pointer = default;

    private void Start()
    {
        //_pointer = GameManager.Instance.Pointer;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentMode == GameManager.GameMode.Play)
        {
            gravity -= 9.81f * Time.deltaTime;
            Vector3 input = new Vector3(SimpleInput.GetAxis("Horizontal"), gravity, SimpleInput.GetAxis("Vertical"));
            _unityCharacterController.Move(input * _movementSpeed);
            if (_unityCharacterController.isGrounded) gravity = 0;
        }
        else if (GameManager.Instance.CurrentMode == GameManager.GameMode.Edit && GameManager.Instance.CurrentEditMode == GameManager.EditMode.CharacterSet)
        {
            CharacterPlacementCheck();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceCharacter();
            }
        }
    }

    private void CharacterPlacementCheck()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            _pointer.position = hit.point + _pointer.up * 2;
        }
        else
        {
            _pointer.position = new Vector3(1000, 1000);
        }
    }

    private void PlaceCharacter()
    {
        GameManager.Instance.ChangeCharacterPlaced(true);
        GameManager.Instance.ChangeEditMode(GameManager.EditMode.Camera);
    }
}
