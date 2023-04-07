using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// プレイヤーのコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct PlayerTag : IComponentData
{
    // プレイヤーの体力
    public float _playerHp;

}
