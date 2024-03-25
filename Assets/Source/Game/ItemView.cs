using System;
using TMPro;
using UnityEngine;

namespace Source
{
  public class ItemView : MonoBehaviour
  {
    private bool _isDestroyed;
    [SerializeField]
    private TextMeshPro _textMeshPro;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private BoxCollider2D _boxCollider2D;
    [SerializeField]
    private Transform _transform;

    private InventoryItem _item;
    private InventoryView _inventory;

    public void Initialize(InventoryItem inventoryItem, InventoryView inventoryView)
    {
      _isDestroyed = false;
      Item = inventoryItem;
      _spriteRenderer.sprite = Item.ItemAsset.Sprite;
      _inventory = inventoryView;
      OnUpdate();
    }

    public InventoryItem Item
    {
      get => _item;
      set
      {
        if (_item != null) _item.OnChange -= OnUpdate;
        _item = value;
        _item.OnChange += OnUpdate;
      }
    }

    private void OnUpdate()
    {
      if (Item.Count == 0)
      {
        if (!_isDestroyed)
          _inventory.DestroyItem(this);
        _isDestroyed = true;
      }
      else
      {
        _textMeshPro.text = (Item.Count == 1) ? "" : Item.Count.ToString();
        _transform.position = _inventory.GetCell(_item.Cell).Transform.position;
      }
    }
    
    public void ColliderEnable(bool enable)
    {
      _boxCollider2D.enabled = enable;
    }
    
    private Vector3 _position;
    private bool _isHeld;
    private bool _isSplited;
    private InventoryItem _halfItem;
    public void OnMouseDown()
    {
      if (Input.GetMouseButtonDown(0))
      {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && _item.Count > 1)
        {
          _isSplited = true;
          _item.Cell.Item = null;
          _halfItem = _inventory.SpawnItem(_item.ItemAsset, (_item.Count / 2) + (_item.Count % 2 == 0 ? 0 : 1), _item.Cell);
          _item.Count /= 2;
        }
        _spriteRenderer.sortingOrder = 1;
        Vector3 mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        _position = mousePos;
        _inventory.CellCollidersEnable(true);
        _inventory.ItemCollidersEnable(false);
        _isHeld = true;
      }
    }
    
    public void OnMouseDrag()
    {
      if (_isHeld)
      {
        Vector3 mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        _transform.position += (mousePos - _position);
        _position = mousePos;
      }
    }
    
    public void OnMouseUp()
    {
      if (_isHeld)
      {
        _spriteRenderer.sortingOrder = 0;
        Vector3 mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(mousePos.x, mousePos.y), new Vector2(mousePos.x, mousePos.y));
        if (hit.collider != null)
        {
          CellView cl = hit.collider.GetComponentInParent<CellView>();
          if (_isSplited)
          {
            _inventory.Inventory.MoveItem(_halfItem, _halfItem.Cell);
            if (cl == null )
            {
              _inventory.Inventory.MoveItem(_item, _halfItem.Cell);
            }
            else if (cl.Cell.Item == _item)
            {
              
              _inventory.Inventory.MoveItem(_item, cl.Cell);
              _inventory.Inventory.MoveItem(_item, _halfItem.Cell);
            }
            else
            {
              if (cl.Cell.Item == null)
              {
                _inventory.Inventory.MoveItem(_item, cl.Cell);
              }
              else
              {
                _inventory.Inventory.MoveItem(_item, _halfItem.Cell);
              }
            }
          }
          else
          {
            if (cl != null)
            {
              _inventory.Inventory.MoveItem(_item, cl.Cell);
            }
          }
        }
        else if (_isSplited)
        {
          _inventory.Inventory.MoveItem(_halfItem, _halfItem.Cell);
          _inventory.Inventory.MoveItem(_item, _halfItem.Cell);
        }
        _inventory.CellCollidersEnable(false);
        _inventory.ItemCollidersEnable(true);
        _isHeld = false;
        _isSplited = false;
      }
      OnUpdate();
    }
  }
}