using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerCharacter2D))]
    public class PlayerUserControll : MonoBehaviour
    {
        private PlayerCharacter2D _character;
        private bool _jump;


        private void Awake()
        {
            _character = GetComponent<PlayerCharacter2D>();
        }


        private void Update()
        {
            if (!_jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                _jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            var h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            _character.Move(h, _jump);
            _jump = false;
        }
    }
}
