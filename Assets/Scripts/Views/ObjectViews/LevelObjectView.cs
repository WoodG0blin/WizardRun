using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class LevelObjectView : View, ILevelObjectView
    {
        protected Stats stats;
        protected ILevelObjectConfig config;
        protected bool activeResponse = false;
        protected float kickCoeff = 2f;

        public void Init(ILevelObjectConfig config)
        {
            this.config = config;
            OnInit();
        }

        protected virtual void OnInit() { }

        public void Draw(Vector3 position)
        {
            SetPosition(position);
            SetActive(true);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (activeResponse && collision.transform.CompareTag("Player"))
            {
                if (stats != null)
                {
                    collision.transform.GetComponent<IDamagable>().ReceiveDamage(stats.MaxHealth / 4);
                    if (this is IDamagable damagable) damagable.ReceiveDamage(stats.MaxHealth / 4);
                }
                collision.rigidbody.AddForce(Vector2.up * kickCoeff, ForceMode2D.Impulse);
            }
            OnCollision(collision);
        }

        protected virtual void OnCollision(Collision2D collision) { }
    }
}