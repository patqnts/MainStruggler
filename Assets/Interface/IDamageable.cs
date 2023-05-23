using UnityEngine;
public interface IDamageable
{
    public float Health { set; get; }
    public void OnHit(float damage, Vector2 knockback);
    public void OnHit(float damage);

    public void OnBurn(float damage, float time);

    public void OnDark(float time);

}