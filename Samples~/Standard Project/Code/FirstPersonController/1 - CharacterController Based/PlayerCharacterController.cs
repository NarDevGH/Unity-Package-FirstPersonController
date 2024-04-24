using UnityEngine;
using FirstPersonController.Camera;
using FirstPersonController.Input;

namespace FirstPersonController.Character.CharacterControler{
    [AddComponentMenu("FirstPersonController/1 - CharacterController Based/CharacterController")]
    [RequireComponent(typeof(PlayerCharacterInput))]
    public partial class PlayerCharacterController : MonoBehaviour, IPlayerCharacter
    {
        #region PROPERTIES

        [SerializeField, Min(0)] private float _health = 100;
        [SerializeField] private PlayerCharacterCamera _camera_controller;

        #endregion
        #region GETTERS & SETTERS

        public float health {
            get {
                return _health;
            }
        }

        public PlayerCharacterCamera camera_controller {
            get {
                return _camera_controller;
            }
        }

        #endregion
        #region  PUBLIC METHODS

        public void TakeDamage(float ammount){
            if(_health <= 0) return;

            _health = _health-ammount < 0 ? 0 : _health - ammount;

            if(_health == 0){
                DyingRoutine();
            }
        }

        #endregion
        #region PRIVATE METHODS

        protected void DyingRoutine(){
            Debug.Log("Players is Dead");
        }

        #endregion
        #region UNITY METHODS

        void Start()
        {
            _character_controller = GetComponent<CharacterController>();
            _target_move_speed = _walk_speed;
        }

        void Update()
        {
            HandleGravity();

            HandleCollideWithCeling();

            // apply movement
            _character_controller.Move(_character_velocity * Time.deltaTime);
        }

        #endregion
    }
}