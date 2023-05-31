using UnityEngine;
using Unity.Entities;
using System.Collections.Generic;

/// <summary>
/// ��̃v���n�u���G���e�B�e�B�ɕϊ�����
/// </summary>
[DisallowMultipleComponent]
public class SpawnAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    [SerializeField, Header("�G���G�̃x�[�X�v���n�u")]
    private GameObject _enemyBasePrefab;

    [SerializeField, Header("�G���G�̒e�̃v���n�u")]
    private GameObject _enemyBulletPrefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // Spawner���ʃ^�O��ǉ�
        dstManager.AddComponentData(entity, new Spawner());

        // ��������v���n�u��ǉ�
        dstManager.AddComponentData(entity, new SpawnerData()
        {
            SpawnPrefabEntity = conversionSystem.GetPrimaryEntity(_enemyBasePrefab)
        });

        Debug.Log("convert");
        // Spawner���ʃ^�O��ǉ�
        dstManager.AddComponentData(entity, new SpawnTag());
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(_enemyBulletPrefab);
    }

    public Entity GetEntityPrefab()
    {
        // �f�t�H���g���[���h���擾
        World defaultWorld = World.DefaultGameObjectInjectionWorld;
        // �G���e�B�e�B�}�l�[�W���[���擾 
        EntityManager entityManager = defaultWorld.EntityManager;

        // GameObject�v���n�u����Entity�v���n�u�ւ̕ϊ�
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