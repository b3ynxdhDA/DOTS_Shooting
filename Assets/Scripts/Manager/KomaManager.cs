using Unity.Entities;
using Unity.Rendering;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#if UNITY_EDITOR
using UnityEngine;
#endif

/// <summary>
/// 駒を管理するクラス
/// </summary>
public class KomaManager
{
    private System.Threading.Tasks.Task<KomaData> normalKoma;

    private Dictionary<GameManager.KomaKind, KomaData> _normalkoma = new Dictionary<GameManager.KomaKind, KomaData>();


    /// <summary>
    /// ゲーム開始時にGameManagerから呼ばれるKomaManagerの参照メソッド
    /// </summary>
    public KomaManager()
    {
        normalKoma = Addressables.LoadAssetAsync<KomaData>("tokin.asset").Task;
        Debug.Log(normalKoma);
    }

    /// <summary>
    /// ボスの駒のステータスを設定する
    /// </summary>
    /// <param name="entity">対象のエンティティ</param>
    /// <param name="hpTag">対象のHPTagコンポーネント</param>
    /// <param name="gunPortTag">対象のGunPortTagコンポーネント</param>
    /// <param name="komaData">設定するKomaDate</param>
    public void SetKomaDate(Entity entity, ref HPTag hpTag, ref GunPortTag gunPortTag, KomaData komaData, EntityCommandBuffer commandBuffer)
    {
        // ボスの基礎ステータスを設定する
        hpTag._hp = komaData.hp;
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