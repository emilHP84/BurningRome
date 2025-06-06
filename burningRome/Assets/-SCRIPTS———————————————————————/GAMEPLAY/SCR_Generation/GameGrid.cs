using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid access;
    [SerializeField] Transform grid;
    GridColumn[] gridColumns;

    void Start()
    {
        access = this;
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

    public void BurnCell(int column, int row, float duration)
    {
        BurnCell(column,row,duration,0,Cardinal.North,false);
    }

    public void BurnCell(int column, int row, float duration, int propagation, Cardinal direction, bool piercing)
    {
        Debug.Log("La case "+column+","+row+" a recu l'ordre de bruler pendant "+duration+" seconde en direction de "+direction);
        if (column < 0 || column > gridColumns.Length - 1 || row < 0 || row > gridColumns[column].gridRows.Length - 1)
        {
            return; //gridColumns[column].gridRows.Length-1
        }

        IFlamable targetCell = gridColumns[column].gridRows[row];
        if (targetCell == null) return;
        bool cellAuthorizePropagation = gridColumns[column].gridRows[row].BurnFor(duration);
        if (cellAuthorizePropagation || piercing) // Si la case autorise la propagation ou que la bombe est perçante
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
                BurnCell(column,row,duration,propagation,direction, piercing);
            }
        }
    }

} // FIN DU SCRIPT


[System.Serializable]
public class GridColumn
{
    public IFlamable[] gridRows;
}
