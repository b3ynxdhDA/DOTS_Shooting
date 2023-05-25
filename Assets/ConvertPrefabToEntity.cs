using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;

/// <summary>
/// ��̃v���n�u���G���e�B�e�B�ɕϊ�����
/// </summary>
public class ConvertPrefabToEntity : MonoBehaviour
{
    [SerializeField, Header("�G���G�̃x�[�X�v���n�u")]
    private GameObject _enemyBasePrefab;

    [SerializeField, Header("�G���G�̒e�̃v���n�u")]
    private GameObject _enemyBulletPrefab;

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