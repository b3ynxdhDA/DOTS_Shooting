using Unity.Entities;
#if UNITY_EDITOR
using UnityEngine;
#endif

/// <summary>
/// 弾の発射地点のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct GunPortTag : IComponentData
{
    // 発射する弾のエンティティ
    public Entity _BulletEntity;

    // 連射の間隔
    [HideInInspector] public float _shootCoolTime;

    // 連射のクールタイム(発射地点ごとに連射のクールタイムを変えるためTagが保持する)
    [HideInInspector] public float _shootInterval;

    // 発射する弾の速度
    [HideInInspector] public float _bulletSpeed;
}
