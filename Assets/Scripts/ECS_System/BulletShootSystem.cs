using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// 弾を生成するシステム
/// </summary>
[AlwaysUpdateSystem]
public class BulletShootSystem : SystemBase
{
    // 変数宣言------------------------------------------------------------------
    // 実行タイミングを管理しているシステムグループ
    private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

    // 弾を発射してから経過した時間
    private float _shootCoolTime;

    // 定数宣言--------------------------------------------------------
    // 射撃の間隔
    const float _SHOOT_INTERVAL = 0.15f;

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
        // コマンドバッファを取得
        EntityCommandBuffer comandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        // クールタイムがインターバルより大きかったら弾を発射する
        if(_shootCoolTime > _SHOOT_INTERVAL)
        {
            Entities
                .WithName("Bullet_Shoot")
                .WithAll<GunPortTag>()
                .WithNone<AimGunPortTag>()
                .WithoutBurst()
                .ForEach((in GunPortTag gunporttag, in LocalToWorld localToWorld) =>
                {
                    // PrefabとなるEntityから弾を複製する
                    Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._straightBulletEntity);

                    // 位置の初期化
                    comandBuffer.SetComponent(instantiateEntity, new Translation
                    {
                        Value = localToWorld.Position
                    });

                    // 弾の向きを初期化
                    comandBuffer.SetComponent(instantiateEntity, new Rotation
                    {
                        Value = localToWorld.Rotation
                    });
                    

                }).Run();// メインスレッドで実行

            Entities
                .WithName("Aim_Shoot")
                .WithAll<GunPortTag, AimGunPortTag>()
                .WithoutBurst()
                .ForEach((in GunPortTag gunporttag, in AimGunPortTag aimgunporttag, in LocalToWorld localToWorld) =>
                {
                    // ターゲットのローカル座標を取得
                    //EntityQuery aimTargetEntityQuery = EntityManager.CreateEntityQuery(typeof(EnemyTag));
                    //Entity aimTargetEntity = aimTargetEntityQuery.GetSingletonEntity();
                    LocalToWorld aimTargetLocalToWorld = GetComponent<LocalToWorld>(aimgunporttag._targetEntity);

                    // 発射台から見たプレイヤーの向き
                    float3 direction = math.normalizesafe(aimTargetLocalToWorld.Position - localToWorld.Position);

                    // PrefabとなるEntityから弾を複製する
                    Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._straightBulletEntity);

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

                    // オートエイム弾の進む角度を設定
                    comandBuffer.SetComponent(instantiateEntity, new AutoAimBulletTag
                    {
                        _moveDirection = direction
                    });

                }).Run();// メインスレッドで実行

            // 指定したJob完了後にECBに登録した命令を実行
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);

            // クールタイムを0にする
            _shootCoolTime = 0;
        }

        // クールタイムに経過時間を反映
        _shootCoolTime += Time.DeltaTime;
    }
}
