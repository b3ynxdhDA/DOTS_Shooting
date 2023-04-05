using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// ボスキャラクターの移動地点のコンポーネントバッファー
/// </summary>
[GenerateAuthoringComponent]
[InternalBufferCapacity(3)]
public struct BossMoveElement : IBufferElementData
{
    // ボスキャラクターの移動する位置
    public float3 _movePoints;
}
