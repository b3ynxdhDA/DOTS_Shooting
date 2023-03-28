using Unity.Entities;

/// <summary>
/// 弾のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct PlayerTag : IComponentData
{
    // 動くスピード
    public float _moveSpeed;
}
