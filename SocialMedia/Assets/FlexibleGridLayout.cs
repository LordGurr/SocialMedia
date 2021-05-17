using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }

    public FitType fitType;

    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;

    public bool fitX;
    public bool fitY;
    [SerializeField] private UnityEvent GridLayoutLoaded;
    private bool Again = false;

    public override void CalculateLayoutInputVertical()
    {
        RectTransform recttransform = gameObject.GetComponent<RectTransform>();
        base.CalculateLayoutInputHorizontal();

        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            fitX = true;
            fitY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns || fitType == FitType.Uniform)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows || fitType == FitType.Uniform)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * (rows - 1)) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
        rectTransform.localPosition = new Vector2(0, 0);
        recttransform.sizeDelta = new Vector2(recttransform.sizeDelta.x, (rows * ((recttransform.rect.width - 40) / 3)) + (rows * 20));
        if (Again)
        {
            Again = false;
            CalculateLayoutInputVertical();
        }
        if (GridLayoutLoaded != null)
        {
            StartCoroutine(finishedLoadingGridLayout());
        }
    }

    private IEnumerator finishedLoadingGridLayout()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return null;
        }
        Debug.Log("Grid finished");
        GridLayoutLoaded.Invoke();
    }

    public void VäntaLite()
    {
        Again = true;
        StartCoroutine(RiktigVänta());
    }

    private IEnumerator RiktigVänta()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return null;
        }
        CalculateLayoutInputVertical();
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}