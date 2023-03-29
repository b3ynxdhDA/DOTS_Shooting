using Unity.Entities;

/// <summary>
/// ターゲットを狙う弾を発射するエンティティのコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct AimGunPortTag : IComponentData
{
    // ターゲットのエンティティ
    public Entity _targetEntity;
}
