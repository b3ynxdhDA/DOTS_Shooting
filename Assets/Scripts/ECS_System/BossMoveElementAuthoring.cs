using UnityEngine;
using Unity.Entities;

/// <summary>
/// �{�X�L�����N�^�[�̈ړ��n�_���擾����V�X�e��
/// </summary>
public class BossMoveElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddBuffer<BossMoveElement>(entity);
    }
}
