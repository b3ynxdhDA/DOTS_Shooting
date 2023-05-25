using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;

/// <summary>
/// 駒のプレハブをエンティティに変換する
/// </summary>
public class ConvertPrefabToEntity : MonoBehaviour
{
    [SerializeField, Header("雑魚敵のベースプレハブ")]
    private GameObject _enemyBasePrefab;

    [SerializeField, Header("雑魚敵の弾のプレハブ")]
    private GameObject _enemyBulletPrefab;

    public Entity GetEntityPrefab()
    {
        // デフォルトワールドを取得
        World defaultWorld = World.DefaultGameObjectInjectionWorld;
        // エンティティマネージャーを取得 
        EntityManager entityManager = defaultWorld.EntityManager;

        // GameObjectプレハブからEntityプレハブへの変換
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        Entity entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(_enemyBasePrefab, settings);
        Entity bulletPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(_enemyBulletPrefab, settings);

        entityManager.AddComponentData(entityPrefab, new PhysicsCollider { });

        entityManager.RemoveComponent<LinkedEntityGroup>(entityPrefab);

        entityManager.SetName(entityPrefab, "NormalEnemy");

        entityManager.SetComponentData(entityPrefab, new GunPortTag
        {
            _BulletEntity = bulletPrefab
        });

        return entityPrefab;
    }
}