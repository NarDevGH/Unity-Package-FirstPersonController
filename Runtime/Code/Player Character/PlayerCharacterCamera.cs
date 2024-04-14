using UnityEngine;

namespace FirstPersonController.Camera
{
    [AddComponentMenu("FirstPersonController/PlayerCamera")]
    public class PlayerCharacterCamera : MonoBehaviour
    {
        #region PROPERTIES

        [SerializeField] private bool _independent_horizontal_rotation = false;
        [Space]
        [SerializeField] private float _min_vertical_rotation = -45;
        [SerializeField] private float _max_vertical_rotation = 45;
        [SerializeField] private float _min_horizontal_rotation = -90;
        [SerializeField] private float _max_horizontal_rotation = 90;
        
        [Header("Setup")]
        [SerializeField] private UnityEngine.Camera _camera;

        private float _vertical_rotation;
        private float _horizontal_rotation;

        #endregion
        #region GETTERS & SETTERS

        public bool independent_horizontal_rotation{
            get {
                return _independent_horizontal_rotation;
            }
            set{
                _independent_horizontal_rotation = value;
            }
        }
        public UnityEngine.Camera camera{
            get {
                return _camera;
            }
        }

        #endregion

        #region CONTROL METHODS

        public void EnableIndependentHorizontalRotation(){
            _independent_horizontal_rotation = true;
        }

        public void DisableIndependentHorizontalRotation(){
            _independent_horizontal_rotation = false;

            //Reset horizontal camera rotation to be align with the character body
            _horizontal_rotation = 0f;
        }

        #endregion

        
        #region PUBLIC METHODS

        public void RotateTowards(Vector2 rotation){
            if(_independent_horizontal_rotation){
                _horizontal_rotation += rotation.x;
                _horizontal_rotation = Mathf.Clamp(_horizontal_rotation, _min_horizontal_rotation, _max_horizontal_rotation);
            }

            _vertical_rotation -= rotation.y;
            _vertical_rotation = Mathf.Clamp(_vertical_rotation, _min_vertical_rotation, _max_vertical_rotation);

            _camera.transform.localRotation = Quaternion.Euler(_vertical_rotation, _horizontal_rotation, _camera.transform.localRotation.z);
        }

        #endregion
        #region UNITY METHODS

        public void Awake(){
            _vertical_rotation = _camera.transform.localEulerAngles.x;
            _horizontal_rotation = _camera.transform.localEulerAngles.y;
        }
        #endregion
    }
}