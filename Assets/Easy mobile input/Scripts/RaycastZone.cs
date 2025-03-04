using UnityEngine;
using UnityEngine.UI;

namespace EasyMobileInput
{
    public class RaycastZone : MaskableGraphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        [System.Obsolete]
        protected override void OnPopulateMesh(Mesh m)
        {
            m.Clear();
        }
    }
}