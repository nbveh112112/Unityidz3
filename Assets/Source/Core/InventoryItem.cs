using System;

namespace Source
{
  public class InventoryItem : IEquatable<InventoryItem>
  {
    private ItemAsset _asset;
    private int _itemCount;
    private InventoryCell _cell;
    public event Action OnChange;

    public ItemAsset ItemAsset => _asset;
    public InventoryItem(ItemAsset asset, int count)
    {
      _asset = asset;
      _itemCount = count;
    }
    
    public int Count
    {
      get => _itemCount;
      set
      {
        _itemCount = value;
        OnChange?.Invoke();
      }
    }

    public InventoryCell Cell
    {
      get => _cell;
      set
      {
        _cell = value;
        OnChange?.Invoke();
      }
    }
    public int MaxCount => _asset.MaxStackSize;
    
    
    public static bool operator ==(InventoryItem other, InventoryItem one)
    {
      return Equals(one, other);
    }

    public static bool operator !=(InventoryItem other, InventoryItem one)
    {
      return !(other == one);
    }

    public bool Equals(InventoryItem other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(_asset, other._asset);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((InventoryItem)obj);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(_asset, _itemCount);
    }
  }
}