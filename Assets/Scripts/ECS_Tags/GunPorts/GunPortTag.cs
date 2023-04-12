using Unity.Entities;
using UnityEngine;

/// <summary>
/// 弾の発射地点のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct GunPortTag : IComponentData
{
    // 発射する弾のエンティティ
    public Entity _straightBulletEntity;

    // 連射の間隔
    public float _shootCoolTime;

    // 連射のクールタイム(発射地点ごとに連射のクールタイムを変えるためTagが保持する)
    [HideInInspector] public float _shootInterval;

    // 発射する弾の速度
    public float _bulletSpeed;
}
