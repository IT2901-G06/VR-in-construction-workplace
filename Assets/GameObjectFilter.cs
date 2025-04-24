using Oculus.Interaction;
using UnityEngine;

public class GameObjectFilter : MonoBehaviour, IGameObjectFilter
{
    [SerializeField]
    private string _filterText;

    public bool Filter(GameObject gameObject)
    {
        Debug.Log("ye " + _filterText + " | " + gameObject.transform.parent.name + " | " + gameObject.transform.parent.name.Contains(_filterText));
        return gameObject.transform.parent.name.Contains(_filterText);
    }
}
