using Unity.Entities;
using UnityEngine;

/// <summary>
/// 弾のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct BulletTag : IComponentData
{
    // 弾の進むスピード
    public float _bulletSpeed;

    // 弾の与えるダメージ
    public float _hitDamage;
}
