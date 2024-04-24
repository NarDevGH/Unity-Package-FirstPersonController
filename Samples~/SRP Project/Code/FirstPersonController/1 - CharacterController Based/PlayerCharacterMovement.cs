using UnityEngine;
namespace Nardev.Player.CharacterControler.CharacterControllerBased{

    public partial class PlayerCharacterController
    {
        #region PROPERTIES

        [Header("Movement")]
        [SerializeField, Min(0)] private float _walk_speed = 2.5F;
        [SerializeField] private bool _jump_enabled = true;
        [SerializeField, Min(0)] private float _jump_force = 1.5F;
        [SerializeField] private bool _run_enabled = true;
        [SerializeField, Min(0)] private float _run_speed_multiplier = 2f;
        [SerializeField] private bool _crouch_enabled = true;
        [SerializeField, Min(0)] private float _crouch_speed_multiplier = 0.75f;

        [Header("Stances")]
        [SerializeField, Min(0), Tooltip("X: radius, Y: height")] private Vector2 _standing_character_size = new Vector2(0.5f,2f);
        [SerializeField, Min(0), Tooltip("X: radius, Y: height")] private Vector2 _crouch_character_size = new Vector2(0.5f,1f);

        [Header("Physics")]
        [SerializeField, Min(0)] private float _character_mass = 1f;
        [SerializeField, Min(0)] private float _props_push_power = 2f;
        
        private bool _movement_enabled = true;
        private float _target_move_speed;
        private Vector3 _character_velocity;
        
        private bool _running;
        private bool _run_only_forward;

        private bool _performing_bloquing_movement;

        private CharacterStance _current_stance = CharacterStance.Standing;
        private CharacterStance _previus_stance = CharacterStance.Standing;

        private UnityEngine.CharacterController _character_controller;

        #endregion

        #region GETTERS & SETTERS

        public CharacterStance current_stance{
            get{
                return _current_stance;
            }
        }

        public bool running{
            get{
                return _running;
            }
        }

        #endregion

        #region CONTROL_METHODS

        public void EnableMovement() {
            if(_movement_enabled){
                Debug.LogWarning("Movement is already enabled");
                return;
            }

            _movement_enabled = true;
            return;
        }
        public void DisableMovement() {
            if(!_movement_enabled){
                Debug.LogWarning("Movement is already disabled");
                return;
            }

            _movement_enabled = false;
            return;
        }
        public void EnableCrouch() {
            if(_crouch_enabled){
                Debug.LogWarning("Crouch is already enabled");
                return;
            }

            _crouch_enabled= true;
            return;
        }
        public void DisableCrouch() {
            if(!_crouch_enabled){
                Debug.LogWarning("Crouch is already disabled");
                return;
            }

            _crouch_enabled= false;
            return;
        }

        public void EnableRunning() {
            if(_run_enabled){
                Debug.LogWarning("Run is already enabled");
                return;
            }

            _run_enabled= true;
            return;
        }
        
        public void DisablRunning() {
            if(!_run_enabled){
                Debug.LogWarning("Run is already disabled");
                return;
            }

            _run_enabled= false;
            return;
        }
        
        public void EnableRunningOnlyForward() {
            if(_run_only_forward){
                Debug.LogWarning("Run only forward is already enabled");
                return;
            }

            _run_only_forward= true;
            return;
        }
        public void DisableRunningOnlyForward() {
            if(!_run_only_forward){
                Debug.LogWarning("Run only forward is already disabled");
                return;
            }

            _run_only_forward= false;
            return;
        }
        public void EnableJump() {
            if(_jump_enabled){
                Debug.LogWarning("Jump is already enabled");
                return;
            }
            _jump_enabled= true;
            return;
        }
        public void DisableJump() {
            if(!_jump_enabled){
                Debug.LogWarning("Jump is already disabled");
                return;
            }
            _jump_enabled= false;
            return;
        }
    
        #endregion

        #region PUBLIC METHODS

