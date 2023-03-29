using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// オートエイム弾のコンポーネントタグ
/// </summary>
[GenerateAuthoringComponent]
public struct AutoAimBulletTag : IComponentData
{
    // ターゲットへの角度
    [HideInInspector]public float3 _moveDirection;
}
