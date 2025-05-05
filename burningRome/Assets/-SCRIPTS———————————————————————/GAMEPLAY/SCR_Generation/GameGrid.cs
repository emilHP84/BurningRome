using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] Transform grid;
    GridColumn[] gridColumns;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        gridColumns = new GridColumn[grid.childCount];
        for (int i=0;i<gridColumns.Length;i++)
        {
            gridColumns[i] = new GridColumn();
            gridColumns[i].gridRows = new IFlamable[grid.GetChild(i).transform.childCount];
            for (int j=0; j< gridColumns[i].gridRows.Length; j++)
            {
                gridColumns[i].gridRows[j] = grid.GetChild(i).transform.GetChild(j).GetComponent<IFlamable>();
            }
        }
    }

    public void BurnCell(int column, int row, float duration, int propagation, Cardinal direction)
    {
        IFlamable targetCell = gridColumns[column].gridRows[row];
        if (targetCell == null) return;
        if (gridColumns[column].gridRows[row].BurnFor(duration) == true) // Si la case autorise la propagation
        {
            propagation--;
            if (propagation>=0)
            {
                switch (direction)
                {
                    case Cardinal.North: row--; break;
                    case Cardinal.South: row++; break;
                    case Cardinal.East: column++; break;
                    case Cardinal.West: column--; break;
                }
                BurnCell(column,row,duration,propagation,direction);
            }
        }
    }

} // FIN DU SCRIPT


[System.Serializable]
public class GridColumn
{
    public IFlamable[] gridRows;
}
