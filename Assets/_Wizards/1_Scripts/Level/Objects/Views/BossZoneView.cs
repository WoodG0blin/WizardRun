using UnityEngine;

namespace WizardsPlatformer
{
    public class BossZoneView : LevelObjectView
    {
        [SerializeField] private Collider _triggerCollider;
        [SerializeField] private Collider _firstCollider;
        [SerializeField] private Collider _lastCollider;
        [SerializeField] private Transform _bossPoint;

        public void SetColliders(bool active)
        {
            _triggerCollider.enabled = !active;

            _firstCollider.enabled = active;
            _firstCollider.gameObject.SetActive(active);

            _lastCollider.enabled = active;
            _lastCollider.gameObject.SetActive(active);
        }

        public GameObject SetBoss(GameObject prefab) => Instantiate(prefab, _bossPoint);
    }
}
