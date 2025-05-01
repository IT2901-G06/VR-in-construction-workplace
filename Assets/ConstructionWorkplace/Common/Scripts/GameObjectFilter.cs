using Oculus.Interaction;
using UnityEngine;

public class GameObjectFilter : MonoBehaviour, IGameObjectFilter
{
    [SerializeField]
    private string _filterText;

    public bool Filter(GameObject gameObject)
    {
        return gameObject.transform.parent.name.Contains(_filterText);
    }
}
