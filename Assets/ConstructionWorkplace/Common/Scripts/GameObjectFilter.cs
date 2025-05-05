using Oculus.Interaction;
using UnityEngine;

/// <summary>
/// This class filters GameObjects based on a the name of their parent.
/// </summary>
public class GameObjectFilter : MonoBehaviour, IGameObjectFilter
{
    [SerializeField]
    [Tooltip("The text to filter GameObjects by. Only GameObjects whose parent name contains this text will be included.")]
    private string _filterText;

    public bool Filter(GameObject gameObject)
    {
        return gameObject.transform.parent.name.Contains(_filterText);
    }
}
