using UnityEngine;

namespace Source
{
  [CreateAssetMenu(fileName = "ItemInfo", menuName = "GameObjects/ItemInfo")]
  public class ItemAsset  : ScriptableObject
  {
    [SerializeField] 
    private string _name;
    [SerializeField] 
    private Sprite _sprite;
    [SerializeField]
    private int _maxStackSize;
    
    public string Name => _name;
    public Sprite Sprite => _sprite;
    public int MaxStackSize => _maxStackSize;
  }
}