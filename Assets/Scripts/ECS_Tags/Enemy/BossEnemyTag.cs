using Unity.Entities;

/// <summary>
/// ボスのコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct BossEnemyTag : IComponentData
{
    // ボスの第１形態の時の体力
    public float _1st_bossHp;

    // ボスの第２形態の時の体力
    public float _2nd_bossHp;

    // ボスの第３形態の時の体力
    public float _3rd_bossHp;

}
