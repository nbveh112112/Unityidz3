using System;
using UnityEngine;

namespace Source
{
  public class ActionButton : MonoBehaviour
  {
    [SerializeField]
    private Actions _action;

    public Action<Actions> onClick;

    public void OnClick()
    {
      onClick?.Invoke(_action);
    }

  }
}