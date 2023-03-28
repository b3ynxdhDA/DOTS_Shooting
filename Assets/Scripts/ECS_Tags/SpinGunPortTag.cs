using Unity.Entities;

/// <summary>
/// 回転しながら弾を発射するエンティティのコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct SpinGunPortTag : IComponentData
{
    // 回転する速度
    public float _spinSpeed;
}
