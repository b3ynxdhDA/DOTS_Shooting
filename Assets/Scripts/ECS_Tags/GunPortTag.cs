using Unity.Entities;

/// <summary>
/// 弾の発射地点のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct GunPortTag : IComponentData
{
    // 発射する弾のエンティティ
    public Entity _straightBulletEntity;

    // 発射する弾のエンティティ
    public Entity _aimBulletEntity;

    // 何列で弾を発射するか
    public int _gunPortAmount;

    public enum BulletKind
    {
        Straight,
        Diffusion,
        Aim
    }

    public BulletKind bulletKind;
}
