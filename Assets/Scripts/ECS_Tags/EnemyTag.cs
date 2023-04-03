using Unity.Entities;

/// <summary>
/// 敵のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct EnemyTag : IComponentData
{
    // 敵の体力
    public float _enemyHp;

}
