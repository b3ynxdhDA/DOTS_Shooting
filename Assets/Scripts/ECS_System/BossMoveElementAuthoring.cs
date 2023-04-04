using UnityEngine;
using Unity.Entities;

/// <summary>
/// ボスキャラクターの移動地点を取得するシステム
/// </summary>
public class BossMoveElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddBuffer<BossMoveElement>(entity);
    }
}
