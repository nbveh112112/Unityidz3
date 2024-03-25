namespace Source
{
  public class InventoryCell
  {
    private (int, int) _position;
    private InventoryItem _item = null;
    
    public InventoryItem Item
    {
      get => _item;
      set => _item = value;
    }

    public (int, int) Position
    {
      get => _position;
      set => _position = value;
    }
    
    
  }
}