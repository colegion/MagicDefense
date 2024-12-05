using System;
using System.Collections;
using Helpers;
using UnityEngine;

namespace Spells
{
    public class Fireball : Spell
    {
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private LayerMask damageableLayer;

        private Vector3 _direction;

        public override void Move(Transform target)
        {
            _direction = GetRandomDirection();

            var moveDuration = Utilities.BASE_MOVE_DURATION / Config.spellSettings.speed;
            StartCoroutine(MoveInDirection(moveDuration));
        }

        private IEnumerator MoveInDirection(float moveDuration)
        {
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                transform.position += _direction * (Config.spellSettings.speed * Time.deltaTime);
                if (Physics.Raycast(transform.position, _direction, out var hit,
                        Config.spellSettings.speed * Time.deltaTime))
                {
                    HandleCollision(hit.collider);
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
            
            Explode();
        }

        private Vector3 GetRandomDirection()
        {
            float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;
        }

        private void HandleCollision(Collider other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                Explode();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer($"Ground"))
            {
                Explode();
            }
        }

        private void Explode()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, damageableLayer);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.TakeDamage(Config.spellSettings.damage);
                }
            }
            
            Debug.Log("Explosion at: " + transform.position);

            ReturnToPool();
        }

        private void OnDrawGizmos()
        {
            // Visualize the explosion radius in the editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
