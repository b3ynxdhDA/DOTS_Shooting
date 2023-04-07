using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// ボスキャラクターの移動地点を取得するシステム
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