        public void Turn(float delta){
            if(!_movement_enabled) return;

            transform.Rotate(Vector3.up * delta);
            return;
        }

        public void MoveTowardsDirection(Vector2 direction){
            if(!_movement_enabled) return;

            direction *=  _target_move_speed;
            float vel = _character_velocity.y;
            _character_velocity =  transform.right * direction.x + transform.forward * direction.y; 
            _character_velocity.y = vel;
            return;
        }

        public void Jump(){
            if(!_jump_enabled) return;
            if(!_character_controller.isGrounded) return;
            if(_current_stance != CharacterStance.Standing) return;

            _character_velocity.y += Mathf.Sqrt(_jump_force * -3.0f * Physics.gravity.y);
            return;
        }
        
        public void StartRunning(){
            if(!_run_enabled) return;
            if(_performing_bloquing_movement) return;

            _running = true;

            //Id coruched, switch to standing stand

            _target_move_speed *= _run_speed_multiplier;
            return;
        }

        public void StopRunning(){
            _running = false;
            _target_move_speed /= _run_speed_multiplier;
            return;
        }

        public void Stand(){
            if(_performing_bloquing_movement) return;
            if(_current_stance == CharacterStance.Standing) return;
            if(CharacterHaveStuffAbove()) return;

            _character_controller.height = _standing_character_size.y;
            _character_controller.center = Vector3.zero;

            SetCurrentStance(CharacterStance.Standing);
            _target_move_speed = _walk_speed;
            return;

        }

        public void Crouch(){
            if(_performing_bloquing_movement) return;
            if(_current_stance == CharacterStance.Crouched) return;


            _character_controller.height = _crouch_character_size.y;
            if(_character_controller.isGrounded){
                //Reduce height by the top
                _character_controller.center = new Vector3(0, -_crouch_character_size.y/2, 0);
            }
            else{
                //Reduce height by the bottom, to alow crouch jumps
                _character_controller.center = new Vector3(0, _crouch_character_size.y/2, 0);
            }

            SetCurrentStance(CharacterStance.Crouched);
            _target_move_speed *= _crouch_speed_multiplier;
            return;
        }

        #endregion

        #region PRIVATE METHODS

        private void HandleCollideWithCeling()
        {
            if (CharacterHaveStuffAbove())
            {
                _character_velocity.y = 0;
            }
        }

        private bool CharacterHaveStuffAbove()
        {
            RaycastHit hit;
            float distanceEqualsCharacterStanding = _standing_character_size.y - _crouch_character_size.y/2;
            if(Physics.Raycast(transform.position, Vector3.up, out hit, distanceEqualsCharacterStanding)){
                if(hit.collider.gameObject.CompareTag("Player") == false){
                    return true;
                }
            }

            return false;
        }

        private void HandleGravity()
        {
            if (_character_controller.isGrounded && _character_velocity.y < 0)
            {
                _character_velocity.y = -2;
            }
            else
            {
                _character_velocity.y += Physics.gravity.y * _character_mass * Time.deltaTime;
            }
        }

        // this script pushes all rigidbodies that the character touches
        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            PushRigidbodies(hit);
        }

        private void PushRigidbodies(ControllerColliderHit hit)
        {
            UnityEngine.Rigidbody body = hit.collider.attachedRigidbody;

            // no rigidbody
            if (body == null || body.isKinematic)
            {
                return;
            }

            // We dont want to push objects below us
            if (hit.moveDirection.y < -0.3)
            {
                return;
            }

            // Calculate push direction from move direction,
            // we only push objects to the sides never up and down
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // If you know how fast your character is trying to move,
            // then you can also multiply the push velocity by that.

            // Apply the push
            body.velocity = pushDir * _props_push_power * _character_velocity.magnitude;
        }


        protected void SetCurrentStance(CharacterStance stance){
            if(stance == _current_stance){
                Debug.LogError("Stance already set to " + stance);
                return;
            }

            _previus_stance = _current_stance;
            _current_stance = stance;
        }

        #endregion
    }
}