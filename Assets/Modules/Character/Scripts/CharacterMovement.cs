using UnityEngine;

namespace Potions.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour
    {
        public void Move(Vector2 dir) => _targetDirection = dir;
        public void Stop() => Move(Vector2.zero);

        private void Awake() => _rb = GetComponent<Rigidbody2D>();
        private void Update() => _direction = Vector2.Lerp(_direction, _targetDirection, Time.deltaTime * _speedSmooth);
        private void FixedUpdate() => _rb.velocity = _direction * _speed;

        [SerializeField] private float _speed;
        [SerializeField] private float _speedSmooth;

        private Rigidbody2D _rb;
        private Vector2 _direction;
        private Vector2 _targetDirection;
    }
}