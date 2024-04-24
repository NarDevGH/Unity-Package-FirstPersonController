using UnityEngine;
using FirstPersonController.Character;

namespace FirstPersonController.Input
{
    [AddComponentMenu("FirstPersonController/PlayerCharacterInput")]
    public class PlayerCharacterInput : MonoBehaviour
    {
        private IPlayerCharacter _player_character;
        Vector2 _mouse_delta;

        // Start is called before the first frame update
        void Awake()
        {
            _player_character = GetComponent<IPlayerCharacter>(); 
        }

        void Start()
        {
            PlayerInputsManager.input_actions_maps.InGame.Enable();

            PlayerInputsManager.LockCursor();

        }
        
        // Update is called once per frame
        void Update()
        {
            if(PlayerInputsManager.input_actions_maps.InGame.enabled == false) return;

            _player_character.MoveTowardsDirection(PlayerInputsManager.input_actions_maps.InGame.Move.ReadValue<Vector2>());

            //Get mouse delta values
            _mouse_delta = PlayerInputsManager.input_actions_maps.InGame.LookAround.ReadValue<Vector2>();

            //Apply sensitivity to mouse
            _mouse_delta.x *= PlayerInputsManager.mouse_sensitivity;
            _mouse_delta.y *= PlayerInputsManager.mouse_sensitivity;

            _player_character.Turn(_mouse_delta.x);
            _player_character.camera_controller.RotateTowards(_mouse_delta);

            if (PlayerInputsManager.input_actions_maps.InGame.Jump.IsPressed())
            {
                _player_character.Jump();
            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.LeftControl)){
                if(_player_character.current_stance == CharacterStance.Standing){
                    _player_character.Crouch();
                }
                else if(_player_character.current_stance == CharacterStance.Crouched){
                    _player_character.Stand();
                }
            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.LeftShift)){
                if(PlayerInputsManager.hold_to_run is true){
                    _player_character.StartRunning();
                }
                else{
                    if(_player_character.running){
                        _player_character.StopRunning();
                    }
                    else{
                        _player_character.StartRunning();
                    }
                }
            }

            if(UnityEngine.Input.GetKeyUp(KeyCode.LeftShift)){
                if(PlayerInputsManager.hold_to_run is true){
                    _player_character.StopRunning();
                }
            }
        }
    }
}