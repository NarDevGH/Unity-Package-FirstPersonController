using UnityEngine;

namespace Nardev.Player.CharacterControler.RigidbodyBased{
    [AddComponentMenu("FirstPersonController/2 - Rigidbody Based/CharacterController")]
    [RequireComponent(typeof(PlayerCharacterInput))]
    [RequireComponent(typeof(Rigidbody))]
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
            _rigidbody = GetComponent<UnityEngine.Rigidbody>();
            _target_move_speed = _walk_speed;
        }

        void Update()
        {

        }

        private void OnCollisionStay(Collision collision)
        {
            if(collision.transform.CompareTag("Ground"))
            {
                _is_grounded = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.transform.CompareTag("Ground"))
            {
                _is_grounded = false;
            }
        }

        #endregion
    }
}