using UnityEngine;

namespace RPG.Core {
    public class AudioTrigger : MonoBehaviour {
        [SerializeField] AudioClip clip;
        [SerializeField] float audioVolume = 1f;
        [SerializeField] int layerFilter = 0;
        [SerializeField] float triggerRadius = 5f;
        [SerializeField] bool isOneTimeOnly = true;

        [SerializeField] bool hasPlayed = false;
        AudioSource audioSource;

        void Start() {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = clip;
            audioSource.volume = audioVolume;

            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = triggerRadius;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == layerFilter) {
                RequestPlayAudioClip();
            }
        }

        void RequestPlayAudioClip() {
            if (isOneTimeOnly && hasPlayed) {
                return;
            } else if (!audioSource.isPlaying) {
                audioSource.Play();
                hasPlayed = true;
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = new Color(255f, 255f, 255f, .5f);
            Gizmos.DrawWireSphere(transform.position, triggerRadius);
        }
    }
}
