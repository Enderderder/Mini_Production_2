using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface KillableEntity
{
    void TakeDamage(float _fDamage);
    void Dead();
}
