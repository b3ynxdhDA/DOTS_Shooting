using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// 弾を生成するシステム
/// </summary>
public class AimShootSystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    /// <summary>
    /// システム作成時に呼ばれる処理
    /// </summary>
    protected override void OnCreate()
    {
        // EntityCommandBufferの取得
        _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    /// <summary>
    /// システム有効時にフレーム毎に呼ばれる処理
    /// </summary>
    protected override void OnUpdate()
    {
        // ゲームのステートがゲーム中以外なら処理しない
        if (GameManager.instance.gameState != GameManager.GameState.GameNow)
        {
            return;
        }

        // コマンドバッファを取得
        EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        Entities
            .WithName("Aim_Shoot")
            .WithAll<GunPortTag, AimGunPortTag>()
            .WithoutBurst()
            .ForEach((ref GunPortTag gunporttag, in AimGunPortTag aimgunporttag, in LocalToWorld localToWorld) =>
            {
                // クールタイムが0より小さかったら弾を発射する
                if (gunporttag._shootInterval < 0)
                {
                    // 発射台から見たターゲットの向きを上向きにして初期化
                    float3 direction = localToWorld.Up;

                    // ターゲットのエンティティが存在するとき
                    if (aimgunporttag._targetEntity != Entity.Null)
                    {
                        // ターゲットのローカル座標を取得
                        ComponentDataFromEntity<Translation> translationComponentData = GetComponentDataFromEntity<Translation>(true);
                        float3 targetPosition = translationComponentData[aimgunporttag._targetEntity].Value;

                        // 発射台から見たターゲットの向きを計算
                        direction = math.normalizesafe(targetPosition - localToWorld.Position);
                    }
                    // PrefabのEntityから弾を複製する
                    Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._BulletEntity);

                    // 位置の初期化
                    comandBuffer.SetComponent(instantiateEntity, new Translation
                    {
                        Value = localToWorld.Position
                    });

                    // 弾の向きを初期化
                    comandBuffer.SetComponent(instantiateEntity, new Rotation
                    {
                        Value = quaternion.LookRotationSafe(direction, math.forward())
                    });

                    // 弾の進む角度を設定
                    comandBuffer.SetComponent(instantiateEntity, new BulletMoveDirectionTag
                    {
                        _moveDirection = direction
                    });

                    // 弾の進む速度を設定
                    comandBuffer.SetComponent(instantiateEntity, new BulletTag
                    {
                        _bulletSpeed = gunporttag._bulletSpeed
                    });
                    // インターバルをセットする
                    gunporttag._shootInterval = gunporttag._shootCoolTime;
                }
                // インターバルから経過時間を減らす
                gunporttag._shootInterval -= Time.DeltaTime;

            }).Run();// メインスレッドで実行

        // 指定したJob完了後にECBに登録した命令を実行
        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

}
