using UnityEngine;

namespace NarDev.Player.Input{
    [AddComponentMenu("Input/PlayerInputsManager")]
    public class PlayerInputManager : MonoBehaviour
    {

        #region PROPERTIES

        private static bool _hold_to_run;
        private static bool _hold_to_crouch;

        private static float _mouse_sensitivity = 0.1f;

        private static @PlayerInputActions _input_actions_maps;

        #endregion

        #region GETTERS & SETTERS

        public static bool hold_to_run{
            get{
                return _hold_to_run;
            }
            set{
                _hold_to_run = value;
            }
        }
        public static bool hold_to_crouch{
            get{
                return _hold_to_crouch;
            }
            set{
                _hold_to_crouch = value;
            }
        }


        public static PlayerInputActions input_actions_maps{
            get { return _input_actions_maps;}
        }

        public static float mouse_sensitivity{
            get { return _mouse_sensitivity;}
            set{
                if(value < 0){
                    Debug.LogWarning("Mouse sensitivity must be greater than 0");
                    return;
                }

                _mouse_sensitivity = value;
                return;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public static void ShowCursor(){
            Cursor.visible = true;
        }
        public static void HideCursor(){
            Cursor.visible = false;
        }

        public static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        public static void DisableCursorLock()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        #endregion
        #region  UNITY METHODS

        void Awake()
        {
            _input_actions_maps = new PlayerInputActions();
        }

        #endregion
    }
}