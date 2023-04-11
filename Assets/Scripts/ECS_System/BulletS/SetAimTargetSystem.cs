using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

/// <summary>
/// プレイヤーがオートエイム弾のターゲットを一番近い敵に設定する
/// </summary>
public class SetAimTargetSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        // プレイヤーのAimGunPortにターゲット(敵)を設定する
        Entities
            .WithAll<PlayerTag,AimGunPortTag>()
            .ForEach((ref AimGunPortTag aimGunPort, ref Translation playerTranslation) =>
            {
                // 一番近くに居る敵
                Entity closestTargetEntity = Entity.Null;
                float closestDistance = float.MaxValue;

                // プレイヤーのポジションを
                float3 playerPosition = playerTranslation.Value;

                Entities
                    .WithAll<EnemyTag>()
                    .ForEach((Entity targetEntity, ref Translation targetTranslation) =>
                    {
                        // プレイヤーからターゲット中の敵までの距離
                        float enemyDistance = math.distance(playerPosition, targetTranslation.Value);

                        // ターゲットしている敵がNULLなら
                        if(closestTargetEntity == Entity.Null)
                        {
                            // 一番近くの敵をターゲットに設定する
                            closestTargetEntity = targetEntity;
                            closestDistance = enemyDistance;
                        }
                        else
                        {
                            // ターゲット中の敵より近くに別の敵が居たら
                            if(enemyDistance < closestDistance)
                            {
                                closestTargetEntity = targetEntity;
                                closestDistance = enemyDistance;
                            }
                        }
                    });
                // コンポーネントに設定する
                aimGunPort._targetEntity = closestTargetEntity;
            });

        // 敵のAimGunPortにターゲットを(プレイヤー)を設定する
        Entities
            .WithAll<EnemyTag,AimGunPortTag>()
            .ForEach((ref AimGunPortTag aimGunPort, ref Translation playerTranslation) =>
            {
                // 一番近くに居る敵
                Entity closestTargetEntity = Entity.Null;

                Entities
                    .WithAll<PlayerTag>()
                    .ForEach((Entity targetEntity, ref Translation targetTranslation) =>
                    {
                        // ターゲットしている敵がNULLなら
                        if(closestTargetEntity == Entity.Null)
                        {
                            // 一番近くの敵をターゲットに設定する
                            closestTargetEntity = targetEntity;
                        }
                    });
                // コンポーネントに設定する
                aimGunPort._targetEntity = closestTargetEntity;
            });
    }
}
