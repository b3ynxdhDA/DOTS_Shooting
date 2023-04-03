using Unity.Entities;

/// <summary>
/// 弾の発射地点のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct GunPortTag : IComponentData
{
    // 発射する弾のエンティティ
    public Entity _straightBulletEntity;

    // 連射の間隔
    public float _shootInterval;
}
