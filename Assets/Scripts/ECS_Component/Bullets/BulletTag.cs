using Unity.Entities;
using UnityEngine;

/// <summary>
/// 弾のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct BulletTag : IComponentData
{
    // 弾の進むスピード
    [HideInInspector] public float _bulletSpeed;
}
