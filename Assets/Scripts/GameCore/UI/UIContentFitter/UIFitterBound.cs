using UnityEngine;

namespace GameCore.UI.UIContentFitter
{
   //"UI" is not InconsistentNaming
   // ReSharper disable once InconsistentNaming
   public class UIFitterBound : MonoBehaviour
   {
      public Vector3 bounds;

      private void OnDrawGizmos()
      {
         Gizmos.color = Color.red;
         Gizmos.DrawWireCube(transform.position,bounds);
      }
   }
}
