using Unity.Entities;

/// <summary>
/// 生成するプレハブを格納するコンポーネント
/// </summary>
[GenerateAuthoringComponent]
public struct SpawnerData : IComponentData 
{
    public Entity SpawnPrefabEntity;
}

