using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// �{�X�L�����N�^�[�̈ړ��n�_���擾����V�X�e��
/// </summary>
public class BossMoveElementAuthorings : MonoBehaviour, IConvertGameObjectToEntity
{
    public float3[] valueArray;

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        DynamicBuffer<BossMoveElement> dynamicBuffer = entityManager.AddBuffer<BossMoveElement>(entity);
        
        foreach(float3 value in valueArray)
        {
            dynamicBuffer.Add(new BossMoveElement { _movePoints = value });
        }
    }
}
