using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility {

        PowerAttackConfig config;

        public void SetConfig(PowerAttackConfig configToSet) {
            this.config = configToSet;
        }

        public void Use(AbilityUseParams useParams) {
            Debug.Log("Power Attack used, extra damage: " + config.GetExtraDamage());
            Debug.Log("Base damage: " + useParams.baseDamage);
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }
    }
}
