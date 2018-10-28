using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters {
    public class AreaEffektBehaviour : MonoBehaviour, ISpecialAbility {

        AreaEffektConfig config;

        public void SetConfig(AreaEffektConfig configToSet) {
            this.config = configToSet;
        }

        public void Use(AbilityUseParams useParams) {
            Debug.Log("Area Effect used");
            Ray ray = new Ray(gameObject.transform.position, Vector3.up);
            RaycastHit[] rayCastHits = Physics.SphereCastAll(ray, config.GetRadius(), 0f, LayerMask.GetMask("Enemy"));
            foreach (RaycastHit rayCastHit in rayCastHits) {
                IDamageable damageable = rayCastHit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null) {
                    float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget();
                    damageable.TakeDamage(damageToDeal);
                }
            }


        }

    }
}
