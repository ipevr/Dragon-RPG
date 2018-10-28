using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    [CreateAssetMenu(menuName = "RPG/Special Ability/Area Effect")]
    public class AreaEffektConfig : SpecialAbility {

        [Header("Area Effect Specific")]
        [SerializeField] float radius = 5f;
        [SerializeField] float damageToEachTarget = 10f;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo) {
            AreaEffektBehaviour behaviourComponent = gameObjectToAttachTo.AddComponent<AreaEffektBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public float GetRadius() {
            return radius;
        }

        public float GetDamageToEachTarget() {
            return damageToEachTarget;
        }

    }

}
