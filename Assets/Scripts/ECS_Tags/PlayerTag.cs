using Unity.Entities;

/// <summary>
/// プレイヤーのコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct PlayerTag : IComponentData
{
    // 動くスピード
    public float _moveSpeed;
    // プレイヤーの体力
    public float _playerHp;
}
