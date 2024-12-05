using System;
using System.Collections;
using Helpers;
using UnityEngine;

namespace Spells
{
    public class Fireball : Spell
    {
        [SerializeField] private float explosionRadius = 5f; // Radius of the explosion
        [SerializeField] private LayerMask damageableLayer; // Layer mask to identify enemies

        private Vector3 _direction;

        public override void Move(Transform target)
        {
            // Choose a random direction or calculate based on your game's logic
            _direction = GetRandomDirection();

            var moveDuration = Utilities.BASE_MOVE_DURATION / Config.spellSettings.speed;

            // Move in the chosen direction
            StartCoroutine(MoveInDirection(moveDuration));
        }

        private IEnumerator MoveInDirection(float moveDuration)
        {
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                // Move the fireball forward in the chosen direction
                transform.position += _direction * (Config.spellSettings.speed * Time.deltaTime);

                // Check for collision using a Raycast
                if (Physics.Raycast(transform.position, _direction, out var hit,
                        Config.spellSettings.speed * Time.deltaTime))
                {
                    HandleCollision(hit.collider);
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Explode when the fireball reaches the ground or exceeds its range
            Explode();
        }

        private Vector3 GetRandomDirection()
        {
            // Choose a random angle around the tower and convert to direction
            float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // Random angle in radians
            return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized; // Convert angle to 2D direction
        }

        private void HandleCollision(Collider other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                Explode(); // Explode on enemy hit
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Explode(); // Explode on ground hit
            }
        }

        private void Explode()
        {
            // Detect enemies in the explosion radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, damageableLayer);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(Config.spellSettings.damage); // Apply damage to each enemy
                }
            }

            // Add explosion effect here (e.g., particle system, sound)
            Debug.Log("Explosion at: " + transform.position);

            ReturnToPool(); // Return the fireball to the pool
        }

        private void OnDrawGizmos()
        {
            // Visualize the explosion radius in the editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
