namespace Treasure.Common
{
    using UnityEngine;

    public static class PanelSizeAdjuster
    {
        public static Vector2 AdjustedPanelSize(Vector2 panelSize, Vector2 minPanelSize)
        {
            float reductionMargin = 2;
            panelSize.x -= reductionMargin;
            panelSize.y -= reductionMargin / 2;

            panelSize.x = Mathf.Clamp(panelSize.x, minPanelSize.x, 700);
            panelSize.y = Mathf.Clamp(panelSize.y, minPanelSize.y, 100);

            return panelSize;
        }
    }
}
