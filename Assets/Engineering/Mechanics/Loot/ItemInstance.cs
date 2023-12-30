using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : MonoBehaviour, IInteractable
{
    public InteractableType Type { get; set; } = InteractableType.Item;
    [SerializeField] Item item;

    public Item Item => item;

    [TitleGroup("Layer Management")][SerializeField] LayerMask outlineMask;
    [TitleGroup("Layer Management")][SerializeField] LayerMask defaultMask;
    [SerializeField] protected GameObject[] gameObjectsToLayerChange;

    [SerializeField][ReadOnly] bool _currentlyHighlighted;
    [SerializeField] float highlightPollRate = 0.0333f;

    protected bool highlightActive = true; 

    int _defaultMask;
    int _outlineMask;

    bool highlight;
    float highlightPollTimer;

    public event EventHandler<InteractEventArgs> OnInteract;

    #region Properties
    int itemId;
    string itemName;
    ItemTypes itemType;

    public int Id => itemId;
    public string Name => itemName;
    public ItemTypes ItemType => itemType;
    #endregion



    public virtual void Start() {
        Debug.Assert(((int)outlineMask).IsPowerOfTwo(), "Outline mask must only include one layer");
        Debug.Assert(((int)defaultMask).IsPowerOfTwo(), "Default mask must only include one layer");
        _defaultMask = (int)Mathf.Log(defaultMask, 2);
        _outlineMask = (int)Mathf.Log(outlineMask, 2);

        _currentlyHighlighted = false;
        gameObject.layer = _defaultMask;

        if (item != null) {
            item.CreateItemInstance(this.gameObject);
        }
    }

    public void InitializeItem(Item thisItem) {
        this.itemId = thisItem.ItemId;
        this.itemName = thisItem.ItemName;
        this.itemType = thisItem.ItemType;
    }


    private void LateUpdate() {
        if (!highlightActive) return;
        if (highlightPollTimer < highlightPollRate) {
            highlightPollTimer += Time.deltaTime;
            return;
        }

        if (highlight) {
            if (!_currentlyHighlighted) {
                _currentlyHighlighted = true;
                SetHighlightLayers(_outlineMask);
            }
        }
        else {
            if (_currentlyHighlighted) {
                _currentlyHighlighted = false;
                SetHighlightLayers(_defaultMask);
            }
        }

        highlightPollTimer = 0;
        highlight = false;
    }


    public virtual void Highlight() {
        highlight = true;
    }

    public virtual void SetHighlightLayers(int layer) {
        for (int i = 0; i < gameObjectsToLayerChange.Length; i++) {
            GameObject go = gameObjectsToLayerChange[i];
            go.layer = layer;
        }
    }
    public virtual void Interact() {
        Debug.Log("Interacted with item of name: " + this.Name);
        OnInteract?.Invoke(this, new InteractEventArgs());
    }

    public virtual void DestroyAfterDelay(float delay) {
        Destroy(this.transform.root.gameObject, delay);
    }
}

public class InteractEventArgs : EventArgs
{
}