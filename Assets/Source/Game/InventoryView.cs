using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


namespace Source
{
  public class InventoryView : MonoBehaviour
  {
    [SerializeField] 
    private int _rows;
    [SerializeField] 
    private int _colls;
    [SerializeField]
    private GameObject _itemPrefab;
    
    private List<ItemAsset> _assets;
    private Dictionary<ItemView, GameObject> _items = new();
    private Dictionary<InventoryCell, CellView> _cells = new();
    private Inventory _inventory;
    private Random _random = new();

    public Inventory Inventory => _inventory;
    private void Start()
    {
      _inventory = new Inventory(_rows, _colls);
      Initiate();
    }

    private void Initiate()
    {
      LoadCells();
      LoadAssets();
      LoadButtons();
    }
    
    private void LoadAssets()
    {
      _assets = new List<ItemAsset>();
      var items = Resources.LoadAll<ItemAsset>("Items");
      _assets.AddRange(items);
      foreach (var asset in _assets)
      {
        SpawnItem(asset, asset.MaxStackSize);
        SpawnItem(asset, asset.MaxStackSize / 2);
      }
    }

    public void DestroyItem(ItemView view)
    {
      try
      {
        _inventory.RemoveItem(view.Item);
        Destroy(_items[view]);
        _items.Remove(view);
      }
      catch
      {
        Debug.Log("Destruction error");
      }
    }

    private void SpawnItem(ItemAsset itemAsset, int count)
    {
      InventoryItem item = new InventoryItem(itemAsset, count);
      bool located;
      (located, item) = _inventory.AddItem(item);
      if (located && item != null)
      {
        GameObject go = Instantiate(_itemPrefab);
        ItemView itemView = go.GetComponent<ItemView>();
        itemView.Initialize(item, this);
        _items.Add(itemView, go);
      }

      if (!located)
      {
        Debug.Log("Inventory full!!!");
      }
    }

    public InventoryItem SpawnItem(ItemAsset itemAsset, int count, InventoryCell cell)
    {
      InventoryItem item = new InventoryItem(itemAsset, count);
      _inventory.AddItem(item, cell);
      GameObject go = Instantiate(_itemPrefab);
      ItemView itemView = go.GetComponent<ItemView>();
      item.Count = count;
      itemView.Initialize(item, this);
      _items.Add(itemView, go);
      return item;
    }

    private void LoadCells()
    {
      foreach (var cell in GetComponentsInChildren<CellView>())
      {
        cell.Cell = _inventory.GetCell(cell.Position.Item1, cell.Position.Item2);
        _cells.Add(cell.Cell, cell);
        cell.ColliderEnable(false);
      }
    }

    public CellView GetCell(InventoryCell inventoryCell)
    {
      return _cells[inventoryCell];
    }

    private void LoadButtons()
    {
      foreach (var button in GetComponentsInChildren<ActionButton>())
      {
        button.onClick += ButtonClick;
      }
    }

    private void ButtonClick(Actions action)
    {
      switch (action)
      {
        case Actions.Sort:
          _inventory.Sort();
          break;
        case Actions.AddRandomItem:
          int asset = _random.Next(0, _assets.Count);
          int count = _random.Next(1, _assets[asset].MaxStackSize);
          SpawnItem(_assets[asset], count);
          break;
        case Actions.Clear:
          _inventory.Clear();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(action), action, null);
      }
    }

    public void CellCollidersEnable(bool enable)
    {
      foreach ((var invCell, var cellView) in _cells)
      {
        cellView.ColliderEnable(enable);
      }
    }

    public void ItemCollidersEnable(bool enable)
    {
      foreach (var item in _items)
      {
        item.Key.ColliderEnable(enable);
      }
    }
  }
}