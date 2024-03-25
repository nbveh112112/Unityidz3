using System;
using System.Collections.Generic;
using UnityEngine.UIElements;


namespace Source
{
  public class Inventory
  {
    private InventoryCell[,] _cells;
    private (int, int) _size;

    public Inventory(int x, int y)
    {
      _size = (x, y);
      _cells = new InventoryCell[x, y];
      for (int i = 0; i < x; ++i)
      {
        for (int j = 0; j < y; ++j)
        {
          _cells[i, j] = new InventoryCell();
          _cells[i, j].Position = (i, j);
        }
      }
    }

    public void Sort()
    {
      List<InventoryItem> items = new();
      (int x, int y) = _size;
      for (int i = 0; i < x; ++i)
      {
        for (int j = 0; j < y; ++j)
        {
          InventoryItem item = _cells[i, j].Item;
          if (item != null)
          {
            items.Add(item);
          }
        }
      }
      items.Sort((x, y) => {
        if (x == y)
        {
          return y.Count.CompareTo(x.Count);
        }
        return String.Compare(x.ItemAsset.Name, y.ItemAsset.Name, StringComparison.Ordinal);
      });
      int counter = 0;
      for (int i = 0; i < x; ++i)
      {
        for (int j = 0; j < y; ++j)
        {
          if (counter < items.Count)
          {
            items[counter].Cell = _cells[i, j];
            _cells[i, j].Item = items[counter];
          }
          else
          {
            _cells[i, j].Item = null;
          }
          counter++;
        }
      }
    }

    public void Clear()
    {
      (int x, int y) = _size;
      for (int i = 0; i < x; ++i)
      {
        for (int j = 0; j < y; ++j)
        {
          InventoryItem item = _cells[i, j].Item;
          if (item != null)
          {
            item.Count = 0;
          }
          _cells[i, j].Item = null;
        }
      }
    }

    public InventoryCell GetCell(int x, int y)
    {
      return _cells[x, y];
    }

    public void RemoveItem(InventoryItem item)
    {
      (int x, int y) = _size;
      for (int i = 0; i < x; ++i)
      {
        for (int j = 0; j < y; ++j)
        {
          if (ReferenceEquals(_cells[i, j].Item, item))
          {
            _cells[i, j].Item = null;
          }
        }
      }
    }

    public void MoveItem(InventoryItem item, InventoryCell cell)
    {
      if (item.Cell == cell)
      {
        if (!ReferenceEquals(cell.Item, item))
        {
          int adiing = Math.Min(item.Count, cell.Item.MaxCount - cell.Item.Count);
          item.Count -= adiing;
          cell.Item.Count += adiing;
        }
        return;
      }
      if (cell.Item == null) {
        if (ReferenceEquals(item.Cell.Item, item))
        {
          item.Cell.Item = null;
        }
        item.Cell = cell;
        cell.Item = item;
      } else {
        if (cell.Item == item)
        {
          int adiing = Math.Min(item.Count, cell.Item.MaxCount - cell.Item.Count);
          item.Count -= adiing;
          cell.Item.Count += adiing;
        }
        else
        {
          (cell.Item, item.Cell.Item, item.Cell, cell.Item.Cell) = (item.Cell.Item, cell.Item, cell.Item.Cell, item.Cell);
        }
      }
    }

    public void AddItem(InventoryItem item, InventoryCell cell)
    {
      if (cell.Item != null)
      {
        throw new Exception("Cell is not empty");
      }

      cell.Item = item;
      item.Cell = cell;
    }
    
    public (bool, InventoryItem) AddItem(InventoryItem item)
    {
      (int x, int y) = _size;
      InventoryCell emptycell = null;
      for (int i = 0; i < x; ++i)
      {
        for (int j = 0; j < y; ++j)
        {
          InventoryItem currentItem = _cells[i, j].Item;
          if (emptycell == null && currentItem == null)
          {
            emptycell = _cells[i, j];
          }
          if (currentItem == item)
          {
            if (currentItem!.Count < currentItem.MaxCount)
            {
              int adiing = Math.Min(item.Count, currentItem.MaxCount - currentItem.Count);
              item.Count -= adiing;
              currentItem.Count += adiing;
            }
            if (currentItem.Count == 0)
            {
              break;
            }
          }
        }
      }
      if (item.Count == 0)
      {
        return (true, null);
      }
      if (emptycell != null)
      {
        emptycell.Item = item;
        item.Cell = emptycell;
        return (true, item);
      }
      
      return (false, null);
    }
  }
}