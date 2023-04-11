using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create KomaData")]
public class KomaData : ScriptableObject
{
    /// <summary>
    /// 駒の見た目のスプライト
    /// </summary>
    public Sprite sprite;

    /// <summary>
    /// 駒の体力
    /// </summary>
    public float hp;

    /// <summary>
    /// 発射する弾の種類のステート
    /// </summary>
    public enum ShootKind
    {
        StraightGunPortTag,
        WideGunPortTag,
        AimGunPortTag
    }

    /// <summary>
    /// 発射する弾の種類
    /// </summary>
    public ShootKind shootKind;

    /// <summary>
    /// 発射する弾の列数
    /// </summary>
    public int shootLine;

    /// <summary>
    /// 発射する弾のスピード
    /// </summary>
    public float bulletSpeed;

    /// <summary>
    /// 射撃のクールタイム
    /// </summary>
    public float shootCoolTime;
}
