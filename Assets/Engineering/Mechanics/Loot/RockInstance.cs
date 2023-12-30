using Sirenix.OdinInspector;
using UnityEngine;

public class RockInstance : WeaponInstance
{
    [Title("Rock Instance Properties")]
    [SerializeField] float damage;
    [SerializeField] TrailRenderer[] trails;
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] LayerMask defaultLayer;
    bool projectileActive = false;
    private void OnCollisionEnter(Collision collision) {
        if (!projectileActive) { return; }
        if (collision.transform.TryGetComponent(out IFestiveTarget target)) {
            target.OnHit(new HitData(collision.GetContact(0).point, weaponItemRB.velocity, damage));
            foreach (TrailRenderer trail in trails) {
                trail.Clear();
                trail.enabled = false;
            }
            foreach (ParticleSystem particle in particles) {
                particle.Clear();
                particle.Stop();
            }
            projectileActive = false;

            Debug.Assert(((int)defaultLayer).IsPowerOfTwo(), "Only 1 layer in mask please");
            SetHighlightLayers(defaultLayer);
            highlightActive = false;
            DestroyAfterDelay(5.0f);
        }
    }

    public override void Start() {
        base.Start();
        projectileActive = true;


        foreach (TrailRenderer trail in trails) {
            trail.Clear();
            trail.enabled = true;
        }

        foreach (ParticleSystem particle in particles) {
            particle.Clear();
            particle.Play();
        }
    }
}
