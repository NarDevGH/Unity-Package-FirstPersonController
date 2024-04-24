using UnityEngine;

namespace Nardev.Player.CharacterControler{

    public enum CharacterStance {Standing, Crouched}

    public interface IPlayerCharacter
    {
        public float health { get;}
        public PlayerCharacterCamera camera_controller{get;}
        public CharacterStance current_stance { get;}
        public bool running { get;}

        public void TakeDamage(float ammount);

        public void MoveTowardsDirection(Vector2 direction);
        public void Turn(float delta);
        public void StartRunning();
        public void StopRunning();    
        public void Jump();
        public void Crouch();
        public void Stand();

        public void EnableMovement();
        public void DisableMovement();
        public void EnableCrouch();
        public void DisableCrouch();

        public void EnableRunning();
        
        public void DisablRunning();
        
        public void EnableRunningOnlyForward();
        public void DisableRunningOnlyForward();
        public void EnableJump();
        public void DisableJump();
    }
}