using Unity.Entities;

/// <summary>
/// 敵のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct EnemyTag : IComponentData
{
    // 動くスピード
    public float _enemyHp;

}
