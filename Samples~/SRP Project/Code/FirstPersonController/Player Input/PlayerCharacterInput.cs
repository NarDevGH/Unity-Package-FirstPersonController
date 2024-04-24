using UnityEngine;
using NarDev.Player.Input;

namespace Nardev.Player.CharacterControler
{
    [AddComponentMenu("FirstPersonController/PlayerCharacterInput")]
    public class PlayerCharacterInput : MonoBehaviour
    {
        #region PROPERTIES
        
        private IPlayerCharacter _player_character;
        Vector2 _mouse_delta;

        #endregion

        #region GETTERS & SETTERS

        private @PlayerInputActions.InGameActions InGameActions
        {
            get => PlayerInputManager.input_actions_maps.InGame;
        }

        #endregion

        #region UNITY METHODS

        // Start is called before the first frame update
        void Awake()
        {
            _player_character = GetComponent<IPlayerCharacter>();

            InGameActions.Jump.performed += Jump;
            InGameActions.StartRunning.performed += StartRunning;
            InGameActions.StopRunning.performed += StopRunning;
            InGameActions.StartCrouch.performed += StartCrouch;
            InGameActions.StopCrouch.performed += StopCrouch;
        }

        void Start()
        {
            InGameActions.Enable();

            PlayerInputManager.LockCursor();

            PlayerInputManager.hold_to_run = true;
            PlayerInputManager.hold_to_crouch = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (InGameActions.enabled == false) return;

            _player_character.MoveTowardsDirection(InGameActions.Move.ReadValue<Vector2>());

            //Get mouse delta values
            _mouse_delta = InGameActions.LookAround.ReadValue<Vector2>();

            //Apply sensitivity to mouse
            _mouse_delta.x *= PlayerInputManager.mouse_sensitivity;
            _mouse_delta.y *= PlayerInputManager.mouse_sensitivity;

            _player_character.Turn(_mouse_delta.x);
            _player_character.camera_controller.RotateTowards(_mouse_delta);
        }

        #endregion

        #region METHODS

        private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _player_character.Jump();
        }

        private void StartRunning(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (PlayerInputManager.hold_to_run)
            {
                _player_character.StartRunning();
            }
            else
            {
                if (_player_character.running)
                {
                    _player_character.StopRunning();
                }
                else
                {
                    _player_character.StartRunning();
                }
            }
        }

        private void StopRunning(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (PlayerInputManager.hold_to_run)
            {
                _player_character.StopRunning();
            }
        }

        private void StartCrouch(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (PlayerInputManager.hold_to_crouch)
            {
                if (_player_character.current_stance == CharacterStance.Standing)
                {
                    _player_character.Crouch();
                }
            }
            else
            {
                if (_player_character.current_stance == CharacterStance.Standing)
                {
                    _player_character.Crouch();
                }
                else if (_player_character.current_stance == CharacterStance.Crouched)
                {
                    _player_character.Stand();
                }
            }

        }

        private void StopCrouch(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (PlayerInputManager.hold_to_crouch)
            {
                if (_player_character.current_stance == CharacterStance.Crouched)
                {
                    _player_character.Stand();
                }
            }
        }

        #endregion
    }
}
