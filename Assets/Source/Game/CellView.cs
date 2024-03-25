using UnityEngine;

namespace Source
{
  public class CellView : MonoBehaviour
  {
    [SerializeField] 
    private int _row;
    [SerializeField] 
    private int _coll;
    [SerializeField]
    private BoxCollider2D _boxCollider2D;
    [SerializeField] 
    private Transform _transform;

    private InventoryCell _cell;
    
    public (int, int) Position => (_row, _coll);
    public Transform Transform => _transform;
    public InventoryCell Cell
    {
      get => _cell;
      set => _cell = value;
    }
    
    public void ColliderEnable(bool enable)
    {
      _boxCollider2D.enabled = enable;
    }
  }
}