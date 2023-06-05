using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// プレイヤーのコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct PlayerMoveDate : IComponentData
{
    // 動くスピード
    public float _moveSpeed;

    // 低速モードのスピード
    public float _slowSpeed;

    // 動く方向
    public float3 _moveDirection;
}
