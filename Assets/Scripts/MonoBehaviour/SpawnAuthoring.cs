using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

/// <summary>
/// 駒のプレハブをエンティティに変換する
/// </summary>
[DisallowMultipleComponent]
public class SpawnAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    [SerializeField, Header("雑魚敵のベースプレハブ")]
    private GameObject _enemyBasePrefab;

    [SerializeField, Header("雑魚敵の弾のプレハブ")]
    private GameObject _enemyBulletPrefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // Spawner識別タグを追加
        dstManager.AddComponentData(entity, new Spawner());

        // 生成するプレハブを追加
        dstManager.AddComponentData(entity, new SpawnerData()
        {
            SpawnPrefabEntity = conversionSystem.GetPrimaryEntity(_enemyBasePrefab)
        });

        Debug.Log("convert");
        // Spawner識別タグを追加
        dstManager.AddComponentData(entity, new SpawnTag());
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(_enemyBulletPrefab);
    }

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

        entityManager.RemoveComponent<LinkedEntityGroup>(entityPrefab);

        entityManager.SetName(entityPrefab, "NormalEnemys");

        entityManager.SetName(bulletPrefab, "Bullet");

        entityManager.SetComponentData(entityPrefab, new GunPortTag
        {
            _BulletEntity = bulletPrefab
        });

        return entityPrefab;
    }
}