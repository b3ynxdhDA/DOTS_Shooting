using Unity.Entities;
using Unity.Rendering;

/// <summary>
/// 駒を管理するクラス
/// </summary>
public class KomaManager
{
    /// <summary>
    /// ゲーム開始時にGameManagerから呼ばれるKomaManagerの参照メソッド
    /// </summary>
    public KomaManager()
    {

    }

    /// <summary>
    /// ボスの駒のステータスを設定する
    /// </summary>
    /// <param name="entity">対象のエンティティ</param>
    /// <param name="hpTag">対象のHPTagコンポーネント</param>
    /// <param name="gunPortTag">対象のGunPortTagコンポーネント</param>
    /// <param name="komaData">設定するKomaDate</param>
    public void SetKomaDate(Entity entity, KomaData komaData, GunPortTag gunPortTag, EntityCommandBuffer commandBuffer)
    {
        // ボスの基礎ステータスを設定する
        commandBuffer.SetComponent(entity, new HPTag
        {
            _hp = komaData.hp
        });

        // セットコンポーネントだとエンティティプレハブがNullになるので
        gunPortTag._shootCoolTime = komaData.shootCoolTime;
        gunPortTag._bulletSpeed = komaData.bulletSpeed;

        // マテリアルを変更
        commandBuffer.SetSharedComponent(entity, new RenderMesh
        {
            mesh = GameManager.instance.Quad,
            material = komaData.material
        });

        // 射撃の種類のコンポーネントを設定する
        // 次にセットするGunPortの種類は何か
        switch (komaData.shootKind)
        {
            case KomaData.ShootKind.StraightGunPortTag:

                // 既にあるGunPortの種別タグを削除する
                commandBuffer.RemoveComponent<WideGunPortTag>(entity);
                commandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // StraightGunPortTagを追加する
                commandBuffer.AddComponent(entity, new StraightGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.WideGunPortTag:

                // 既にあるGunPortの種別タグを削除する
                commandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                commandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // WideGunPortTagを追加する
                commandBuffer.AddComponent(entity, new WideGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.AimGunPortTag:

                // 既にあるGunPortの種別タグを削除する
                commandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                commandBuffer.RemoveComponent<WideGunPortTag>(entity);

                // AimGunPortTagを追加する
                commandBuffer.AddComponent(entity, new AimGunPortTag { });
                break;
        }
    }
}