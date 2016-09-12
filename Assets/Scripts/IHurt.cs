using UnityEngine;

public interface IHurt {
    /// <summary>
    /// Hurt this actor
    /// </summary>
    /// <param name="point">Point of impact to hurt the actor</param>
    void Hurt(ContactPoint2D point);
}