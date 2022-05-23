using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TW
{
    public class InputManager : MonoBehaviour
    {
        [Header("References")]
        public PlayerControls inputActions;

        [Header("Inputs")]
        Vector2 movementInput;
        public Vector2 mouseInput;
        public Vector2 analogInput;
        public float horizontal;
        public float vertical;
        public bool dash_Input;
        public bool jump_Input;
        public bool cast_Input;
        public bool leftInv_Input;
        public bool rightInv_Input;
        public bool specialCast_Input;
        public bool pause_input;

        public float delta;

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.Movement.Move.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.Movement.Mouse.performed += inputActions => mouseInput = inputActions.ReadValue<Vector2>();
                inputActions.Movement.Analog.performed += inputActions => analogInput = inputActions.ReadValue<Vector2>();
            }
            inputActions.Enable();
        }
        private void OnDisable()
        {
            inputActions.Disable();
        }


        void Update()
        {
            delta = Time.deltaTime;

            HandleMovement();
            HandleAttacks();
            HandleInventory();
            HandleSettings();
        }

        public void HandleMovement()
        {

            //horizontal = -movementInput.y;
            //vertical = movementInput.x;
            //Defalth Values
            horizontal = movementInput.x;
            vertical = movementInput.y;

            inputActions.Actions.Dash.performed += i => dash_Input = true;
            inputActions.Actions.Jump.performed += i => jump_Input = true;
        }

        public void HandleAttacks()
        {
            inputActions.Actions.Cast.performed += i => cast_Input = true;
            inputActions.Actions.SpecialCast.performed += i => specialCast_Input = true;

        }

        public void HandleInventory()
        {
            inputActions.Inventory.ChooseRight.performed += i => rightInv_Input = true;
            inputActions.Inventory.ChooseLeft.performed += i => leftInv_Input = true;
        }

        public void HandleSettings()
        {
            inputActions.Settings.Pause.performed += i => pause_input = true;
        }

        private void LateUpdate()
        {
            dash_Input = false;
            jump_Input = false;
            cast_Input = false;
            specialCast_Input = false;

            leftInv_Input = false;
            rightInv_Input = false;

            pause_input = false;
        }

    }
}