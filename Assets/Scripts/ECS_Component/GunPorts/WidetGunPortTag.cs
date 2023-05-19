using Unity.Entities;

/// <summary>
/// 真っ直ぐ弾を発射するエンティティのコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct WideGunPortTag : IComponentData
{
    // 砲身の数
    public int _lines;
}
