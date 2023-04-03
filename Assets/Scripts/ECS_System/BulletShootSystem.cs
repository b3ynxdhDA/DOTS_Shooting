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

    // 射撃の間隔
    private float _ShootInterval = 0.15f;

    // 定数宣言--------------------------------------------------------
    // 射撃する列の間隔
    const float _SHOOT_SPACE = 0.5f;

    // 射撃する角度の間隔
    const float _SHOOT_RAD = 1.5f;
    // 円周率
    const float _PI = math.PI;

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
        if(_shootCoolTime > _ShootInterval)
        {
            Entities
                .WithName("Straight_Shoot")
                .WithAll<GunPortTag, StraightGunPortTag>()
                .WithoutBurst()
                .ForEach((in GunPortTag gunporttag, in StraightGunPortTag straightTag, in LocalToWorld localToWorld) =>
                {
                    // 発射する弾の列の数だけループ
                    for (int i = 0; i < straightTag._lines; i++)
                    {
                        // PrefabとなるEntityから弾を複製する
                        Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._straightBulletEntity);

                        // 発射の列ごとの位置を計算する
                        float3 pos = new float3(localToWorld.Position.x + i - _SHOOT_SPACE * straightTag._lines + _SHOOT_SPACE, localToWorld.Position.y, 0f);
                        
                        // 位置の初期化
                        comandBuffer.SetComponent(instantiateEntity, new Translation
                        {
                            Value = pos
                        });

                        // 弾の向きを初期化
                        comandBuffer.SetComponent(instantiateEntity, new Rotation
                        {
                            Value = localToWorld.Rotation
                        });

                    }
                }).Run();// メインスレッドで実行

            Entities
                .WithName("Wide_Shoot")
                .WithAll<GunPortTag, WideGunPortTag>()
                .WithoutBurst()
                .ForEach((in GunPortTag gunporttag, in WideGunPortTag WideTag, in LocalToWorld localToWorld) =>
                {
                    // 発射する弾の列の数だけループ
                    for (int i = 0; i < WideTag._lines; i++)
                    {
                        // PrefabとなるEntityから弾を複製する
                        Entity instantiateEntity = comandBuffer.Instantiate(gunporttag._straightBulletEntity);

                        // 発射地点のローカル座標
                        float3 localPos = localToWorld.Position;

                        // 発射角度     弧度法:(_PI / 12) = 度数法:15度、弧度法:(_PI / 24) = 度数法:7.5度
                        float angle = i * (_PI / 12) - WideTag._lines * (_PI / 24) + (_PI / 24);

                        // 発射角を求めるための座標
                        float3 position = new float3(localPos.x + _SHOOT_RAD * math.cos(angle), localPos.y + _SHOOT_RAD * math.sin(angle), localPos.z);
                        // 発射源からみた座標の向き
                        float3 diff = math.normalizesafe(position - localToWorld.Position);

                        // 位置の初期化
                        comandBuffer.SetComponent(instantiateEntity, new Translation
                        {
                            Value = localPos
                        });

                        // 弾の向きを初期化
                        comandBuffer.SetComponent(instantiateEntity, new Rotation
                        {
                            Value = quaternion.LookRotationSafe(diff, math.up())
                        });

                    }
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
