using UnityEngine;

public abstract class Ability : MonoBehaviour
{
   #region Use Methods
   public abstract void Use(int dir);

   protected abstract void EndUse();
   #endregion
}
