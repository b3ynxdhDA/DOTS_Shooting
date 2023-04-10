using Unity.Entities;

/// <summary>
/// 弾のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct BulletTag : IComponentData
{
    // 弾の進むスピード
    public float _bulletSpeed;
}
