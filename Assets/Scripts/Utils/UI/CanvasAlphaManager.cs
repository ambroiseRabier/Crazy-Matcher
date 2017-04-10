using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Utils
{
    public static class CanvasAlphaManager
    {

        #region Canvas Function

        public static void HideCanvas(GameObject canvas)
        {
            SetAlphaCanvas(canvas, 0);
        }

        public static void DisplayCanvas(GameObject canvas)
        {
            SetAlphaCanvas(canvas, 1);
        }

        public static void SetAlphaCanvas(GameObject canvas, float alpha)
        {
            foreach (Transform child in canvas.transform)
            {
                if (child.childCount != 0)
                {
                    SetAlphaCanvas(child.gameObject, alpha);
                }

                if (child.GetComponent<Image>() != null)
                {
                    SetAlphaComponent(child.GetComponent<Image>(), alpha);
                }
                else if (child.GetComponent<Text>() != null)
                {
                    SetAlphaComponent(child.GetComponent<Text>(), alpha);
                }
            }

        }

        #endregion


        #region Component

        public static void HideComponent(MaskableGraphic graphic)
        {
            SetAlphaComponent(graphic, 0);
        }

        public static void DisplayComponent(MaskableGraphic graphic)
        {
            SetAlphaComponent(graphic, 1);
        }

        public static void SetAlphaComponent(MaskableGraphic graphic, float alpha)
        {
            Color childColor;
            childColor = graphic.color;
            childColor.a = alpha;
            graphic.color = childColor;
        }

        #endregion

    }
}
