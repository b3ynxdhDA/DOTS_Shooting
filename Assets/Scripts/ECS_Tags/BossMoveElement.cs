using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// オートエイム弾のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
[InternalBufferCapacity(3)]
public struct BossMoveElement : IBufferElementData
{
    // ターゲットへの角度
    public Entity _movePoints;
}
