using Unity.Entities;

/// <summary>
/// ��������v���n�u���i�[����R���|�[�l���g
/// </summary>
[GenerateAuthoringComponent]
public struct SpawnerData : IComponentData 
{
    public Entity SpawnPrefabEntity;
}

