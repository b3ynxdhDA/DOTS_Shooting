using Unity.Entities;

/// <summary>
/// キャラクターのHPのコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct HPTag : IComponentData
{
    // プレイヤーの体力
    public float _hp;
}
