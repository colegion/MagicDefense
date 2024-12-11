using System;
using System.Collections;
using DG.Tweening;
using Helpers;
using UnityEngine;

namespace Spells
{
    public class Fireball : Spell
    {
        [SerializeField] private GameObject spellMesh;
        [SerializeField] private ParticleSystem explosionEffect;
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private LayerMask damageableLayer;

        private Vector3 _direction;
        private Coroutine _moveRoutine;

        public override void Move(Transform target)
        {
            _direction = GetDirectionToRandomEnemy();
            spellMesh.gameObject.SetActive(true);

            var moveDuration = Utilities.BASE_MOVE_DURATION / Config.spellSettings.speed;
            _moveRoutine = StartCoroutine(MoveInDirection(moveDuration));
        }

        private IEnumerator MoveInDirection(float moveDuration)
        {
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                Debug.Log($"Moving to {_direction}, Position: {transform.position}");

                transform.position += _direction * (Config.spellSettings.speed * Time.deltaTime);
                if (Physics.Raycast(transform.position, _direction, out var hit, Config.spellSettings.speed * Time.deltaTime))
                {
                    Debug.Log($"Raycast hit: {hit.collider.name}");
                    HandleCollision(hit.collider);
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            Explode();
        }

        private Vector3 GetDirectionToRandomEnemy()
        {
            var onScreenEnemies = GameController.Instance.GetOnScreenEnemies();

            if (onScreenEnemies.Count == 0)
            {
                float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
                return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;
            }

            var randomEnemy = onScreenEnemies[UnityEngine.Random.Range(0, onScreenEnemies.Count)];
            Vector3 directionToEnemy = (randomEnemy.transform.position - transform.position).normalized;
            directionToEnemy.y = 0;
            return directionToEnemy;
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
            StopCoroutine(_moveRoutine);
            spellMesh.gameObject.SetActive(false);
            explosionEffect.gameObject.SetActive(true);
            explosionEffect.Play();
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
        
        public override void ReturnToPool()
        {
            Config = null;
            DOVirtual.DelayedCall(.5f, () =>
            {
                explosionEffect.gameObject.SetActive(false);
                gameObject.SetActive(false);
            });
        }
    }
}